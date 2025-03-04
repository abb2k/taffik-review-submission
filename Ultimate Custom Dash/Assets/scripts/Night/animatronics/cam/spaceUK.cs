using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spaceUK : AnimatronicBase
{
    public GameObject pcScreenCont;
    public Transform spaceUKIcon;
    public Transform ZbotIcon;
    public Image cameraViewScreen;
    public Image camView;

    [ShowAssetPreview]
    public Sprite gameOverVer;
    [ShowAssetPreview]
    public Sprite normalVer;
    [Space]
    [ShowAssetPreview]
    public Sprite[] SmallCam;
    [Space]
    [ShowAssetPreview]
    public Sprite OpenedMiniDoor;
    [ShowAssetPreview]
    public Sprite ClosedMiniDoor;
    [Space]
    [ShowAssetPreview]
    public Sprite CamButtonSelected;
    [ShowAssetPreview]
    public Sprite CamButtonDeSelected;

    public Image[] buttons;
    public Image[] doorButtons;

    public complexPosition[] positions;

    public Image CamStatic;

    bool gameover;
    float gameOverKillTimer;

    public enum SmallCams {SmallCam1, SmallCam2, SmallCam3};
    public SmallCams currCamera;

    public SmallCams currZBotPos;

    public enum blockPoses {None, blockCam1, blockCam2, blockCam3};
    public blockPoses currBlockPos;

    public override void AnimatronicStart()
    {
        if (!GM.silent)
        {
            OppretunityEvery = 27;
            OppretunityTimer = OppretunityEvery;
        }
        gameOverKillTimer = Random.Range(3.0f, 12.0f);
        currZBotPos = RandomPosition();
        switch (currZBotPos)
        {
            case SmallCams.SmallCam1:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[0]);
                break;

            case SmallCams.SmallCam2:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[1]);
                break;

            case SmallCams.SmallCam3:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[2]);
                break;
        }
        
        if (AILevel == 0)
        {
            pcScreenCont.SetActive(false);
            spaceUKIcon.gameObject.SetActive(false);
            camView.sprite = normalVer;
        }
        else
        {
            pcScreenCont.SetActive(true);
            spaceUKIcon.gameObject.SetActive(true);
            camView.sprite = gameOverVer;
        }
        int randCam = Random.Range(0,3);
        updateMiniCams((SmallCams)randCam);
    }

    float normelizeColor = 0.0039215686274509803921568627451f;
    public override void AnimatronicUpdate()
    {
        if (CamStatic.color.a > 24 * normelizeColor)
        {
            CamStatic.color = new Color(1, 1, 1, (((CamStatic.color.a * 255) - Time.deltaTime * 300) * normelizeColor));
            if (CamStatic.color.a <= 24 * normelizeColor)
            {
                CamStatic.color = new Color(1, 1, 1, 24 * normelizeColor);
            }
        }
        if (NM.NightOngoing)
        {
            if (gameover)
            {
                gameOverKillTimer -= Time.deltaTime;
                if (gameOverKillTimer <= 0)
                {
                    Jumpscare();
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!GM.silent && NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam08) return;

        switch (currZBotPos)
        {
            case SmallCams.SmallCam1:
                if (currBlockPos == blockPoses.blockCam1)
                {
                    changeZBotPosition();
                }
                else
                {
                    gameover = true;
                    pcScreenCont.SetActive(false);
                    spaceUKIcon.gameObject.SetActive(false);
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam08)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                break;

            case SmallCams.SmallCam2:
                if (currBlockPos == blockPoses.blockCam2)
                {
                    changeZBotPosition();
                }
                else
                {
                    gameover = true;
                    pcScreenCont.SetActive(false);
                    spaceUKIcon.gameObject.SetActive(false);
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam08)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                break;

            case SmallCams.SmallCam3:
                if (currBlockPos == blockPoses.blockCam3)
                {
                    changeZBotPosition();
                }
                else
                {
                    gameover = true;
                    pcScreenCont.SetActive(false);
                    spaceUKIcon.gameObject.SetActive(false);
                    if (NM.CamSys.CurrentCamera == cameraSystem.Cameras.Cam08)
                    {
                        NM.CamSys.PulseStatic();
                    }
                }
                break;
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        pcScreenCont.SetActive(false);
        spaceUKIcon.gameObject.SetActive(false);
        camView.sprite = normalVer;
        gameOverKillTimer = Random.Range(3.0f, 12.0f);
        currZBotPos = RandomPosition();
        switch (currZBotPos)
        {
            case SmallCams.SmallCam1:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[0]);
                break;

            case SmallCams.SmallCam2:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[1]);
                break;

            case SmallCams.SmallCam3:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[2]);
                break;
        }
        int randCam = Random.Range(0, 3);
        updateMiniCams((SmallCams)randCam);
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        pcScreenCont.SetActive(false);
        spaceUKIcon.gameObject.SetActive(false);
    }

    void updateMiniCams(SmallCams cam)
    {
        currCamera = cam;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == (int)cam)
            {
                buttons[i].sprite = CamButtonSelected;
            }
            else
            {
                buttons[i].sprite = CamButtonDeSelected;
            }
        }

        if (currZBotPos == currCamera)
        {
            ZbotIcon.gameObject.SetActive(true);
        }
        else
        {
            ZbotIcon.gameObject.SetActive(false);
        }

        cameraViewScreen.sprite = SmallCam[(int)cam];
        pulseMiniStatic();
    }

    void changeZBotPosition()
    {
        
        currZBotPos = RandomPosition();
        switch (currZBotPos)
        {
            case SmallCams.SmallCam1:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[0]);
                break;

            case SmallCams.SmallCam2:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[1]);
                break;

            case SmallCams.SmallCam3:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[2]);
                break;
        }
        if (currZBotPos == currCamera)
        {
            ZbotIcon.gameObject.SetActive(true);
        }
        else
        {
            ZbotIcon.gameObject.SetActive(false);
        }
        pulseMiniStatic();
    }

    SmallCams RandomPosition()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                return SmallCams.SmallCam1;

            case 1:
                return SmallCams.SmallCam2;

            case 2:
                return SmallCams.SmallCam3;
        }
        return SmallCams.SmallCam1;
    }

    void CloseSmallDoor(blockPoses pos)
    {
        currBlockPos = pos;

        for (int i = 0; i < doorButtons.Length; i++)
        {
            if (i == (int)pos - 1)
            {
                doorButtons[i].sprite = ClosedMiniDoor;
            }
            else
            {
                doorButtons[i].sprite = OpenedMiniDoor;
            }
        }
    }

    public virtual void SwitchMiniCams(int camrera)
    {
        updateMiniCams((SmallCams)camrera);
        GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
    }

    public virtual void MiniDoorToggle(int door)
    {
        CloseSmallDoor((blockPoses)door);
        GM.soundManager.CreateSoundEffect("miniDoor", GM.soundManager.GetSoundFromList("door"));
    }

    public void pulseMiniStatic()
    {
        CamStatic.color = Color.white;
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        gameOverKillTimer = Random.Range(3.0f, 12.0f);
        currZBotPos = RandomPosition();
        switch (currZBotPos)
        {
            case SmallCams.SmallCam1:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[0]);
                break;

            case SmallCams.SmallCam2:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[1]);
                break;

            case SmallCams.SmallCam3:
                ZbotIcon = GameManager.setComplexPos(ZbotIcon, positions[2]);
                break;
        }
        pcScreenCont.SetActive(true);
        spaceUKIcon.gameObject.SetActive(true);
        camView.sprite = gameOverVer;
        int randCam = Random.Range(0, 3);
        updateMiniCams((SmallCams)randCam);
    }
}
