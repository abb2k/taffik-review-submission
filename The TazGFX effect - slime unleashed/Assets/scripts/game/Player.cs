using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [BoxGroup("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [BoxGroup("Movement")]
    [SerializeField] private int dashTime;

    private Vector2 movement;

    [BoxGroup("Movement")]
    public bool canMove;

    private bool canDash;

    [BoxGroup("Movement")]
    [SerializeField] private float dashSpeed;

    [BoxGroup("Movement")]
    [SerializeField] private Rigidbody2D rb;

    [BoxGroup("Movement")]
    [SerializeField] private bool IsDashing;

    [BoxGroup("Movement")]
    [SerializeField] private GameObject MainCamera;

    private bool IsMoving;

    private bool InAir;

    [BoxGroup("Movement")]
    public bool InJump;

    [BoxGroup("Movement")]
    public bool DisableInput;

    [BoxGroup("Movement")]
    [SerializeField] private CircleCollider2D solidHitbox;

    [BoxGroup("Movement")]
    public bool collision;

    [BoxGroup("Movement")]
    [SerializeField] private bool LockInfiniJump;

    [Space]

    [BoxGroup("PooEffect")]
    public GameObject slimypoopoo;
    [BoxGroup("PooEffect")]
    public float burntScale;
    [HideInInspector]
    public Transform theSewer;

    [Space]

    [BoxGroup("Health Managment")]
    [Range(0, 100)]
    [SerializeField] private int health;

    [BoxGroup("Health Managment")]
    [SerializeField] private RectTransform HealthBar;

    [BoxGroup("Health Managment")]
    [SerializeField] private int DamageReduction;

    [BoxGroup("Health Managment")]
    [SerializeField] private int knockBackReduction;

    [BoxGroup("Health Managment")]
    [SerializeField] private float HealthBarLerpTime;

    [BoxGroup("Health Managment")]
    [SerializeField] private float HitEffectTime;

    [Space]

    [BoxGroup("Attack")]
    [SerializeField] private int Damage;

    [BoxGroup("Attack")]
    [SerializeField] private int knockBack;

    [BoxGroup("Attack")]
    [SerializeField] private int SpinDamage;

    [BoxGroup("Attack")]
    [SerializeField] private int SpinKnockBack;

    [BoxGroup("Attack")]
    public GameObject swordHolder;

    [BoxGroup("Attack")]
    [SerializeField] private Animator sword;

    [BoxGroup("Attack")]
    public bool swinging;

    [BoxGroup("Attack")]
    [SerializeField] private bool isSpinning;

    [BoxGroup("Attack")]
    [SerializeField] private GameObject SpinBox;

    [Space]

    [BoxGroup("Animation")]
    public Animator animator;

    [BoxGroup("Animation")]
    [SerializeField] private GameObject PlayerCanvas;

    [BoxGroup("Animation")]
    [SerializeField] private float InvincibilityTime;
    private bool invincable;

    [BoxGroup("Animation")]
    [SerializeField] private int InvincibilityFlashingAmount;

    [BoxGroup("Animation")]
    [SerializeField] private List<SpriteRenderer> spritesToFlash;

    [BoxGroup("Animation")]
    [SerializeField] private GameObject GlobalVolume;

    [BoxGroup("Animation")]
    [SerializeField] private Light2D MyLight;

    [HideInInspector]
    public SpriteRenderer sr;

    private GameManager GM;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(MainCamera);
        DontDestroyOnLoad(GlobalVolume);
    }

    private static Player inctance;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Cursor.lockState = CursorLockMode.Confined;
        GM = GameManager.getShared();

        if (inctance == null)
        {
            inctance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        solidHitbox.enabled = collision;
        if (!theSewer)
        {
            GameObject slimePooPooContainer = Instantiate(GM.EmptyObj);
            slimePooPooContainer.transform.position = Vector3.zero;
            slimePooPooContainer.name = "slime poo poo container";
            theSewer = slimePooPooContainer.transform;
        }

        Movement();
        
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }

        UpdateHealthBar(health);
        SwordManag();

        if (Input.GetKeyDown(KeyCode.Space) && !swinging && !IsDashing && !isSpinning && !InAir && !IsMoving && !DisableInput)
        {
            SpinAttack();
        }

        SceneManagment();
    }

    void Movement()
    {
        if (swinging || DisableInput)
        {
            movement.x = 0;
            movement.y = 0;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        if (movement.x == 0 && movement.y == 0 && !DisableInput)
        {
            animator.SetBool("isJumping", false);
            IsMoving = false;
        }
        else
        {
            if (!LockInfiniJump)
            {
                IsMoving = true;
                animator.SetBool("isJumping", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isSpinning && !DisableInput)
        {
            animator.SetBool("isDashing", true);

            canDash = false;
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

    }

    public void AllowMovement()
    {
        canMove = true;
        InAir = true;
    }
    public void DisableMovement()
    {
        canMove = false;
        InAir = false;
    }
    public void slimyShit()
    {
        GameObject goo = Instantiate(slimypoopoo, theSewer);
        goo.transform.position = new Vector3(transform.position.x, transform.position.y - 0.35f, 0.5f);
        if (animator.GetBool("isDashing"))
        {
            Color purple;
            ColorUtility.TryParseHtmlString("#000000", out purple);
            goo.GetComponent<SpriteRenderer>().color = purple;
            goo.transform.localScale += Vector3.one * burntScale;
        }
    }

    public void allowDash()
    {
        canDash = true;
    }
    public void disableDash()
    {
        canDash = false;
    }

    public void startedJump()
    {
        InJump = true;
    }
    public void endedJump()
    {
        InJump = false;
    }

    public void dash()
    {
        Vector2 lastMovement = movement;

        canMove = false;
        IsDashing = true;
        StartCoroutine(DashMovement(lastMovement));

        IEnumerator DashMovement(Vector2 movement)
        {
            for (int i = 0; i < dashTime; i++)
            {
                rb.MovePosition(rb.position + movement * dashSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            animator.SetBool("isDashing", false);
            IsDashing = false;
            InAir = false;
        }
    }

    Coroutine _damageEffects;
    public void damage(int damage, float knockBack, Vector2 KnockbackDirection)
    {
        if (!invincable) 
        {
            if (health > 0)
            {
                int FinalDamage = damage - DamageReduction;

                if (FinalDamage >= 0)
                {
                    health -= FinalDamage;
                }

                HealEffect(FinalDamage, true);

                if (_damageEffects == null) StartCoroutine(DamageEffects());
                else
                {
                    StopCoroutine(_damageEffects);
                    StartCoroutine(DamageEffects());
                }
            }
            if (!IsDashing)
            {
                float kbFinal = knockBack - knockBackReduction;
                if (kbFinal > 0)
                {
                    rb.AddForce(KnockbackDirection * knockBack, ForceMode2D.Impulse);
                }
            }
            StartCoroutine(InvincibilityTimer());
        }

        IEnumerator InvincibilityTimer()
        {
            invincable = true;

            for (int i = 0; i < InvincibilityFlashingAmount; i++)
            {
                List<float> prevAs = new List<float>();
                for (int c = 0; c < spritesToFlash.Count; c++)
                {
                    prevAs.Add(spritesToFlash[c].color.a);
                    var col = spritesToFlash[c].color;
                    col.a = 0;
                    spritesToFlash[c].color = col;
                }
                yield return new WaitForSeconds(InvincibilityTime / (InvincibilityFlashingAmount * 4));
                for (int c = 0; c < spritesToFlash.Count; c++)
                {
                    var col = spritesToFlash[c].color;
                    col.a = prevAs[c];
                    spritesToFlash[c].color = col;
                }
                prevAs.Clear();
                yield return new WaitForSeconds(InvincibilityTime / (InvincibilityFlashingAmount * 4));
            }

            invincable = false;
        }

        IEnumerator DamageEffects()
        {
            sr.color = Color.red;

            while (true)
            {
                sr.color = Color.Lerp(sr.color, Color.white, HitEffectTime * Time.deltaTime);

                if (sr.color == Color.white) break;

                yield return null;
            }
        }
    }

    void UpdateHealthBar(int Health)
    {
        var size = HealthBar.sizeDelta;
        size.x = Mathf.Lerp(size.x, (int)(Health / 1.6666666666666666666666666666667f), HealthBarLerpTime * Time.deltaTime);
        HealthBar.sizeDelta = size;
    }

    void SwordManag()
    {
        if (!swinging)
        {
            Vector3 mouse_pos;
            Vector3 object_pos;
            float angle;

            mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f; //The distance between the camera and object
            object_pos = Camera.main.WorldToScreenPoint(swordHolder.transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            swordHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Input.GetMouseButtonDown(0) && !isSpinning && !DisableInput)
            {
                swinging = true;
                swordHolder.SetActive(true);
                sword.SetBool("swing", true);
            }
        }
    }

    public void DamageEnemy(enemyBase Enemy)
    {
        Vector3 dir = Enemy.transform.position - transform.position;

        Enemy.damage(Damage, knockBack, dir, this);
    }
    public void SpinDamageEnemy(enemyBase Enemy)
    {
        Vector3 dir = Enemy.transform.position - transform.position;

        Enemy.damage(SpinDamage, SpinKnockBack, dir, this);
    }

    public void InstaHeal(int Amount)
    {
        health += Amount;

        int Overheal = 0;

        if (health > 100)
        {
            Overheal = health - 100;

            health -= Overheal;
        }

        HealEffect(Amount - Overheal, false);

        //Amout - Overheal = how much really healed
    }

    public void Regenaration(int Amount, float healTime, int healChunks)
    {
        StartCoroutine(regen(Amount, healTime, healChunks));

        IEnumerator regen(int Amount, float healTime, int healChunks)
        {
            int LoopLength = Amount / healChunks;
            int healedAmount = 0;

            for (int i = 0; i < LoopLength; i++)
            {
                int TempHealAmount = 0;

                healedAmount += healChunks;
                health += healChunks;
                TempHealAmount += healChunks;

                if (health > 100)
                {
                    int overHeal = health - 100;
                    health -= overHeal;
                    TempHealAmount -= overHeal;
                }

                HealEffect(TempHealAmount, false);

                yield return new WaitForSeconds(healTime / LoopLength);
            }
            if (Amount < healedAmount)
            {
                int TempHealAmount = 0;

                int extraheal = Amount - healedAmount;
                health += extraheal;
                TempHealAmount += extraheal;

                if (health > 100)
                {
                    int overHeal = health - 100;
                    health -= overHeal;
                    TempHealAmount -= overHeal;
                }

                HealEffect(TempHealAmount, false);
            }
        }
    }

    public void HealEffect(int Amount, bool Damage)
    {
        StartCoroutine(HealEffectE(Amount));
        IEnumerator HealEffectE(int Amount)
        {
            GameObject canvas = Instantiate(GM.FloatingTextObj);
            
            canvas.transform.position = new Vector3(transform.position.x, transform.position.y, 0.3f);

            TextMeshProUGUI text = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (!Damage)
            {
                if (Amount > 0)
                {
                    text.text = "+" + Amount;
                }
                else
                {
                    text.text = "HP FULL";
                    text.fontSize = 24;
                }
            }
            else
            {
                text.color = Color.red;
                text.text = "-" + Amount;
            }

            for (int i = 0; i < 15; i++)
            {
                if (canvas)
                {
                    if (i == 11) text.color = GameManager.SetColorAlpha(text.color, 0);
                    if (i == 12) text.color = GameManager.SetColorAlpha(text.color, 1);
                    if (i == 13) text.color = GameManager.SetColorAlpha(text.color, 0);
                    if (i == 14) text.color = GameManager.SetColorAlpha(text.color, 1);
                    if (i == 15) text.color = GameManager.SetColorAlpha(text.color, 0);

                    canvas.transform.position += new Vector3(0, 0.3f, 0);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            Destroy(canvas);
        }
    }

    public void SpinAttack()
    {
        isSpinning = true;
        animator.SetBool("spinning", true);
        SpinBox.SetActive(true);
    }

    public void EndSpin()
    {
        isSpinning = false;
        animator.SetBool("spinning", false);
        SpinBox.SetActive(false);
    }

    void SceneManagment()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            DisableInput = true;
            collision = false;
            LockInfiniJump = true;
            PlayerCanvas.SetActive(false);
        }
        else
        {
            LockInfiniJump = false;
            PlayerCanvas.SetActive(true);
        }
    }
}
//hello
