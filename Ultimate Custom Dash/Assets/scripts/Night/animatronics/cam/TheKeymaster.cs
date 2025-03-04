using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheKeymaster : AnimatronicBase
{
    [BoxGroup("Settings")]
    public RectTransform blocker;
    [BoxGroup("Settings")]
    public RectTransform[] buttonsPossible;
    [BoxGroup("Settings")]
    [ReadOnly] public int clickTimes;
    [BoxGroup("Settings")]
    [ReadOnly] public int clickNeeded;
    [BoxGroup("Settings")]
    public Animator iconAnim;
    Image lockImage;
    Image characterImage;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (AILevel < 5)
        {
            clickNeeded = 1;
        }
        else if (AILevel >= 5 && AILevel < 10)
        {
            clickNeeded = 2;
        }
        else if (AILevel >= 10 && AILevel < 15)
        {
            clickNeeded = 3;
        }
        else if (AILevel >= 15 && AILevel < 20)
        {
            clickNeeded = 4;
        }
        else if (AILevel == 20)
        {
            clickNeeded = 5;
        }

        lockImage = iconAnim.GetComponent<Image>();
        characterImage = iconAnim.transform.GetChild(iconAnim.transform.childCount - 1).GetComponent<Image>();
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (!lockImage) return;
        float calcOpacity = (OppretunityTimer / OppretunityEvery) + 0.65f;

        var lC = lockImage.color;
        lC.a = calcOpacity;
        lockImage.color = lC;

        var cC = characterImage.color;
        cC.a = calcOpacity;
        characterImage.color = cC;
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        int randombutton = Random.Range(0, buttonsPossible.Length);

        for (int i = 0; i < buttonsPossible.Length; i++)
        {
            if (i == randombutton)
            {
                blocker.SetParent(buttonsPossible[i]);
                blocker.sizeDelta = buttonsPossible[i].sizeDelta;
                blocker.localScale = Vector3.one;
                blocker.localPosition = Vector3.zero;
                blocker.SetParent(buttonsPossible[i].parent);
                blocker.gameObject.SetActive(true);
                clickTimes = 0;
            }
        }
        StartCoroutine(disableHitboxForABit());
    }

    IEnumerator disableHitboxForABit()
    {
        blocker.GetComponent<Image>().raycastTarget = false;

        yield return new WaitForSeconds(1.5f);

        blocker.GetComponent<Image>().raycastTarget = true;
    }

    public virtual void onClicked()
    {
        GM.soundManager.CreateSoundEffect("clicked", GM.soundManager.GetSoundFromList("gdChestClicked"));
        iconAnim.SetBool("test", true);
        iconAnim.Play("New State 0", 0);
        if (clickTimes >= clickNeeded)
        {
            GM.soundManager.CreateSoundEffect("vosKilled", GM.soundManager.GetSoundFromList("popSharp"));
            blocker.gameObject.SetActive(false);
        }
        clickTimes++;
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        clickTimes = 0;
        blocker.gameObject.SetActive(false);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        if (AILevel < 5)
        {
            clickNeeded = 1;
        }
        else if (AILevel >= 5 && AILevel < 10)
        {
            clickNeeded = 2;
        }
        else if (AILevel >= 10 && AILevel < 15)
        {
            clickNeeded = 3;
        }
        else if (AILevel >= 15 && AILevel < 20)
        {
            clickNeeded = 4;
        }
        else if (AILevel == 20)
        {
            clickNeeded = 5;
        }
    }
}
