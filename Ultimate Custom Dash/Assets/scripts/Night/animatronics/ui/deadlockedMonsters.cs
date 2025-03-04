using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deadlockedMonsters : AnimatronicBase
{
    public bool appeared;
    public bool failCool;

    public Image Monster1;
    public Image Monster2;
    public Image Monster3;

    public RectTransform plus;

    public int wins;
    public int fails;
    public float FailPanishmentCool;

    public GameObject OverallCont;

    Coroutine bgSounds;

    public peacefulPillers pillers;

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {

    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (appeared)
                {
                    if (RectTransformExtensions.GetWorldRect(plus).Overlaps(RectTransformExtensions.GetWorldRect(Monster1.rectTransform), true) && Monster1.gameObject.activeSelf)
                    {
                        Monster1.gameObject.SetActive(false);
                        GM.soundManager.CreateSoundEffect("fishCought", GM.soundManager.GetSoundFromList("fishCought"));
                        wins++;
                    }
                    else if (RectTransformExtensions.GetWorldRect(plus).Overlaps(RectTransformExtensions.GetWorldRect(Monster2.rectTransform), true) && Monster2.gameObject.activeSelf)
                    {
                        Monster2.gameObject.SetActive(false);
                        GM.soundManager.CreateSoundEffect("fishCought", GM.soundManager.GetSoundFromList("fishCought"));
                        wins++;
                    }
                    else if (RectTransformExtensions.GetWorldRect(plus).Overlaps(RectTransformExtensions.GetWorldRect(Monster3.rectTransform), true) && Monster3.gameObject.activeSelf)
                    {
                        Monster3.gameObject.SetActive(false);
                        GM.soundManager.CreateSoundEffect("fishCought", GM.soundManager.GetSoundFromList("fishCought"));
                        wins++;
                    }
                    else
                    {
                        fails++;
                        GM.soundManager.CreateSoundEffect("fishLost", GM.soundManager.GetSoundFromList("fishLost"));
                    }
                }
            }

            if (appeared && fails >= 3)
            {
                // fail game (fail too much)
                fails = 0;
                wins = 0;
                appeared = false;
                failCool = true;
                NM.EnabledDoorControls = false;
                OverallCont.SetActive(false);
                StartCoroutine(deathSounds(2));
                StopCoroutine(bgSounds);
            }
            if (appeared && wins >= 3)
            {
                //win game (catch all fish)
                appeared = false;
                fails = 0;
                wins = 0;
                OverallCont.SetActive(false);
                StopCoroutine(bgSounds);
            }
            if (failCool)
            {
                if (FailPanishmentCool > 0)
                {
                    FailPanishmentCool -= Time.deltaTime;
                }
                else
                {
                    // re allow doors
                    FailPanishmentCool = 8.75f;
                    failCool = false;
                    NM.EnabledDoorControls = true;
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!appeared)
        {
            if (!failCool && !pillers.displaying)
            {
                appeared = true;
                OverallCont.SetActive(true);
                Monster1.gameObject.SetActive(true);
                Monster2.gameObject.SetActive(true);
                Monster3.gameObject.SetActive(true);
                
                Monster1.transform.localPosition = new Vector3(Random.Range(-185, 185), Monster1.transform.localPosition.y, Monster1.transform.localPosition.z);
                Monster1.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                Monster1.GetComponent<DeadlockedMonstersMovement>().speed = 0.8f + Random.Range(0.0f, 1.0f);
                Monster1.GetComponent<DeadlockedMonstersMovement>().GoingRight = false;

                Monster2.transform.localPosition = new Vector3(Random.Range(-185, 185), Monster2.transform.localPosition.y, Monster2.transform.localPosition.z);
                Monster2.transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
                Monster2.GetComponent<DeadlockedMonstersMovement>().speed = 0.8f + Random.Range(0.0f, 1.0f);
                Monster2.GetComponent<DeadlockedMonstersMovement>().GoingRight = true;

                Monster3.transform.localPosition = new Vector3(Random.Range(-185, 185), Monster3.transform.localPosition.y, Monster3.transform.localPosition.z);
                Monster3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                Monster3.GetComponent<DeadlockedMonstersMovement>().speed = 0.8f + Random.Range(0.0f, 1.0f);
                Monster3.GetComponent<DeadlockedMonstersMovement>().GoingRight = false;

                bgSounds = StartCoroutine(BGSounds());
            }
        }
        else
        {
            // fail game (take too long)
            appeared = false;
            failCool = true;
            OverallCont.SetActive(false);
            fails = 0;
            NM.EnabledDoorControls = false;
            wins = 0;
            StartCoroutine(deathSounds(3));
            StopCoroutine(bgSounds);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        appeared = false;
        failCool = false;
        OverallCont.SetActive(false);
        fails = 0;
        wins = 0;
        Monster1.gameObject.SetActive(false);
        Monster2.gameObject.SetActive(false);
        Monster3.gameObject.SetActive(false);
        if (bgSounds != null) StopCoroutine(bgSounds);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        OverallCont.SetActive(false);
        if (bgSounds != null)
        {
            StopCoroutine(bgSounds);
        }
    }

    IEnumerator deathSounds(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GM.soundManager.CreateSoundEffect("fishLost", GM.soundManager.GetSoundFromList("fishLost"));
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator BGSounds()
    {
        while (true)
        {
            GM.soundManager.CreateSoundEffect("fishing", GM.soundManager.GetSoundFromList("fishing"));
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }

    public override void OnNightComplete()
    {
        OverallCont.SetActive(false);
        if (bgSounds != null)
        {
            StopCoroutine(bgSounds);
        }
    }
}
