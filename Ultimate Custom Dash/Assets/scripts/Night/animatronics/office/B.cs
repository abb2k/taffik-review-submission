using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B : AnimatronicBase
{
    public GameObject B1;
    public GameObject B2;
    public GameObject B3;
    public GameObject B4;
    public GameObject B5;

    public Transform B1real;
    public Transform B2real;
    public Transform B3real;
    public Transform B4real;
    public Transform B5real;

    public AudioClip Buzzing;
    AudioSource buzzingSource;

    public float progresslevel = 0;

    public float progressingSpeed = 1.5f;
    public float removeMultiplier = 40;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(progressingSpeed, "progressingSpeed"));
        AddCustomValue(new FloatValue(removeMultiplier, "removeMultiplier"));
        AddCustomValue(new FloatValue(progresslevel, "progresslevel"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        buzzingSource = GM.soundManager.CreateLoopingSound("Buzzing", Buzzing);
        buzzingSource.volume = 0;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (NM.FlashlightOn)
            {
                float previPLevel = progresslevel;
                progresslevel -= Time.deltaTime * removeMultiplier;

                if (progresslevel < 0) progresslevel = 0;

                if (previPLevel > 100 && progresslevel <= 100)
                {
                    B1.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = B1real.transform.position;
                }
                if (previPLevel > 200 && progresslevel <= 200)
                {
                    B2.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = B2real.transform.position;
                }
                if (previPLevel > 300 && progresslevel <= 300)
                {
                    B3.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = B3real.transform.position;
                }
                if (previPLevel > 400 && progresslevel <= 400)
                {
                    B4.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = B4real.transform.position;
                }
                if (previPLevel > 500 && progresslevel <= 500)
                {
                    B5.SetActive(false);
                    GameObject poof = PlayPoof();
                    poof.transform.position = B5real.transform.position;
                }
            }

            if (progresslevel > 100)
            {
                B1.SetActive(true);
            }
            if (progresslevel > 200)
            {
                B2.SetActive(true);
            }
            if (progresslevel > 300)
            {
                B3.SetActive(true);
            }
            if (progresslevel > 400)
            {
                B4.SetActive(true);
            }
            if (progresslevel > 500)
            {
                B5.SetActive(true);
            }
            if (progresslevel > 600)
            {
                Jumpscare();
            }

            if (buzzingSource)
                buzzingSource.volume = progresslevel / 600;

            for (int i = 0; i < customVals.Floats.Count; i++)
            {
                if (customVals.Floats[i].keyName == "progresslevel")
                {
                    customVals.Floats[i].value = progresslevel;
                    foreach (Transform child in NM.debugging.FieldsCont)
                    {
                        if (child.TryGetComponent(out floatField ff))
                        {
                            if (ff.valueName == customVals.Floats[i].keyName)
                            {
                                ff.SetValue(customVals.Floats[i]);
                            }
                        }
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (NM.CamsFullyOpened)
        {
            progresslevel += progressingSpeed;
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        progresslevel = 0;
        B1.SetActive(false);
        B2.SetActive(false);
        B3.SetActive(false);
        B4.SetActive(false);
        B5.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "progressingSpeed")
        {
            progressingSpeed = value.value;
        }
        if (value.keyName == "removeMultiplier")
        {
            removeMultiplier = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
