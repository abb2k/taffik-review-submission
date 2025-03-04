using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class enemyBase : MonoBehaviour
{
    [BoxGroup("Enemy Default")]
    [SerializeField] protected int enemyHealth;
    [BoxGroup("Enemy Default")]
    [SerializeField] protected bool enemyAggro;
    [BoxGroup("Enemy Default")]
    public SpriteRenderer sr;
    [BoxGroup("Enemy Default")]
    public float enemyVision;
    [BoxGroup("Enemy Default")]
    [SerializeField] protected ItemDrop itemDrop;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hit(collision.gameObject.GetComponent<Player>());
        }
    }


    public virtual void Hit(Player player) { }

    public virtual void damage(int damage, float knockBack, Vector2 KnockbackDirection, Player Damager)
    {
        if (enemyHealth > 0)
        {
            GotDamaged(damage, knockBack, KnockbackDirection, Damager);
        }

        if (enemyHealth <= 0)
        {
            death();
        }
    }

    public virtual void GotDamaged(int damage, float knockBack, Vector2 KnockbackDirection, Player Damager) { }


    public virtual void death() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyVision);
    }
}
