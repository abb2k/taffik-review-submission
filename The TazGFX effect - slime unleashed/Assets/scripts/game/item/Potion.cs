using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    [Space]

    [BoxGroup("Potion Settings")]
    [SerializeField] private int Heal;

    [BoxGroup("Potion Settings")]
    [SerializeField] private int HealTime;

    [BoxGroup("Potion Settings")]
    [SerializeField] private int HealChunks;

    public override void OnCollect(Player player)
    {
        player.Regenaration(Heal,HealTime,HealChunks);

        Destroy(gameObject);
    }
}
