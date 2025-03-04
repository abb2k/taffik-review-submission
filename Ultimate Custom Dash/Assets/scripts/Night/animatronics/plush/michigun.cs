using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class michigun : AnimatronicBase
{
    public plushes pmanager;
    public bool wasDCoined;

    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

        wasDCoined = true;
        pmanager.michiSprite.SetActive(false);
        pmanager.ShopmichiSprite.SetActive(false);
    }
    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
