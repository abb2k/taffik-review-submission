using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class peacefulPillers : AnimatronicBase
{
    public GameObject frame1;
    public GameObject frame2;
    public GameObject frame3;

    public bool displaying;

    bool enterLock;

    public override void AnimatronicGameStart()
    {
        if (GM != null) GM.soundManager.CreateIdleSource("PeacefulMusic", GM.soundManager.GetSoundFromList("peaceful"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (displaying && Input.GetKeyDown(KeyCode.Return))
            {
                displaying = false;
                frame1.SetActive(false);
                frame2.SetActive(false);
                frame3.SetActive(false);
                GM.soundManager.CreateSoundEffect("peacefulClick", GM.soundManager.GetSoundFromList("flashclick"));
                GM.soundManager.getActiveSource("PeacefulMusic").Stop();
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!displaying)
        {
            displaying = true;

            int randomF = Random.Range(0, 3);
            switch (randomF)
            {
                case 0:
                    frame1.SetActive(true);
                    break;

                case 1:
                    frame2.SetActive(true);
                    break;

                case 2:
                    frame3.SetActive(true);
                    break;
            }

            GM.soundManager.getActiveSource("PeacefulMusic").Play();
        }
        else
        {
            displaying = false;
            frame1.SetActive(false);
            frame2.SetActive(false);
            frame3.SetActive(false);
            GM.soundManager.CreateSoundEffect("peacefulClick", GM.soundManager.GetSoundFromList("flashclick"));
            GM.soundManager.getActiveSource("PeacefulMusic").Stop();
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        displaying = false;
        frame1.SetActive(false);
        frame2.SetActive(false);
        frame3.SetActive(false);
        GM.soundManager.getActiveSource("PeacefulMusic").Stop();
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        frame1.SetActive(false);
        frame2.SetActive(false);
        frame3.SetActive(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }

    public override void OnNightComplete()
    {
        displaying = false;
        frame1.SetActive(false);
        frame2.SetActive(false);
        frame3.SetActive(false);

        AudioSource s = GM.soundManager.getActiveSource("PeacefulMusic");

        if (s)
            s.Stop();
    }
}
