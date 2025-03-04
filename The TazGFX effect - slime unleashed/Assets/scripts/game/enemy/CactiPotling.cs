using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CactiPotling : emptyPotling
{
    [Space]

    [BoxGroup("Cacti Settings")]
    [SerializeField] private int thornsDamage;

    [BoxGroup("Cacti Settings")]
    [SerializeField] private int thornsChance;

    [BoxGroup("Cacti Settings")]
    [SerializeField] private int thornsKnockBack;

    public override void OnDamage(Player Damager)
    {
        int RandomRes = Random.Range(1, thornsChance + 1);

        if (RandomRes == thornsChance)
        {
            Vector3 dir = Damager.transform.position - transform.position;

            Damager.damage(thornsDamage, thornsKnockBack, dir);
        }
    }
}
