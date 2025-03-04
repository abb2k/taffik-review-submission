using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tech : AnimatronicBase
{
    public Transform sprite;
    public float timeUntilKill;
    public float maskNeededTime;
    public float moveSpeed;

    [SerializeField] float maskForTimer;
    [SerializeField] float killtimer;
    [SerializeField] bool attacking;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(timeUntilKill, "timeUntilKill"));
        AddCustomValue(new FloatValue(maskNeededTime, "maskNeededTime"));
        AddCustomValue(new FloatValue(moveSpeed, "moveSpeed"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        GM = GameManager.get();
        AudioSource source = GM.soundManager.CreateIdleSource("techSound", GM.soundManager.GetSoundFromList("stare"));
        source.loop = true;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (attacking)
        {
            killtimer -= Time.deltaTime;

            if (NM.InMask)
            {
                maskForTimer -= Time.deltaTime;
            }

            if (maskForTimer <= 0)
            {
                maskForTimer = 0;
                attacking = false;
                NM.StartEffectsBlackscreen(false);
                AudioSource a = GM.soundManager.getActiveSource("techSound");
                if (a != null)
                    a.Stop();
            }
            var pos = sprite.localPosition;
            pos.x = Mathf.MoveTowards(pos.x, -3, moveSpeed * Time.deltaTime);
            sprite.localPosition = pos;

            if (killtimer <= 0)
            {
                killtimer = 0;
                attacking = false;
                AudioSource a = GM.soundManager.getActiveSource("techSound");
                if (a != null)
                    a.Stop();
                NM.StartEffectsBlackscreen(false);
                Jumpscare();
            }
        }
        else
        {
            var pos = sprite.localPosition;
            pos.x = Mathf.MoveTowards(pos.x, -17.57f, moveSpeed * Time.deltaTime * 4);
            sprite.localPosition = pos;
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!attacking)
        {
            attacking = true;
            killtimer = timeUntilKill;
            maskForTimer = maskNeededTime;
            NM.StartEffectsBlackscreen(true);
            GM.soundManager.playSoundOnIdleSource("techSound");
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        if (attacking)
        {
            NM.StartEffectsBlackscreen(false);
        }
        attacking = false;
        if (GM.soundManager.getActiveSource("techSound") != null)
        {
            GM.soundManager.getActiveSource("techSound").Stop();
        }

    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        sprite.gameObject.SetActive(false);
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "timeUntilKill")
        {
            timeUntilKill = value.value;
        }
        if (value.keyName == "maskNeededTime")
        {
            maskNeededTime = value.value;
        }
        if (value.keyName == "moveSpeed")
        {
            moveSpeed = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }

    public override GameObject Jumpscare()
    {
        return NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name, null, 9);
    }
}
