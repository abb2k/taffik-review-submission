using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimatronicCell : MonoBehaviour
{
    public Sprite sprite;
    public Animatronic AnimatronicSettings;
    [Multiline]
    public string Description;

    public Image outlineRenderer;
    public Image myImage;
    public TextMeshProUGUI AIText;

    public Sprite outline;
    public Sprite outlineSelected;

    public AudioClip clicking;
    public AudioClip blip;

    public GameObject MyUI;

    bool onCell;
    bool onUp;
    bool onDown;

    float speedTimer;

    float speedyDelay;

    bool clickingUp;

    bool clickingDown;

    bool oneTimeCreateDesc;

    GameObject descB;

    public GameObject descPosLeft;
    public GameObject descPosRight;

    [HideInInspector]
    public mainMenu MainMenu;

    public bool OppisateDesc;

    private void Start()
    {
        descPosLeft = Instantiate(GameManager.get().emptyObject, transform);
        descPosLeft.transform.localPosition = new Vector3(232.5f, 0, 0);
        descPosLeft.name = "descPosLeft";

        descPosRight = Instantiate(GameManager.get().emptyObject, transform);
        descPosRight.transform.localPosition = new Vector3(-232.5f, 0, 0);
        descPosRight.name = "descPosRight";
    }

    void Update()
    {
        //deactivate and activate UI when hovering
        if (!onCell && !onUp && !onDown)
        {
            outlineRenderer.sprite = outline;
            MyUI.SetActive(false);
            oneTimeCreateDesc = false;
            if (descB != null)
            {
                Destroy(descB);
            }
        }
        else
        {
            outlineRenderer.sprite = outlineSelected;
            if (MainMenu.allowedToShowCellUI)
            {
                MyUI.SetActive(true);
            }
            if (!oneTimeCreateDesc)
            {
                oneTimeCreateDesc = true;
                if (GameManager.get().SaveData.showCharInfo)
                {
                    descB = Instantiate(GameManager.get().descriptionBox, transform.parent.parent);
                    if (!OppisateDesc)
                    {
                        descB.transform.position = new Vector3(descPosLeft.transform.position.x, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        descB.transform.position = new Vector3(descPosRight.transform.position.x, transform.position.y, transform.position.z);
                    }
                    descB.transform.localScale = transform.parent.localScale;
                    descB.GetComponent<descriptionBox>().setDiscText(AnimatronicSettings, Description);
                }
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            speedTimer -= Time.deltaTime;
            if (speedTimer <= 0)
            {
                if (speedyDelay > 0)
                {
                    speedyDelay -= Time.deltaTime;
                    if (speedyDelay <= 0)
                    {
                        if (clickingUp)
                        {
                            if (AnimatronicSettings.AILevel < 20)
                            {
                                AnimatronicSettings.AILevel++;
                                SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
                                AIText.text = AnimatronicSettings.AILevel.ToString();
                            }
                        }
                        if (clickingDown)
                        {
                            if (AnimatronicSettings.AILevel > 0)
                            {
                                AnimatronicSettings.AILevel--;
                                SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
                                AIText.text = AnimatronicSettings.AILevel.ToString();
                            }
                        }
                        speedyDelay = 0.05f;
                    }
                }
                else
                {
                    if (clickingUp)
                    {
                        if (AnimatronicSettings.AILevel < 20)
                        {
                            AnimatronicSettings.AILevel++;
                            SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
                            AIText.text = AnimatronicSettings.AILevel.ToString();
                        }
                    }
                    if (clickingDown)
                    {
                        if (AnimatronicSettings.AILevel > 0)
                        {
                            AnimatronicSettings.AILevel--;
                            SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
                            AIText.text = AnimatronicSettings.AILevel.ToString();
                        }
                    }
                    speedyDelay = 0.025f;
                }
            }
        }
        else
        {
            speedTimer = 0.2f;
        }

        //make stuff low transparency when AI level is at 0
        if (AnimatronicSettings.AILevel == 0)
        {
            myImage.color = new Color(1, 1, 1, 0.2f);
            AIText.color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            myImage.color = new Color32(255, 255, 255, (byte)MainMenu.CellOnOpacity);
            AIText.color = new Color32(255, 255, 255, (byte)MainMenu.CellOnOpacity);
        }
    }

    public void onEnter()
    {
        onCell = true;
    }
    public void onExit()
    {
        onCell = false;
    }

    public void clickedUp()
    {
        if (AnimatronicSettings.AILevel < 20)
        {
            AnimatronicSettings.AILevel++;
            SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
            AIText.text = AnimatronicSettings.AILevel.ToString();
        }
        clickingUp = true;
        if (GameManager.DidDoubleClick())
        {
            AnimatronicSettings.AILevel = 20;
            SoundManager.getSoundManager().CreateSoundEffect("blip", blip);
            AIText.text = AnimatronicSettings.AILevel.ToString();
        }
        
    }
    public void clickedDown()
    {
        if (AnimatronicSettings.AILevel > 0)
        {
            AnimatronicSettings.AILevel--;
            SoundManager.getSoundManager().CreateSoundEffect("clicking", clicking);
            AIText.text = AnimatronicSettings.AILevel.ToString();
        }
        clickingDown = true;
        if (GameManager.DidDoubleClick())
        {
            AnimatronicSettings.AILevel = 0;
            SoundManager.getSoundManager().CreateSoundEffect("blip", blip);
            AIText.text = AnimatronicSettings.AILevel.ToString();
        }
    }

    public void StoppedClickedUp()
    {
        clickingUp = false;
    }
    public void StoppedClickedDown()
    {
        clickingDown = false;
    }
    public void onEnterUp()
    {
        onUp = true;
    }
    public void onExitUp()
    {
        onUp = false;
        clickingUp = false;
    }

    public void onEnterDown()
    {
        onDown = true;
    }
    public void onExitDown()
    {
        onDown = false;
        clickingDown = false;
    }

}
