using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class plushes : AnimatronicBase
{
    public enum Plushes { cotkeeper, Mindcap, Michi }
    public List<Plushes> allPlushes;
    public cotKeeper COTKeeper;
    public mindCap mindcap;
    public michigun Michigun;

    [SerializeField] bool cotkUsed;
    [SerializeField] bool mindcapUsed;
    [SerializeField] bool MichigunUsed;

    [ReadOnly][SerializeField] float saftyTimer;
    [ReadOnly][SerializeField] bool saftyLock;

    public bool boughtElderE;
    public bool boughtMindcapPlush;
    public bool boughtTriple;

    public GameObject cotSprite;
    public GameObject mindcapSprite;
    public GameObject michiSprite;

    public GameObject ShopcotSprite;
    public GameObject ShopmindcapSprite;
    public GameObject ShopmichiSprite;

    Plushes mostRecent;
    //called when animatronic gets his AILevel
    public override void AnimatronicGameStart()
    {
        if (GM)
            allPlushes = GM.ShuffleList(allPlushes);
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (saftyTimer > 0)
        {
            saftyTimer -= Time.deltaTime;
        }
        else if (GM)
        {
            if (!saftyLock)
            {
                if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02) return;

                saftyLock = true;
                if (cotkUsed && !boughtElderE && !COTKeeper.wasDCoined && mostRecent == Plushes.cotkeeper)
                {
                    COTKeeper.Jumpscare();
                }

                if (mindcapUsed && !boughtMindcapPlush && !mindcap.wasDCoined && mostRecent == Plushes.Mindcap)
                {
                    mindcap.Jumpscare();
                }

                if (MichigunUsed && !boughtTriple && !Michigun.wasDCoined && mostRecent == Plushes.Michi)
                {
                    Michigun.Jumpscare();
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (cotSprite.activeSelf)
        {
            cotSprite.SetActive(false);
            if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
            {
                NM.CamSys.PulseStatic();
            }
        }
        if (mindcapSprite.activeSelf)
        {
            mindcapSprite.SetActive(false);
            if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
            {
                NM.CamSys.PulseStatic();
            }
        }
        if (michiSprite.activeSelf)
        {
            michiSprite.SetActive(false);
            if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
            {
                NM.CamSys.PulseStatic();
            }
        }


        if (allPlushes.Count > 0)
        {
            Plushes oppCalc = allPlushes[0];

            allPlushes.RemoveAt(0);

            if (oppCalc == Plushes.cotkeeper && !cotkUsed && COTKeeper.AILevel != 0)
            {
                cotkUsed = true;
                saftyTimer = 20 / (COTKeeper.AILevel / 15);
                saftyLock = false;
                cotSprite.SetActive(true);
                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
                {
                    NM.CamSys.PulseStatic();
                }
                mostRecent = Plushes.cotkeeper;
                //spawn cot
            }
            if (oppCalc == Plushes.Mindcap && !mindcapUsed && mindcap.AILevel != 0)
            {
                mindcapUsed = true;
                saftyTimer = 20 / (mindcap.AILevel / 15);
                saftyLock = false;
                mindcapSprite.SetActive(true);
                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
                {
                    NM.CamSys.PulseStatic();
                }
                mostRecent = Plushes.Mindcap;
                //spawn midcap
            }
            if (oppCalc == Plushes.Michi && !MichigunUsed && Michigun.AILevel != 0)
            {
                MichigunUsed = true;
                saftyTimer = 20 / (Michigun.AILevel / 15);
                saftyLock = false;
                michiSprite.SetActive(true);
                if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam02)
                {
                    NM.CamSys.PulseStatic();
                }
                mostRecent = Plushes.Michi;
                //spawn michi
            }
            else
            {
                //spawn nobody
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public virtual void BoughtTriple()
    {
        boughtTriple = true;
    }
    public virtual void BoughtMindcap()
    {
        boughtMindcapPlush = true;
    }
    public virtual void BoughtEmerald()
    {
        boughtElderE = true;
    }
}
