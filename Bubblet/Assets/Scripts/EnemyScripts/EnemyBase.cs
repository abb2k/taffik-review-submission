using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, ICatchable
{
    [Header("Stats")]
    [SerializeField] private Ranged aliveTime;
    [SerializeField] private Ranged shootEvery;
    [SerializeField] protected DamageInfo damageInfo;

    [Header("Modifiers")]
    [SerializeField] private float healOnConsume;
    [SerializeField] protected float multiAddedOnConsume;
    [SerializeField] protected float scoreWorth;

    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioClip shootSound;

    public event UnityAction<EnemyBase> onDeath;

    protected bool wasCaught;
    private bool isAlive = true;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public Transform getCapturedObject()
    {
        return transform;
    }

    public float getHealOnCosume()
    {
        return healOnConsume;
    }
    public void OnCatch()
    {
        wasCaught = true;
        animator.enabled = false;
        
        GetComponent<Collider2D>().enabled = false;
        GameManager.get().getLevelManager().addBaseScore(scoreWorth);
        onDeath.Invoke(this);
    }

    public void startShootAndLifeTimer()
    {
        StartCoroutine(lifetime());
        StartCoroutine(shootTimer());
        if (Random.Range(0, 2) == 1) OnShoot();
    }

    IEnumerator lifetime()
    {
        aliveTime.generateRandom();
        yield return new WaitForSeconds(aliveTime.knowRandom);
        if (!wasCaught && !GameManager.get().Bubble.IsDead)
            OnEscape();
        StopAllCoroutines();
    }

    IEnumerator shootTimer()
    {
        while (true)
        {
            shootEvery.generateRandom();
            yield return new WaitForSeconds(shootEvery.knowRandom);

            if (!isAlive || wasCaught) break;

            OnShoot();
        }
    }

    protected virtual void OnShoot()
    {

    }

    protected virtual void OnEscape()
    {
        isAlive = false;
        onDeath.Invoke(this);
        FeedbackText text = GameManager.get().CreateFeedbackText(transform.position);
        text.StartTextEffect("escaped");
        GameManager.get().getLevelManager().addBaseScore(scoreWorth);
        Destroy(gameObject);
    }

    public IEnumerator transitionIntoArena(Vector3 to, float time, UnityAction<EnemyBase> callback)
    {
        Vector3 orignalPos = transform.position;

        for (float t = 0; t < time; t += Time.deltaTime)
        {
            if (transform == null) break;

            transform.position = Vector3.Lerp(orignalPos, to, t / time);

            yield return null;
        }

        callback.Invoke(this);
    }

    public float getMultiOnConsume()
    {
        return multiAddedOnConsume;
    }

    public void sendBulletWithDamageInfo(Bullet bullet, Vector2 dir)
    {
        bullet.setInfo(damageInfo);
        bullet.send(dir);
    }

    public void playShootSound()
    {
        SoundManager.getSoundManager().CreateSoundEffect("shoot", shootSound);
    }
}
