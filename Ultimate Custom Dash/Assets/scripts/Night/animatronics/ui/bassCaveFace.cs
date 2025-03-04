using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bassCaveFace : AnimatronicBase
{
    public GameObject bass;

    public float stayTime;

    public float TimeBetweenBeeps;

    float stayTimer;

    bool inHere;

    int progress;

    Coroutine soundthingy;

    public KeyCode lettr1;
    public KeyCode lettr2;
    public KeyCode lettr3;
    public KeyCode lettr4;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(stayTime, "stayTime"));
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
            if (inHere)
            {
                switch (progress)
                {
                    case 0:
                        if (Input.GetKeyDown(lettr1))
                        {
                            progress++;
                            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
                        }
                        break;
                    case 1:
                        if (Input.GetKeyDown(lettr2))
                        {
                            progress++;
                            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
                        }
                        break;
                    case 2:
                        if (Input.GetKeyDown(lettr3))
                        {
                            progress++;
                            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
                        }
                        break;
                    case 3:
                        if (Input.GetKeyDown(lettr4))
                        {
                            inHere = false;
                            GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
                            bass.SetActive(false);
                            StopCoroutine(soundthingy);
                            soundthingy = null;
                            NM.addToNoise(-6);
                        }
                        break;
                }
                stayTimer -= Time.deltaTime;


                if (stayTimer <= 0)
                {
                    inHere = false;
                    bass.SetActive(false);
                    GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
                    StopCoroutine(soundthingy);
                    soundthingy = null;
                    NM.addToNoise(-6);
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!inHere)
        {
            if (soundthingy != null) StopCoroutine(soundthingy);
            soundthingy = StartCoroutine(soundthing());
            stayTimer = stayTime;
            progress = 0;
            inHere = true;
            bass.SetActive(true);
            NM.addToNoise(6);
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        if (inHere)
        {
            inHere = false;
            StopCoroutine(soundthingy);
            soundthingy = null;
            progress = 0;
            bass.SetActive(false);
            NM.addToNoise(-6);
        }
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        if (soundthingy != null)
            StopCoroutine(soundthingy);
    }

    IEnumerator soundthing()
    {
        while (true)
        {
            GM.soundManager.CreateSoundEffect("lolbeep", GM.soundManager.GetSoundFromList("lolbeep"));
            yield return new WaitForSeconds(TimeBetweenBeeps);
        }
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "stayTime")
        {
            stayTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }

    public override void OnNightComplete()
    {
        if (soundthingy != null)
            StopCoroutine(soundthingy);
    }
}
