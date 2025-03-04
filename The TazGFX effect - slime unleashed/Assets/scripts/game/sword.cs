using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    [BoxGroup("References")]
    [SerializeField] private Player player;

    public void AnimEnded()
    {
        GetComponent<Animator>().SetBool("swing", false);
        player.swordHolder.SetActive(false);
        player.swinging = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemyBase enemyS))
        {
            player.DamageEnemy(enemyS);
        }
    }
}
