using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class ventSnares : MonoBehaviour
{
    public NightManager nm;
    public Image[] snares;
    [ShowAssetPreview]
    public Sprite snareEmpty;
    [ShowAssetPreview]
    public Sprite snareFilledSide;
    [ShowAssetPreview]
    public Sprite snareFilledTop;

    public void SetSnare(int snare)
    {
        SoundManager.getSoundManager().CreateSoundEffect("blip", SoundManager.getSoundManager().GetSoundFromList("blip"));
        if ((NightManager.sentSnareStates)(snare + 1) != nm.currentSnare)
        {
            for (int i = 0; i < snares.Length; i++)
            {
                if (i == snare)
                {
                    if (i == 1)
                    {
                        //top one
                        snares[i].sprite = snareFilledTop;
                        snares[i].SetNativeSize();
                        snares[i].color = Color.white;
                    }
                    else
                    {
                        //other
                        snares[i].sprite = snareFilledSide;
                        snares[i].SetNativeSize();
                        snares[i].color = Color.white;
                    }
                }
                else
                {
                    snares[i].sprite = snareEmpty;
                    snares[i].SetNativeSize();
                    snares[i].color = new Color32(255, 255, 255, 175);
                }
            }
            nm.currentSnare = (NightManager.sentSnareStates)(snare + 1);
        }
    }
}
