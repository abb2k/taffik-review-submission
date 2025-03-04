using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ventPath : MonoBehaviour
{
    public float progress;
    public enum Path { left, top, right };
    public Path selectedPath;

    private void Awake()
    {
        transform.localPosition = new Vector3(-1090, 288, 0);
        RanddomizeDirection();
    }

    public void UpdatePath()
    {
        if (selectedPath == Path.left)
        {
            if (progress <= 725.5f)
            {
                transform.localPosition = new Vector3(-1090 + progress, 288, 0);
            }
            else if (progress <= 1373.5f)
            {
                float turn1 = progress - 725.5f;
                transform.localPosition = new Vector3(-364.5f, 288 - turn1, 0);
            }
            else if (progress <= 1748f)
            {
                float turn2 = progress - 1373.5f;
                transform.localPosition = new Vector3(-364.5f + turn2, -360, 0);
            }
            else if (progress <= 1990)
            {
                float turn3 = progress - 1748f;
                transform.localPosition = new Vector3(10, -360 - turn3, 0);
            }
            else
            {
                transform.localPosition = new Vector3(10, -602, 0);
            }
        }
        else if (selectedPath == Path.top)
        {
            
            if (progress <= 549)
            {
                transform.localPosition = new Vector3(-1090 + progress, 288, 0);
            }
            else if (progress <= 906.5f)
            {
                float turn1 = progress - 549;

                transform.localPosition = new Vector3(-541, 288 + turn1, 0);
            }
            else if (progress <= 1860)
            {
                float turn2 = progress - 906.5f;

                transform.localPosition = new Vector3(-541 + turn2, 645.5f, 0);
            }
            else if (progress <= 2266)
            {
                float turn3 = progress - 1860;

                transform.localPosition = new Vector3(412.5f, 645.5f - turn3, 0);
            }
            else if (progress <= 2668.5f)
            {
                float turn4 = progress - 2266;
                transform.localPosition = new Vector3(412.5f - turn4, 239.5f, 0);
            }
            else if (progress <= 3510)
            {
                float turn5 = progress - 2668.5f;
                transform.localPosition = new Vector3(10, 239.5f - turn5, 0);
            }
            else
            {
                transform.localPosition = new Vector3(10, -602, 0);
            }
        }
        else if (selectedPath == Path.right)
        {

            if (progress <= 549)
            {
                transform.localPosition = new Vector3(-1090 + progress, 288, 0);
            }
            else if (progress <= 906.5f)
            {
                float turn1 = progress - 549;
                transform.localPosition = new Vector3(-541, 288 + turn1, 0);
            }
            else if (progress <= 2232)
            {
                float turn2 = progress - 906.5f;
                transform.localPosition = new Vector3(-541 + turn2, 645.5f, 0);
            }
            else if (progress <= 3241)
            {
                float turn3 = progress - 2232;
                transform.localPosition = new Vector3(784.5f, 645.5f - turn3, 0);
            }
            else if (progress <= 4015.5)
            {
                float turn4 = progress - 3241;
                transform.localPosition = new Vector3(784.5f - turn4, -363.5f, 0);
            }
            else if (progress <= 4254)
            {
                float turn5 = progress - 4015.5f;
                transform.localPosition = new Vector3(10, -363.5f - turn5, 0);
            }
            else
            {
                transform.localPosition = new Vector3(10, -602, 0);
            }
        }
    }

    public void resetPosition()
    {
        progress = 0;
        RanddomizeDirection();
        UpdatePath();
    }

    public void RanddomizeDirection()
    {
        int randomDir = Random.Range(0, 3);
        selectedPath = (Path)randomDir;
    }

    public void AddProgress(float amount)
    {
        progress += amount;
        UpdatePath();
    }
    
    public bool isTouchingAVentSnare(bool checkIfClosed)
    {
        switch (selectedPath)
        {
            case Path.left:
                if (progress >= 1450 && progress <= 1575)
                {
                    if (checkIfClosed)
                    {
                        if (NightManager.inctance.currentSnare == NightManager.sentSnareStates.left)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
                else
                {
                    return false;
                }
            case Path.top:
                if (progress >= 2970 && progress <= 3095)
                {
                    if (checkIfClosed)
                    {
                        if (NightManager.inctance.currentSnare == NightManager.sentSnareStates.top)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
                else
                {
                    return false;
                }
            case Path.right:
                if (progress >= 3730 && progress <= 3855)
                {
                    if (checkIfClosed)
                    {
                        if (NightManager.inctance.currentSnare == NightManager.sentSnareStates.right)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    public bool isAtEnd()
    {
        switch (selectedPath)
        {
            case Path.left:
                if (progress >= 1990)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Path.top:
                if (progress >= 3510)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Path.right:
                if (progress >= 4254)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        GetComponent<Image>().SetNativeSize();
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
