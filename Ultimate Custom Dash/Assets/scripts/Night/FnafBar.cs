using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FnafBar : MonoBehaviour
{
    [Range(0, 6)]
    public int progress;
    [Range(3, 5)]
    public int RednessStart;
    [ShowAssetPreview]
    public Sprite BarFill1;
    [ShowAssetPreview]
    public Sprite BarFill2;
    [ShowAssetPreview]
    public Sprite BarFill3White;
    [ShowAssetPreview]
    public Sprite BarFill3Red;
    [ShowAssetPreview]
    public Sprite BarFill4White;
    [ShowAssetPreview]
    public Sprite BarFill4Red;
    [ShowAssetPreview]
    public Sprite BarFill5Red;
    [ShowAssetPreview]
    public Sprite BarFill6Red;
    public Image Fill;
    bool rednessFlashing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeValue(int value)
    {
        if (value > 6)
        {
            value = 6;
        }
        if (value < 0)
        {
            value = 0;
        }
        progress = value;
        switch (progress)
        {
            case 0:
                Fill.enabled = false;
                rednessFlashing = false;
                break;
            case 1:
                Fill.enabled = true;
                Fill.sprite = BarFill1;
                rednessFlashing = false;
                break;
            case 2:
                Fill.enabled = true;
                Fill.sprite = BarFill2;
                rednessFlashing = false;
                break;
            case 3:
                if (progress >= RednessStart)
                {
                    rednessFlashing = true;
                    Fill.sprite = BarFill3Red;
                }
                else
                {
                    Fill.enabled = true;
                    Fill.sprite = BarFill3White;
                    rednessFlashing = false;
                }
                break;
            case 4:
                if (progress >= RednessStart)
                {
                    rednessFlashing = true;
                    Fill.sprite = BarFill4Red;
                }
                else
                {
                    Fill.enabled = true;
                    Fill.sprite = BarFill4White;
                    rednessFlashing = false;
                }
                break;
            case 5:
                rednessFlashing = true;
                Fill.sprite = BarFill5Red;
                break;
            case 6:
                rednessFlashing = true;
                Fill.sprite = BarFill6Red;
                break;
        }
    }

    public void RedFlashing()
    {
        if (rednessFlashing)
        {
            Fill.enabled = !Fill.enabled;
        }
    }
}
