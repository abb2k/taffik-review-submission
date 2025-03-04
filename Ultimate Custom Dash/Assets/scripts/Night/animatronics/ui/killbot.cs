using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class killbot : AnimatronicBase
{
    bool doingThing;

    public GameObject notifiCont;

    public TextMeshProUGUI loadingText;

    public float jumpscareTimer = 3.5f;

    public Vector4 minMaxPositionXY;

    float originalJumpTimer;

    public float delayTimer = 2;

    bool playJumpscare;

    bool rermoveNotiffi;

    public AnimationClip textAnim;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        originalJumpTimer = jumpscareTimer;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (doingThing)
            {
                if (jumpscareTimer > 0)
                {
                    jumpscareTimer -= Time.deltaTime;
                    float jumpscareTimerTo100 = 100 - (jumpscareTimer * (100 / originalJumpTimer));

                    loadingText.text = (int)jumpscareTimerTo100 + "%";
                }
                else
                {
                    if (delayTimer > 0)
                    {
                        delayTimer -= Time.deltaTime;
                        if (!rermoveNotiffi)
                        {
                            rermoveNotiffi = true;
                            notifiCont.SetActive(false);

                            NM.Jumpscare(textAnim, null, false, false, false, null, null);
                        }
                    }
                    else
                    {
                        if (!playJumpscare)
                        {
                            playJumpscare = true;

                            NM.addToNoise(1);

                            GameObject jumps = NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, "Killbot", null, 8, "", 1, jumpEnded);
                            jumps.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                        }
                    }
                }

            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!doingThing)
        {
            doingThing = true;
            notifiCont.SetActive(true);
            notifiCont.transform.localPosition = new Vector3(Random.Range(minMaxPositionXY.x, minMaxPositionXY.y), Random.Range(minMaxPositionXY.z, minMaxPositionXY.w), notifiCont.transform.position.z);
            GM.soundManager.CreateSoundEffect("killbotmusic", GM.soundManager.GetSoundFromList("killbotmusic"));
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        doingThing = false;
        notifiCont.SetActive(false);
        jumpscareTimer = 3.5f;
        delayTimer = 2;
        GM.soundManager.DeleteSource("killbotmusic");
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    void jumpEnded()
    {
        rermoveNotiffi = false;
        doingThing = false;
        playJumpscare = false;
        jumpscareTimer = 3.5f;
        delayTimer = 2;
        NM.addToNoise(-1);
        GM.soundManager.DeleteSource("killbotmusic");
    }

    public virtual void OnClickedExitButton()
    {
        if (!rermoveNotiffi)
        {
            doingThing = false;
            notifiCont.SetActive(false);
            jumpscareTimer = 3.5f;
            delayTimer = 2;
            GM.soundManager.DeleteSource("killbotmusic");
            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        originalJumpTimer = jumpscareTimer;
    }

    public override void OnNightComplete()
    {
        doingThing = false;
        notifiCont.SetActive(false);
        GM.soundManager.DeleteSource("killbotmusic");
    }
}
