using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[SelectionBase]
public class player : MonoBehaviour
{
    [Header("movement")]

    [SerializeField] private float movementSpeed;

    [Header("dash")]

    [SerializeField] private Keybind dashKey;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCost;
    [SerializeField] private ParticleSystem Particles;
    [Space]
    [SerializeField] private AnimationCurve dashSpeedFade;

    [Header("health")]

    [Range(0f, 100f)]
    [SerializeField] private float health;
    [SerializeField] private float defaultHealth;
    [SerializeField] private Ranged sizeRange;
    

    [Space]

    [SerializeField] private float IFramesTime;
    [SerializeField] private int IFrameFlashAmount;
    [SerializeField] private float IFrameFlashOffTime;
    [SerializeField, Range(0, 255)] private byte IFrameOpacity;

    [Space]

    [SerializeField] private float regenWaitTime;
    private float regenWaitTimer;
    [SerializeField] private float regenRate;
    private float regenRateTimer;
    [SerializeField] private float regenAmount;

    [Space]

    [SerializeField] private float KBImmobilityTime;
    private bool isKnockedBack;

    [Header("gun mode")]

    [SerializeField] private Keybind gunModeKey;
    [SerializeField] private float gunModeSpeed;
    [SerializeField] private float gunModeBulletSpeed;
    [SerializeField] private float gunCooldown;
    [SerializeField] private GameObject bubbleBulletToSpawn;
    [SerializeField] private Sprite[] bubbleVariations;
    [SerializeField] private float gunUseCost;
    [SerializeField] private Ranged CaptureBubbleRotation;

    [Header("Animation")]

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject gfxCont;

    [Space]

    [SerializeField] private SpriteRenderer body;

    [Space]

    [SerializeField] private Vector2 faceMovementRange;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private float faceMovementSpeed;
    [SerializeField] private Animator CameraAnim;

    [Header("Menuing")]

    public Keybind selectButtonKey;

    public List<IMenuInteractable> buttonsOnMeAAA = new List<IMenuInteractable>();
    private IMenuInteractable currentButton = null;

    public bool isInUI = true;
    public bool isInMainMenu = true;

    [Header("Sound")]

    [SerializeField] private AudioClip dahsSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hurtSound;


    //private members

    private float maxHealth = 100;

    private Rigidbody2D rb;

    private bool canMove = true;

    private bool isDashing;
    private bool isInvincible;
    private bool isGun;
    private bool isGunCooldownOver = true;
    private bool isDead;
    public bool IsDead { get { return isDead; } }

    private void Start()
    {
        UpdateScaleByHealth();
        rb = GetComponent<Rigidbody2D>();
        resetRegen();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnockedBack)
        {
            if (canMove)
                Movement();
            else
                rb.linearVelocity = Vector3.zero;
        }

        if (isInUI)
        {
            MenuingHandler();
        }

        if (isDead) return;

        if (GameManager.GetInputDown(dashKey))
            dash();

        AnimationsHandler();

        gunMode();

        regenHandler();
    }

    void Movement()
    {
        Vector2 rawMovement = getRawMovement();

        float currentSpeed = movementSpeed;

        if (isGun)
            currentSpeed = gunModeSpeed;

        rb.linearVelocity = rawMovement * currentSpeed;
    }

    async void dash()
    {
        if (isDashing || isGun || isDead) return;

        if (health > dashCost)
            AddHealth(-dashCost);
        else
        {
            onAbilityFailed();
            return;
        }

        animator.Play("PlayerDash");
        Particles.Play();

        SoundManager.getSoundManager().CreateSoundEffect("dash", dahsSound);

        isDashing = true;

        canMove = false;

        Vector2 dashDir = getRawMovement();

        float fadedSpeed = dashSpeed;

        for (float t = 0; t < dashTime; t += Time.deltaTime)
        {
            fadedSpeed = dashSpeedFade.Evaluate(t / dashTime) * dashSpeed;

            rb.linearVelocity = dashDir * fadedSpeed;

            await Task.Yield();
        }

        isDashing = false;
        canMove = true;
    }

    Vector2 getRawMovement()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    void SetHealth(float newHealth)
    {
        this.health = Mathf.Clamp(newHealth, 0, maxHealth);

        UpdateScaleByHealth();
    }

    public void AddHealth(float health)
    {
        if (isInMainMenu || isDead) return;

        this.health += health;

        this.health = Mathf.Clamp(this.health, 0, maxHealth);

        UpdateScaleByHealth();
    }

    void UpdateScaleByHealth()
    {
        var scale = transform.localScale;

        float scaleTime = 0;
        if (this.health > 0)
            scaleTime = this.health / maxHealth;

        scale.x = sizeRange.Lerp(scaleTime);
        scale.y = sizeRange.Lerp(scaleTime);

        transform.localScale = scale;
    }

    public void OnDamage(DamageInfo damage, Transform KBOrigin = null)
    {
        if (isInvincible || isDashing || isKnockedBack || isDead) return;

        AddHealth(-damage.damageAmount);
        GameManager.get().playCameraShake();
        Particles.Play();

        if (this.health <= 0)
        {
            OnDeath();
            return;
        }
        SoundManager.getSoundManager().CreateSoundEffect("hurt", hurtSound);
        ActivateIFrames();

        resetRegen();
        
        if (KBOrigin != null)
            addKB(KBOrigin.position, damage.KBStrength);

        //add some damage effect idk lol :skull:
    }

    void addKB(Vector3 origin, float strength)
    {
        Vector3 direction = (this.transform.position - origin).normalized;

        rb.AddForce(direction * strength);

        StartCoroutine(KBImmobileTimer());
        IEnumerator KBImmobileTimer()
        {
            isKnockedBack = true;
            yield return new WaitForSeconds(KBImmobilityTime);
            isKnockedBack = false;
        }
    }

    async void ActivateIFrames()
    {
        if (isInvincible) return;

        isInvincible = true;

        float flashRate = IFramesTime / IFrameFlashAmount - IFrameFlashOffTime;

        for (int i = 0; i < IFrameFlashAmount; i++)
        {
            //turn off
            var bCol = body.color;
            bCol.a = ((float)IFrameOpacity) / 255f;
            body.color = bCol;
            face.color = new Color32(255, 255, 255, IFrameOpacity);
            await Task.Delay((int)(IFrameFlashOffTime * 1000));
            //turn on
            bCol = body.color;
            bCol.a = 1;
            body.color = bCol;
            face.color = new Color32(255, 255, 255, 255);

            await Task.Delay((int)(flashRate * 1000));
        }

        isInvincible = false;
    }

    void AnimationsHandler()
    {
        face.transform.localPosition = Vector2.Lerp(face.transform.localPosition, getRawMovement() * faceMovementRange, Time.deltaTime * faceMovementSpeed);
    }

    void gunMode(bool forceOff = false)
    {
        if (isDashing || isKnockedBack) return;

        if (GameManager.GetInputDown(gunModeKey))
        {
            //turn to gun
            isGun = true;
        }
        else if (!GameManager.GetInput(gunModeKey))
        {
            //back to normal
            isGun = false;
        }

        if (forceOff) isGun = false;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        animator.SetBool("isInGun", isGun);

        face.gameObject.SetActive(!isGun);

        if (isGun)
        {
            Vector3 diff = mouseWorldPos - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            body.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

            var flipperScale = body.transform.parent.transform.localScale;
            flipperScale.y = body.transform.eulerAngles.z <= 270 && body.transform.eulerAngles.z >= 90 ? -1 : 1;
            body.transform.parent.transform.localScale = flipperScale;
        }
        else
        {
            body.transform.rotation = Quaternion.identity;
        }

        if (isGun && Input.GetMouseButtonDown(0) && isGunCooldownOver)
        {
            if (health > gunUseCost)
                AddHealth(-gunUseCost);
            else
            {
                onAbilityFailed();
                return;
            }

            StartCoroutine(GunCooldown());
            animator.Play("PlayerGun");

            SoundManager.getSoundManager().CreateSoundEffect("shoot", shootSound);

            GameObject bubbleBullet = Instantiate(bubbleBulletToSpawn, transform.position, Quaternion.identity);
            GameManager.get().getLevelManager().addToCleanup(bubbleBullet.transform);
            BulletCapture bulletCap = bubbleBullet.GetComponent<BulletCapture>();
            bubbleBullet.GetComponent<SpriteRenderer>().sprite = bubbleVariations[Random.Range(0, bubbleVariations.Length)];
            bulletCap.rb.AddTorque(CaptureBubbleRotation.newRandom);
            bulletCap.send(body.transform.right);
        }
    }

    IEnumerator GunCooldown()
    {
        isGunCooldownOver = false;
        yield return new WaitForSeconds(gunCooldown);
        isGunCooldownOver = true;
    }

    void resetRegen()
    {
        regenWaitTimer = regenWaitTime;
        regenRateTimer = 0;
    }

    void regenHandler()
    {
        if (isDashing || isKnockedBack || isGun || getRawMovement() != Vector2.zero)
        {
            resetRegen();
            return;
        }

        if (regenWaitTimer > 0)
        {
            regenWaitTimer -= Time.deltaTime;
            return;
        }

        if (regenRateTimer > 0)
        {
            regenRateTimer -= Time.deltaTime;
            return;
        }

        AddHealth(regenAmount);
        regenRateTimer = regenRate;
    }

    void MenuingHandler()
    {
        if (GameManager.GetInputDown(selectButtonKey) && buttonsOnMeAAA.Count != 0)
        {
            currentButton = buttonsOnMeAAA[0];
            buttonsOnMeAAA[0].onInteractEnter(this);
        }
        else if (GameManager.GetInputUp(selectButtonKey) && currentButton != null && buttonsOnMeAAA.Count != 0)
        {
            IMenuInteractable tempI = buttonsOnMeAAA[0];
            buttonsOnMeAAA.RemoveAt(0);
            buttonsOnMeAAA.Add(tempI);

            currentButton.onInteractExit(this);
        }
    }

    public void OnMenuInteractable(IMenuInteractable interactable, bool entered)
    {
        if (entered)
            buttonsOnMeAAA.Add(interactable);
        else if (buttonsOnMeAAA.Contains(interactable))
        {
            if (currentButton == interactable)
                currentButton.onInteractExit(this);

            buttonsOnMeAAA.Remove(interactable);
        }
    }


    private void OnDeath()
    {
        if (isDead) return;

        isDead = true;
        canMove = false;
        isInUI = true;
        animator.Play("PlayerDeath");
        face.enabled = false;

        SoundManager.getSoundManager().CreateSoundEffect("death", deathSound);

        GameManager.get().getMenuManager().pullScoreMenu();
    }

    //revives the player after death.
    public void revive()
    {
        if (!isDead) return;

        isDead = false;
        canMove = true;
        face.enabled = true;

        animator.Play("PlayerWalk");

        GameManager.get().getMenuManager().pullBackScoreMenu();

        SetHealth(defaultHealth);
    }

    //activates when an ability fails to activate, usually by not having enough health.
    public void onAbilityFailed()
    {

    }
}