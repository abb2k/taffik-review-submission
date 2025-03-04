using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reqCube : AnimatronicBase
{
    public GameObject cube;
    public float waitTime;
    public float DisableTime;
    public Animator anim;

    bool inRoom;
    bool rollWakeChance;
    bool jumpscared;

    bool oneTimeCamCheck;
    bool camsHaveOpened;

    Coroutine waitToDiable;

    public offcie officeManager;
    public Transform CCont;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(waitTime, "waitTime"));
        AddCustomValue(new FloatValue(DisableTime, "DisableTime"));
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
            if (officeManager.office == offcieLayer.offices.Fnaf3)
            {
                CCont.position = new Vector3(-0.17f, 1.27f, 0);
            }
            else if (officeManager.office == offcieLayer.offices.Fnaf4)
            {
                CCont.position = new Vector3(-3.43f, 1.11f, 0);
            }
            else if (officeManager.office == offcieLayer.offices.SL)
            {
                CCont.position = new Vector3(-0.18f, -0.42f, 0);
            }

            if (NM.CamsFullyOpened)
            {
                camsHaveOpened = true;
            }
            if (!NM.CamsFullyOpened && camsHaveOpened)
            {
                camsHaveOpened = false;
                int randomChance = Random.Range(0, 20 + ExtraChance);
                if (randomChance <= AILevel && AILevel != 0)
                {
                    if (!inRoom && !jumpscared)
                    {
                        anim.SetBool("poped", false);
                        anim.Play("New State", 0);
                        inRoom = true;
                        cube.SetActive(true);
                        if (waitToDiable != null) StopCoroutine(waitToDiable);
                        waitToDiable = StartCoroutine(waitToDiableTimer());
                    }
                }
            }
            if (inRoom && !jumpscared)
            {
                if (GameManager.DetectClickedOutside(cube, false))
                {
                    if (waitToDiable != null) StopCoroutine(waitToDiable);
                    inRoom = false;
                    anim.SetBool("poped", false);
                    anim.Play("New State", 0);
                    anim.SetBool("poped", true);

                    NM.addFazCoins(1);
                    GM.soundManager.CreateSoundEffect("reqcubeKilled", GM.soundManager.GetSoundFromList("popSharp"));
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        inRoom = false;
        cube.SetActive(false);
        if (waitToDiable != null) StopCoroutine(waitToDiable);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        if (waitToDiable != null) StopCoroutine(waitToDiable);
    }

    void disableCams()
    {
        Jumpscare();
        jumpscared = true;
        cube.SetActive(false);
        inRoom = false;
        StartCoroutine(disableCamsAfter());
        IEnumerator disableCamsAfter()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return null;
            }
            NM.CamsAllowed = false;
        }
        StartCoroutine(reEnableTiemr());
        IEnumerator reEnableTiemr()
        {
            yield return new WaitForSeconds(DisableTime);
            reEnableCams();
        }
    }

    void reEnableCams()
    {
        NM.CamsAllowed = true;
        jumpscared = false;
    }

    IEnumerator waitToDiableTimer()
    {
        yield return new WaitForSeconds(waitTime);
        disableCams();
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "waitTime")
        {
            waitTime = value.value;
        }
        if (value.keyName == "DisableTime")
        {
            DisableTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
