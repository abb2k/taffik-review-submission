using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopkeeper : AnimatronicBase
{
    [BoxGroup("Settings")]
    public SpriteRenderer Character;
    [BoxGroup("Settings")]
    public GameObject payButton;
    [BoxGroup("Settings")]
    [Space]
    [ShowAssetPreview]
    public Sprite awake;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite asleep;
    [BoxGroup("Settings")]
    [Space]
    public bool sleeping;
    [BoxGroup("Settings")]
    public float HeatTime;
    [BoxGroup("Settings")]
    [ReadOnly][SerializeField] float HeatTimer;
    [BoxGroup("Settings")]
    [ReadOnly][SerializeField] float GlitchTimer;
    [BoxGroup("Settings")]
    public GameObject GDDialogPre;
    [BoxGroup("Settings")]
    [ReadOnly] public GameObject GDDialog;
    [BoxGroup("Settings")]
    public Transform topUI;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite dialogIconHappy;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite dialogIconSad;
    [BoxGroup("Settings")]
    [ReadOnly][SerializeField] float angerTimer;
    bool lockOnce;
    bool lockHeaterDialog;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(HeatTime, "HeatTime"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        HeatTimer = HeatTime;
        if (AILevel != 0)
        {
            Character.gameObject.SetActive(true);
        }
        else
        {
            Character.gameObject.SetActive(false);
        }
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (!sleeping)
        {
            if (NM.CurrSpecialMode == NightManager.SpecialModes.Heater)
            {
                if (!lockHeaterDialog)
                {
                    lockHeaterDialog = true;
                    GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Y0u ArE tRYinG t0 T3IcK mE");
                }
                
                if (GlitchTimer > 0)
                {
                    GlitchTimer -= Time.deltaTime;
                }
                else
                {
                    GlitchTimer = Random.Range(0.05f, 0.5f);
                    if (Character.sprite == awake)
                    {
                        Character.sprite = asleep;
                    }
                    else
                    {
                        Character.sprite = awake;
                    }
                }

                if (HeatTimer > 0)
                {
                    HeatTimer -= Time.deltaTime;
                }
                else
                {
                    sleeping = true;
                    Character.sprite = asleep;
                    payButton.SetActive(false);
                    if (GDDialog != null)
                    {
                        GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Thank you, for depositing 8 coins.");
                        GDDialog.GetComponent<GDDialogBox>().destroyMeTimer(3);
                    }
                }
            }
            else
            {
                Character.sprite = awake;

                if (lockHeaterDialog)
                {
                    lockHeaterDialog = false;
                    if (angerTimer > (OppretunityEvery / 4) * 2)
                    {
                        GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Please deposit 8 coins.");
                    }
                    else
                    {
                        GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconSad, "Shopkeeper", "Please deposit 8 coins!");
                    }
                }
                
            }
        }

        if (GameManager.DetectClickedOutside(payButton, false) && !sleeping)
        {
            if (NM.FazCoins >= 8)
            {
                NM.addFazCoins(-8);

                NM.ItemBought("SK8Coins");
                sleeping = true;
                Character.sprite = asleep;
                payButton.SetActive(false);
                SoundManager.getSoundManager().CreateSoundEffect("buyItem", SoundManager.getSoundManager().GetSoundFromList("buyItem"));
                GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Thank you, for depositing 8 coins.");
                GDDialog.GetComponent<GDDialogBox>().destroyMeTimer(3);
            }
        }

        if (!sleeping)
        {
            angerTimer -= Time.deltaTime;
            if (NM.CurrSpecialMode != NightManager.SpecialModes.Heater)
            {
                if (angerTimer <= (OppretunityEvery / 4) * 3 && angerTimer > (OppretunityEvery / 4) * 2)
                {
                    if (!lockOnce)
                    {
                        lockOnce = true;
                        if (GDDialog != null) GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Please deposit 8 coins.");
                    }
                }
                else if (angerTimer <= (OppretunityEvery / 4) * 2 && angerTimer > OppretunityEvery / 4)
                {
                    if (lockOnce)
                    {
                        lockOnce = false;
                        if (GDDialog != null) GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconSad, "Shopkeeper", "Please deposit 8 coins!");
                    }
                }
                else if (angerTimer <= OppretunityEvery / 4)
                {
                    if (!lockOnce)
                    {
                        lockOnce = true;
                        if (GDDialog != null) GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconSad, "Shopkeeper", "Please deposit 8 coins!");
                    }
                }
            }
        }
        else
        {
            angerTimer = OppretunityEvery;
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (sleeping)
        {
            if (!GM.silent && !NM.CamsFullyOpened) return;

            sleeping = false;
            Character.sprite = awake;
            payButton.SetActive(true);
            HeatTimer = HeatTime;
            lockOnce = false;
            if (GDDialog == null)
            {
                GDDialog = Instantiate(GDDialogPre, topUI);
                GDDialog.transform.SetAsFirstSibling();
                GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Please deposit 8 coins.");
            }
            else
            {
                Destroy(GDDialog);
                GDDialog = Instantiate(GDDialogPre, topUI);
                GDDialog.transform.SetAsFirstSibling();
                GDDialog.GetComponent<GDDialogBox>().CreateDialog(dialogIconHappy, "Shopkeeper", "Please deposit 8 coins.");
            }
        }
        else
        {
            Jumpscare();
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        if (GDDialog != null)
        {
            Destroy(GDDialog);
        }
        sleeping = true;
        Character.sprite = asleep;
        Character.gameObject.SetActive(false);
        payButton.SetActive(false);
        lockHeaterDialog = false;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        Character.gameObject.SetActive(false);
        payButton.SetActive(false);
        if (GDDialog != null)
        {
            Destroy(GDDialog);
        }
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "HeatTime")
        {
            HeatTime = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        HeatTimer = HeatTime;
        Character.gameObject.SetActive(true);
    }
}
