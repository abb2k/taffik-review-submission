using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class theEschaton : AnimatronicBase
{
    public SpriteRenderer eschatonRend;
    [ShowAssetPreview]
    public Sprite turnedOff;
    [ShowAssetPreview]
    public Sprite turnedOn;

    public CircleCollider2D asleepHitbox;
    public CircleCollider2D awakeHitbox;

    bool inOffice;

    bool avoiding;

    bool isTurnedOn;

    bool killplayer;

    float noKillCooldown;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (GameManager.DetectClickedOutside(asleepHitbox, false) || GameManager.DetectClickedOutside(awakeHitbox, false))
            {
                if (inOffice && !NM.CamsFullyOpened)
                {
                    if (!isTurnedOn)
                    {
                        //if clicked while sleeping
                        if (!killplayer && noKillCooldown <= 0)
                        {
                            killplayer = true;
                            Jumpscare();
                        }
                    }
                    else
                    {
                        //if clicked while awake
                        NM.StartEffectsBlackscreen(true);
                        avoiding = true;
                        GM.soundManager.CreateSoundEffect("shock", GM.soundManager.GetSoundFromList("shock"));
                        StartCoroutine(waitForAvoiding());
                        IEnumerator waitForAvoiding()
                        {
                            yield return new WaitForSeconds(0.5f);
                            avoded();
                        }
                    }
                }
            }

            if (noKillCooldown > 0)
            {
                noKillCooldown -= Time.deltaTime;
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && !NM.CamsFullyOpened) return;

        if (!inOffice)
        {
            eschatonRend.gameObject.SetActive(true);
            eschatonRend.sprite = turnedOff;
            asleepHitbox.enabled = true;
            awakeHitbox.enabled = false;
            noKillCooldown = 1;
            if (!NM.CamsFullyOpened)
            {
                NM.pulseBlackscreen();
            }
            inOffice = true;
        }
        else
        {
            if (!isTurnedOn)
            {
                eschatonRend.sprite = turnedOn;
                asleepHitbox.enabled = false;
                awakeHitbox.enabled = true;
                if (!NM.CamsFullyOpened)
                {
                    NM.pulseBlackscreen();
                }
                isTurnedOn = true;
            }
            else if (isTurnedOn && !avoiding)
            {
                if (!killplayer)
                {
                    killplayer = true;
                    Jumpscare();
                }
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        eschatonRend.gameObject.SetActive(false);
        asleepHitbox.enabled = false;
        awakeHitbox.enabled = false;
        noKillCooldown = 1;
        avoiding = false;
        isTurnedOn = false;
        inOffice = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        eschatonRend.gameObject.SetActive(false);
    }
    void avoded()
    {
        avoiding = false;
        eschatonRend.gameObject.SetActive(false);
        inOffice = false;
        isTurnedOn = false;
        NM.pulseBlackscreen();
        NM.StartEffectsBlackscreen(false);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);


    }
}
