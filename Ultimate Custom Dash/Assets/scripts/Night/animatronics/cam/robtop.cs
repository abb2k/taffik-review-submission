using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class robtop : AnimatronicBase
{
    public float jumpscareTimer;

    public float wind;

    public bool holding;
    public bool Escaped;

    public float boxChargeMinitimer;

    public RectTransform boxWindBar;

    [ShowAssetPreview]
    public Sprite mboff;
    [ShowAssetPreview]
    public Sprite mbon;

    public Image mbButton;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(wind, "wind"));
        AddCustomValue(new BoolValue(Escaped, "Escaped"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        jumpscareTimer = Random.Range(3.0f, 8.0f);
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            boxWindBar.localScale = new Vector3(wind / 100, boxWindBar.localScale.y, boxWindBar.localScale.z);

            if (holding)
            {
                if (boxChargeMinitimer > 0)
                {
                    boxChargeMinitimer -= Time.deltaTime;
                }
                else
                {
                    boxChargeMinitimer = 0.3f;
                    GM.soundManager.CreateSoundEffect("windup", GM.soundManager.GetSoundFromList("windup"));
                    if (wind < 100)
                    {
                        wind += 16.5f;

                        if (wind >= 100)
                        {
                            wind = 100;
                        }
                    }
                }
            }
            if (Escaped)
            {
                if (jumpscareTimer > 0)
                {
                    jumpscareTimer -= Time.deltaTime;
                }
                else
                {
                    Escaped = false;
                    jumpscareTimer = 1000;
                    Jumpscare();
                }
            }

            for (int i = 0; i < customVals.Floats.Count; i++)
            {
                if (customVals.Floats[i].keyName == "wind")
                {
                    customVals.Floats[i].value = wind;
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

            if (!NM.CamsFullyOpened)
            {
                holding = false;
                mbButton.sprite = mboff;
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (NM.CurrSpecialMode == NightManager.SpecialModes.GMB)
        {
            if (wind < 100)
            {
                if (GM.silent)
                    wind += 6;
                else
                    wind += 9;

                if (wind >= 100)
                {
                    wind = 100;
                }
            }
        }
        else if (!holding)
        {
            if (wind > 0)
            {
                wind -= 0.125f * AILevel;

                if (wind <= 0 && !Escaped)
                {
                    wind = 0;
                    Escaped = true;
                    NM.fastVentelation = true;
                    if (GM.soundManager.getActiveSource("Robescape") == null)
                    {
                        GM.soundManager.CreateLoopingSound("Robescape", GM.soundManager.GetSoundFromList("escape"));
                        GM.soundManager.DeleteSource("MusicBox");
                    }

                    foreach (Transform child in NM.debugging.FieldsCont)
                    {
                        if (child.TryGetComponent(out boolField ff))
                        {
                            if (ff.valueName == "Escaped")
                            {
                                ff.SetValue(new BoolValue(Escaped, ff.valueName));
                            }
                        }
                    }
                }
            }
            else if (!Escaped)
            {
                if (GM.soundManager.getActiveSource("Robescape") == null)
                {
                    GM.soundManager.CreateLoopingSound("Robescape", GM.soundManager.GetSoundFromList("escape"));
                    GM.soundManager.DeleteSource("MusicBox");
                }
                NM.fastVentelation = true;
                wind = 0;
                Escaped = true;

                foreach (Transform child in NM.debugging.FieldsCont)
                {
                    if (child.TryGetComponent(out boolField ff))
                    {
                        if (ff.valueName == "Escaped")
                        {
                            ff.SetValue(new BoolValue(Escaped, ff.valueName));
                        }
                    }
                }
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        wind = 100;
        boxWindBar.localScale = new Vector3(wind / 100, boxWindBar.localScale.y, boxWindBar.localScale.z);
        if (Escaped)
        {
            NM.fastVentelation = false;
            Escaped = false;
        }
        if (GM.soundManager.getActiveSource("Robescape") != null)
        {
            GM.soundManager.DeleteSource("Robescape");
        }
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public virtual void startedHold()
    {
        holding = true;
        mbButton.sprite = mbon;
    }

    public virtual void stoppedHold()
    {
        holding = false;
        mbButton.sprite = mboff;
    }

    public override void SetCustomValue(BoolValue value)
    {
        if (value.keyName == "Escaped")
        {
            Escaped = value.value;
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        jumpscareTimer = Random.Range(3.0f, 8.0f);
    }
}
