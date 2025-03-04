using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Samifying : AnimatronicBase
{
    public Animator anim;

    public Transform cont;

    bool isIn;

    public float kbspeed;
    public float rotspeed;

    bool jumpLock;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (GameManager.DetectClickedOutside(anim.gameObject, false))
        {
            if (isIn)
            {
                GM.soundManager.CreateSoundEffect("samiClicked", GM.soundManager.GetSoundFromList("mouseSnap"));
            }
            isIn = false;
            anim.speed = 0f;
            anim.enabled = false;
            jumpLock = false;
        }

        if (!isIn)
        {
            var pos = cont.localPosition;
            pos.x = Mathf.MoveTowards(pos.x, 30.09f, Time.deltaTime * kbspeed);
            pos.y = Mathf.MoveTowards(pos.y, 15.28f, Time.deltaTime * kbspeed);
            cont.localPosition = pos;

            var angles = cont.eulerAngles;
            angles.z += Time.deltaTime * rotspeed;
            cont.eulerAngles = angles;
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && !NM.CamsFullyOpened) return;

        GM.soundManager.CreateSoundEffect("samiLaugh", GM.soundManager.GetSoundFromList("samiLaugh"));
        anim.enabled = true;
        anim.speed = 1;
        anim.Play("New State");
        anim.SetBool("started", true);
        isIn = true;
        cont.localPosition = new Vector3(0,0, cont.localPosition.z);
        cont.eulerAngles = Vector3.zero;
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        isIn = false;
        anim.speed = 0f;
        anim.enabled = false;
        jumpLock = false;
        cont.gameObject.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        cont.gameObject.SetActive(false);
    }

    public void animEnded()
    {
        if (jumpLock)
        {
            Jumpscare();
        }
    }

    public void animStarted()
    {
        jumpLock = true;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        cont.gameObject.SetActive(true);
    }
}
