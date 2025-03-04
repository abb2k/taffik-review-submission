using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ductPath : MonoBehaviour
{
    public ductPoint leftDuctStart;
    public ductPoint rightDuctStart;
    public ductPoint CurrentPos;

    private void Awake()
    {
        resetPositioin();
    }

    public void Move(bool listenToLure)
    {
        bool didListenToLure = false;
        if (listenToLure)
        {
            if (CurrentPos.isAudioLureClose(out ductPoint point))
            {
                SetPosTo(point);
                didListenToLure = true;
            }
        }
        
        if (!didListenToLure)
        {
            int randomDir = Random.Range(0, 6);

            if (randomDir == 0)
            {
                int randomChoice = Random.Range(0, CurrentPos.back.Length);
                if (CurrentPos.back.Length != 0)
                {
                    SetPosTo(CurrentPos.back[randomChoice]);
                }
            }
            else
            {
                int randomChoice = Random.Range(0, CurrentPos.forward.Length);
                if (CurrentPos.forward.Length != 0)
                {
                    SetPosTo(CurrentPos.forward[randomChoice]);
                }
            }
        }
    }

    public void UpdatePos()
    {
        SetPosTo(CurrentPos);
    }

    public void SetPosTo(ductPoint pos)
    {
        CurrentPos = pos;
        transform.position = pos.transform.position;
    }

    public void resetPositioin()
    {
        int randomSide = Random.Range(0, 2);

        if (randomSide == 0)
        {
            SetPosTo(leftDuctStart);
        }
        else
        {
            SetPosTo(rightDuctStart);
        }
    }

    public bool IsAtEnd()
    {
        if (CurrentPos.leftEnd || CurrentPos.rightEnd)
        {
            return true;
        }
        return false;
    }

    public bool IsDuctNotClosedOnMe()
    {
        if (CurrentPos.leftEnd && !NightManager.inctance.LeftDuctClosed)
        {
            return true;
        }
        if (CurrentPos.rightEnd && NightManager.inctance.LeftDuctClosed)
        {
            return true;
        }
        return false;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        GetComponent<Image>().SetNativeSize();
    }

    public void pushBack(bool makeSound = true)
    {
        resetPositioin();
        if (makeSound)
            SoundManager.getSoundManager().CreateSoundEffect("pushback", SoundManager.getSoundManager().GetSoundFromList("pushback"));
    }
}
