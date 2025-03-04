using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DVSaws : AnimatronicBase
{
    public Transform worldParent;
    public Transform tableParent;
    public saw[] saws;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].originalPos = GameManager.TransformToCompPos(saws[i].TheSaw);
        }

        if (AILevel == 0)
        {
            worldParent.gameObject.SetActive(false);
            tableParent.gameObject.SetActive(false);
        }
        else
        {
            worldParent.gameObject.SetActive(true);
            tableParent.gameObject.SetActive(true);
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        for (int i = 0; i < saws.Length; i++)
        {
            if (saws[i].onTabel)
                if (GameManager.DetectClickedOutside(saws[i].TheSaw.gameObject, false))
                {
                    saws[i].onTabel = false;
                    saws[i].TheSaw.SetParent(tableParent);
                    saws[i].TheSaw = GameManager.setComplexPos(saws[i].TheSaw, saws[i].originalPos);
                    attackState = false;
                }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && !NM.CamsFullyOpened) return;

        int nonSelecCount = 0;
        for (int i = 0; i < saws.Length; i++)
        {
            if (!saws[i].onTabel) nonSelecCount++;
        }

        if (nonSelecCount == 0)
        {
            attackState = true;
            return;
        }
        else
        {
            attackState = false;
        }

        int randomSawSelec = Random.Range(0, nonSelecCount);
        int tabelExtra = 0;

        for (int i = 0; i < saws.Length; i++)
        {
            if (!saws[i].onTabel)
            {
                if (i - tabelExtra == randomSawSelec)
                {
                    saws[i].onTabel = true;
                    saws[i].TheSaw.SetParent(worldParent);
                    saws[i].TheSaw = GameManager.setComplexPos(saws[i].TheSaw, saws[i].poistion);
                }
            }
            else
            {
                tabelExtra++;
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        worldParent.gameObject.SetActive(false);
        tableParent.gameObject.SetActive(false);
        for (int i = 0; i < saws.Length; i++)
        {
            if (GameManager.DetectClickedOutside(saws[i].TheSaw.gameObject, false))
            {
                saws[i].onTabel = false;
                saws[i].TheSaw.SetParent(tableParent);
                saws[i].TheSaw = GameManager.setComplexPos(saws[i].TheSaw, saws[i].originalPos);
            }
        }
        attackState = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].TheSaw.gameObject.SetActive(false);
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        worldParent.gameObject.SetActive(true);
        tableParent.gameObject.SetActive(true);
    }
}

[System.Serializable]
public class saw
{
    [HideInInspector] public bool onTabel;
    public complexPosition poistion;
    public Transform TheSaw;
    public complexPosition originalPos;
}
