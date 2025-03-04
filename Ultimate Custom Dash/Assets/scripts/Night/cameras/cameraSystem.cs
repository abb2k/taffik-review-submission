using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static NightManager;

public class cameraSystem : MonoBehaviour
{
    GameManager GM;
    public NightManager nm;

    [BoxGroup("static")]
    public Image[] CamStatic;
    [BoxGroup("static")]
    public float StaticNormelizeSpeed;
    [BoxGroup("static")]
    public float StaticNormalOpacity;

    [BoxGroup("camPan")]
    public GameObject camPanObj;
    [BoxGroup("camPan")]
    public float panSpeed;
    float waitTimer;
    [BoxGroup("camPan")]
    public float waitTime;
    bool movingLeft;

    public enum Cameras { Cam01, Cam02, Cam03, Cam04, Cam05, Cam06, Cam07, Cam08 };
    [BoxGroup("Camera Displays")]
    public Cameras CurrentCamera;
    [Space]
    [BoxGroup("Camera Displays")]
    [ShowAssetPreview]
    public Sprite CamButtonOn;
    [BoxGroup("Camera Displays")]
    [ShowAssetPreview]
    public Sprite CamButtonOff;
    [Space]
    [BoxGroup("Camera Displays")]
    public GameObject[] Cams;
    [BoxGroup("Camera Displays")]
    public Image[] CamButtons;
    [Space]
    [BoxGroup("Camera Displays")]
    public GameObject musicBoxUI;
    [BoxGroup("Camera Displays")]
    public GameObject closeCurtains;
    [BoxGroup("Camera Displays")]
    public GameObject AOnly;

    [BoxGroup("FazCoins")]
    public GameObject FazCoinPrefab;
    [BoxGroup("FazCoins")]
    public List<GameObject> Coins;
    [BoxGroup("FazCoins")]
    public List<AnimatronicBase> CharactersWhoGiveCoins;

    public event UnityAction<Cameras> onCameraChanged;


    void Start()
    {
        GM = GameManager.get();
    }

    void Update()
    {
        float normelizeColor = 0.0039215686274509803921568627451f;

        for (int i = 0; i < CamStatic.Length; i++)
        {
            if (CamStatic[i].color.a > StaticNormalOpacity * normelizeColor)
            {
                CamStatic[i].color = new Color(1, 1, 1, (((CamStatic[i].color.a * 255) - Time.deltaTime * StaticNormelizeSpeed) * normelizeColor));
                if (CamStatic[i].color.a <= StaticNormalOpacity * normelizeColor)
                {
                    CamStatic[i].color = new Color(1, 1, 1, StaticNormalOpacity * normelizeColor);
                }
            }
        }
        camPanning();
    }

    void camPanning()
    {
        waitTimer -= Time.deltaTime;

        if (movingLeft)
        {
            if (waitTimer <= 0)
            {
                camPanObj.transform.localPosition += new Vector3(Time.deltaTime * panSpeed, 0, 0);

                if (camPanObj.transform.localPosition.x >= 612)
                {
                    camPanObj.transform.localPosition = new Vector3(612, 0, 0);
                    waitTimer = waitTime;
                    movingLeft = false;
                }
            }
        }
        else
        {
            if (waitTimer <= 0)
            {
                camPanObj.transform.localPosition -= new Vector3(Time.deltaTime * panSpeed, 0, 0);

                if (camPanObj.transform.localPosition.x <= -612)
                {
                    camPanObj.transform.localPosition = new Vector3(-612, 0, 0);
                    waitTimer = waitTime;
                    movingLeft = true;
                }
            }
        }
    }

    public void PulseStatic()
    {
        for (int i = 0; i < CamStatic.Length; i++)
        {
            CamStatic[i].color = Color.white;
        }
    }

    public void switchCamera(int cam)
    {
        onCameraChanged(CurrentCamera);

        if (CurrentCamera != (Cameras)cam)
        {
            //switch To Different
            addCoinsToCamera((Cameras)cam);
        }

        CurrentCamera = (Cameras)cam;

        if (CurrentCamera != Cameras.Cam04 && nm.CurrSpecialMode != NightManager.SpecialModes.GMB)
        {
            if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").Pause();
        }
        if (CurrentCamera == Cameras.Cam04 && nm.CamsFullyOpened)
        {
            if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").UnPause();
            musicBoxUI.SetActive(true);
            nm.coinDispCont.SetActive(false);
            AOnly.SetActive(true);
        }
        else
        {
            nm.coinDispCont.SetActive(true);
            musicBoxUI.SetActive(false);
            AOnly.SetActive(false);
        }

        if (CurrentCamera == Cameras.Cam06)
        {
            closeCurtains.SetActive(true);
        }
        else
        {
            closeCurtains.SetActive(false);
        }

        for (int i = 0; i < Cams.Length; i++)
        {
            if (cam == i)
            {
                Cams[i].SetActive(true);
            }
            else
            {
                Cams[i].SetActive(false);
            }
        }
        for (int i = 0; i < CamButtons.Length; i++)
        {
            if (cam == i)
            {
                CamButtons[i].sprite = CamButtonOn;
            }
            else
            {
                CamButtons[i].sprite = CamButtonOff;
            }
        }
        PulseStatic();
        GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
    }

    public void addCoinsToCamera(Cameras camera)
    {
        for (int i = 0; i < Coins.Count; i++)
        {
            Destroy(Coins[i]);
        }
        Coins.Clear();
        for (int i = 0; i < Cams.Length; i++)
        {
            if ((int)camera == i)
            {
                float averageAILevelofCharactersWhoGiveCoins = 0;
                for (int c = 0; c < CharactersWhoGiveCoins.Count; c++)
                {
                    if (CharactersWhoGiveCoins[c].Name == "Requiem Cube" || CharactersWhoGiveCoins[c].Name == "Demon")
                    {
                        averageAILevelofCharactersWhoGiveCoins += CharactersWhoGiveCoins[c].AILevel * 2;
                    }
                    else
                    {
                        averageAILevelofCharactersWhoGiveCoins += CharactersWhoGiveCoins[c].AILevel;
                    }
                }

                averageAILevelofCharactersWhoGiveCoins /= CharactersWhoGiveCoins.Count + 2;

                int maxCoins = 3;

                if (averageAILevelofCharactersWhoGiveCoins < 15)
                {
                    maxCoins = 4;
                }
                else if (averageAILevelofCharactersWhoGiveCoins < 5)
                {
                    maxCoins = 5;
                }

                int randomCoinAmount = Random.Range(0, maxCoins);

                for (int b = 0; b < randomCoinAmount; b++)
                {
                    GameObject coin = Instantiate(FazCoinPrefab, Cams[i].transform);
                    Coins.Add(coin);
                    coin.transform.localPosition = new Vector3(Random.Range(-2377.0f, 2377.0f), Random.Range(-956.0f, 956.0f), 0);
                }
            }
        }
    }

    public void RemoveCoin(GameObject coin)
    {
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Coins[i] == coin)
            {
                SoundManager.getSoundManager().CreateSoundEffect("coinCollect", SoundManager.getSoundManager().GetSoundFromList("coinCollect"));
                Coins.RemoveAt(i);
                i--;
                Destroy(coin);
            }
        }
    }


    public bool IsLookingAtCamera(Cameras camera)
    {
        if (nm.CamsFullyOpened)
        {
            if (CurrentCamera == camera)
            {
                return true;
            }
        }

        return false;
    }
}
