using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingBox : MonoBehaviour
{
    [BoxGroup("References")]
    [SerializeField] private Player player;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemyBase enemyS))
        {
            player.SpinDamageEnemy(enemyS);
        }
    }
}
