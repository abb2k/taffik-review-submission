using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using NaughtyAttributes;
using static UnityEngine.GraphicsBuffer;

public class emptyPotling : enemyBase
{
    [Space]
    [BoxGroup("Potling Settings")]
    [SerializeField] private int emptyPotlingDamage;

    [BoxGroup("Potling Settings")]
    [SerializeField] private float emptyPotlingknockBack;

    [BoxGroup("Potling Settings")]
    [SerializeField] private int DamageReduction;

    [BoxGroup("Potling Settings")]
    [SerializeField] private int knockBackReduction;

    [BoxGroup("Potling Settings")]
    [SerializeField] private float InvincibilityTime;

    [HideInInspector]
    [SerializeField] private bool invincable;

    [BoxGroup("Potling Settings")]
    [SerializeField] private int InvincibilityFlashingAmount;

    [BoxGroup("Potling Settings")]
    [SerializeField] private List<SpriteRenderer> spritesToFlash;
    bool dead;

    [Space]

    [BoxGroup("Pathfinding Settings")]
    [SerializeField] private float demantiaTimer;
    private float demantiaTimeCapsule;

    [BoxGroup("Pathfinding Settings")]
    [SerializeField] private AIPath pathFinder;

    [Space]

    [BoxGroup("Effects")]
    [SerializeField] private float HitEffectTime;
    [BoxGroup("Effects")]
    [SerializeField] private Rigidbody2D rb;

    [Space]

    [BoxGroup("Animation")]
    [SerializeField] private Animator anim;

    void Start()
    {
        GetComponent<AIDestinationSetter>().target = GameManager.getShared().player.transform;
    }

    void Update()
    {
        RaycastHit2D[] koletBashetah = Physics2D.CircleCastAll(transform.position, enemyVision, Vector2.up);
        for (int i = 0; i < koletBashetah.Length; i++)
        {
            if (koletBashetah[i].transform.tag == "Player" && !dead)
            {
                enemyAggro = true;
                anim.SetBool("isMoving", true);
                demantiaTimeCapsule = demantiaTimer;
            }
        }

        if (demantiaTimeCapsule > 0)
        {
            demantiaTimeCapsule -= Time.deltaTime;
        }
        else
        {
            enemyAggro = false;
            anim.SetBool("isMoving", false);
            pathFinder.canMove = false;
        }
        
    }
    public override void Hit(Player player)
    {
        Vector3 dir = player.transform.position - transform.position;

        player.damage(emptyPotlingDamage, emptyPotlingknockBack, dir);
    }

    public void allowMovement()
    {
        pathFinder.canMove = true;


    }
    public void disableMovement()
    {
        pathFinder.canMove = false;

    }

    [HideInInspector]
    private Coroutine _damageEffects;
    public override void GotDamaged(int damage, float knockBack, Vector2 KnockbackDirection, Player Damager)
    {
        if (!invincable)
        {
            invincable = true;
            int FinalDamage = damage - DamageReduction;

            if (FinalDamage >= 0)
            {
                enemyHealth -= damage;
            }
            if (_damageEffects == null) StartCoroutine(DamageEffects());
            else
            {
                StopCoroutine(_damageEffects);
                StartCoroutine(DamageEffects());
            }
            float kbFinal = knockBack - knockBackReduction;
            if (kbFinal > 0)
            {
                rb.AddForce(KnockbackDirection * knockBack, ForceMode2D.Impulse);
            }
            OnDamage(Damager);
            StartCoroutine(InvincibilityTimer());
        }
    }

    public IEnumerator InvincibilityTimer()
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

    public IEnumerator DamageEffects()
    {
        sr.color = Color.red;

        while (true)
        {
            sr.color = Color.Lerp(sr.color, Color.white, HitEffectTime * Time.deltaTime);

            if (sr.color == Color.white) break;

            yield return null;
        }
    }

    public override void death()
    {
        anim.SetBool("dead", true);
        dead = true;
        enemyAggro = false;
    }

    public void OnDeathAnimEnded()
    {
        Destroy(gameObject);
    }

    public virtual void OnDamage(Player Damager) { }

    public void dropItems()
    {
        itemDrop.RollItemDrop();
    }
}
