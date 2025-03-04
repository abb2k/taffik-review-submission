using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class basementMonster : AnimatronicBase
{
    [BoxGroup("Settings")]
    public GameObject BasementMonsterINWORLD;
    [BoxGroup("Settings")]
    public SpriteRenderer eyeLeft;
    [BoxGroup("Settings")]
    public SpriteRenderer eyeRight;

    public enum eyeColors { blue, orange, green };
    [Space]
    [BoxGroup("Settings")]
    public List<eyeColors> colors;

    [Space]
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite blueKey;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite orangeKey;
    [BoxGroup("Settings")]
    [ShowAssetPreview]
    public Sprite greenKey;
    [BoxGroup("Settings")]
    public keyPosition[] keyPositions;
    [BoxGroup("Settings")]
    public List<GameObject> keys;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {

    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (colors.Count == 0)
        {
            if (!GM.silent && !NM.CamsFullyOpened) return;

            NM.pulseBlackscreen();
            colors = randomizeColors();
            eyeLeft.color = eyeColorToColor(colors[0]);
            eyeRight.color = eyeColorToColor(colors[1]);
            BasementMonsterINWORLD.SetActive(true);
            for (int i = 0; i <= 2; i++)
            {
                CreateKey((eyeColors)i);
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
        BasementMonsterINWORLD.SetActive(false);
        colors.Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            Destroy(keys[i]);
        }
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        BasementMonsterINWORLD.SetActive(false);
    }

    List<eyeColors> randomizeColors()
    {
        List<eyeColors> colorsSelected = new List<eyeColors>();

        while (colorsSelected.Count < 2)
        {
            int randomColor = Random.Range(0, 3);
            eyeColors colorChosen = (eyeColors)randomColor;

            bool alreadyChosen = false;

            for (int i = 0; i < colorsSelected.Count; i++)
            {
                if (colorsSelected[i] == colorChosen)
                {
                    alreadyChosen = true;
                }
            }

            if (!alreadyChosen)
            {
                colorsSelected.Add(colorChosen);
            }
        }
        
        return colorsSelected;
    }

    Color eyeColorToColor(eyeColors eyeCol)
    {
        switch (eyeCol)
        {
            case eyeColors.blue:
                return new Color32(0, 201, 255, 255);
            case eyeColors.orange:
                return new Color32(255, 165, 0, 255);
            case eyeColors.green:
                return Color.green;
        }
        return Color.black;
    }

    public virtual void KeyCollected(int eyecolor, GameObject key)
    {
        eyeColors colorGotten = (eyeColors)eyecolor;

        bool isColorRight = false;

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == key)
            {
                keys.RemoveAt(i);
            }
        }

        Destroy(key);
        NM.CamSys.PulseStatic();

        for (int i = 0; i < colors.Count; i++)
        {
            if (colors[i] == colorGotten)
            {
                isColorRight = true;
                colors.RemoveAt(i);
                for (int b = 0; b < keyPositions.Length; b++)
                {
                    if (keyPositions[b].posCaught && keyPositions[b].heldColor == colorGotten)
                    {
                        keyPositions[b].posCaught = false;
                    }
                }
                break;
            }
        }

        if (isColorRight)
        {

        }
        else
        {
            Jumpscare();
        }

        if (colors.Count == 0)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                Destroy(keys[i]);
                keys.RemoveAt(i);
                i--;
            }
            for (int b = 0; b < keyPositions.Length; b++)
            {
                keyPositions[b].posCaught = false;
            }
            BasementMonsterINWORLD.SetActive(false);
        }
    }

    public void CreateKey(eyeColors color, keyPosition position = null)
    {
        keyPosition keypos = position;
        if (position == null)
        {
            keypos = new keyPosition(null, Vector3.zero, Vector3.zero, true);
            while (keypos.posCaught == true)
            {
                int randomPos = Random.Range(0, keyPositions.Length);

                keypos = keyPositions[randomPos];

                bool isAllcought = true;
                for (int i = 0; i < keyPositions.Length; i++)
                {
                    if (!keyPositions[i].posCaught)
                    {
                        isAllcought = false;
                    }
                }

                if (isAllcought) break;
            }

            keypos.heldColor = color;
            keypos.posCaught = true;
        }

        GameObject key = Instantiate(GM.emptyObject, keypos.parent);
        Image myImage = key.AddComponent<Image>();
        keys.Add(key);

        EventTrigger trigger = key.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        

        switch (color)
        {
            case eyeColors.blue:
                myImage.sprite = blueKey;
                entry.callback.AddListener((eventData) => { KeyCollected(0, key); });
                break;
            case eyeColors.orange:
                myImage.sprite = orangeKey;
                entry.callback.AddListener((eventData) => { KeyCollected(1, key); });
                break;
            case eyeColors.green:
                myImage.sprite = greenKey;
                entry.callback.AddListener((eventData) => { KeyCollected(2, key); });
                break;
        }

        
        trigger.triggers.Add(entry);

        myImage.SetNativeSize();

        key.transform.localPosition = keypos.position;
        key.transform.localEulerAngles = keypos.rotation;
        key.transform.localScale = Vector3.one * 2;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}