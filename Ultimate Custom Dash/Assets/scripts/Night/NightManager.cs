using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class NightManager : MonoBehaviour
{
    GameManager GM;

    public static NightManager inctance;

    public debugStuff debugging;

    [BoxGroup("World")]
    public desk Desk;
    [BoxGroup("World")]
    public cameraMovement movement;
    [BoxGroup("World")]
    public Camera PostUICamera;
    [BoxGroup("World")]
    public Camera NoPostUICamera;
    [BoxGroup("World")]
    public Volume GlobalPostVol;
    [BoxGroup("World")]
    public VolumeProfile WorldPostVol;
    [BoxGroup("World")]
    public VolumeProfile CamsPostVol;

    public enum doors { left, top, right, bottomRight};
    public event UnityAction<doors, bool> OnDoorStateChanged;
    [Header("Doors")]
    [BoxGroup("World")]
    public bool leftDoorClosed;
    [BoxGroup("World")]
    public Animator leftDoorAnim;
    [Space]
    [BoxGroup("World")]
    public bool topDoorClosed;
    [BoxGroup("World")]
    public Animator topDoorAnim;
    [Space]
    [BoxGroup("World")]
    public bool rightDoorClosed;
    [BoxGroup("World")]
    public Animator rightDoorAnim;
    [Space]
    [BoxGroup("World")]
    public bool bottomRightDoorClosed;
    [BoxGroup("World")]
    public Animator bottomRightDoorAnim;
    [Space]
    [BoxGroup("World")]
    public bool EnabledDoorControls;
    [Space]
    [Header("FREDDY NOSE")]
    [BoxGroup("World")]
    public GameObject freddyNose;
    [Space]
    [Header("Floor Sign")]
    [BoxGroup("World")]
    public GameObject signLeft;
    [BoxGroup("World")]
    public GameObject signRight;
    [BoxGroup("World")]
    public bool signOnRight;
    [BoxGroup("World")]
    [Header("Other Office Stuff")]
    public offcie Office;
    [BoxGroup("World")]
    public GameObject Flashlight;
    [BoxGroup("World")]
    public bool FlashlightOn;
    [BoxGroup("World")]
    public bool FlashlightDisabled;

    [BoxGroup("Cams")]
    public bool InCams;
    [BoxGroup("Cams")]
    [SerializeField][ReadOnly] bool onCamsButton;
    [BoxGroup("Cams")]
    [SerializeField][ReadOnly]bool lockCamToggle;
    [BoxGroup("Cams")]
    [SerializeField][ReadOnly] bool zeroFrameCamsFix;
    [BoxGroup("Cams")]
    public bool CamsAllowed;
    [BoxGroup("Cams")]
    public Image CamsButton;
    [BoxGroup("Cams")]
    public Animator MonitorAnim;
    [BoxGroup("Cams")]
    [Space]
    public GameObject CamsContainer;
    [BoxGroup("Cams")]
    public bool CamsFullyOpened;
    [BoxGroup("Cams")]
    public cameraSystem CamSys;
    public enum camSystems { cameraSys, ventSys, ductSys };
    [Space]
    [BoxGroup("Cams")]
    public camSystems currSys;

    public event UnityAction<camSystems> OnCamSysSwitch;

    [BoxGroup("Cams")]
    public GameObject[] systemConts;
    public enum sentSnareStates { none, left, top, right };
    [Space]
    [Header("Vents")]
    [BoxGroup("Cams")]
    public Transform VentIconCont;
    [BoxGroup("Cams")]
    public sentSnareStates currentSnare;
    [Space]
    [Header("Ducts")]
    [BoxGroup("Cams")]
    public Transform DuctIconCont;
    [BoxGroup("Cams")]
    public bool LeftDuctClosed;
    [BoxGroup("Cams")]
    public ductPoint leftTopPoint;
    [BoxGroup("Cams")]
    public ductPoint rightTopPoint;

    [BoxGroup("UI")]
    public RectTransform AllUICont;
    [BoxGroup("UI")]
    public TextMeshProUGUI timer;
    float TimeInSeconds = 0;
    int TimeInTenthSeconds = 0;

    [BoxGroup("UI")]
    public TextMeshProUGUI amTimer;
    [BoxGroup("UI")]
    public float changeAMEvery;
    [Space]
    [BoxGroup("UI")]
    public FnafBar UsageBar;
    [BoxGroup("UI")]
    public TextMeshProUGUI PowerText;
    [BoxGroup("UI")]
    public FnafBar NoiseBar;
    [Space]
    [BoxGroup("UI")]
    public TextMeshProUGUI fazCoinstext;
    [BoxGroup("UI")]
    public GameObject dCoin;
    [BoxGroup("UI")]
    public GameObject dCoinFollowing;
    [BoxGroup("UI")]
    public bool pickedDCoin;
    [BoxGroup("UI")]
    [ShowAssetPreview]
    public Sprite dcoinoff;
    [BoxGroup("UI")]
    [ShowAssetPreview]
    public Sprite dcoinon;
    [BoxGroup("UI")]
    public TextMeshProUGUI dcoinText;
    [BoxGroup("UI")]
    public GameObject coinDispCont;
    [Space]
    [BoxGroup("UI")]
    public TextMeshProUGUI tempText;
    [BoxGroup("UI")]
    public TextMeshProUGUI tempTextTempIndic;
    [BoxGroup("UI")]
    public RectTransform tempTextScalePoint;
    [BoxGroup("UI")]
    public Image BigBlackscreen;
    [BoxGroup("UI")]
    public Animator[] ResetVentStuff;
    [Space]
    [BoxGroup("UI")]
    public Image EffectsBlackscreen;
    [BoxGroup("UI")]
    public bool EffectsBlackscreenActive;
    [BoxGroup("UI")]
    public int isEffectsActive;
    [BoxGroup("UI")]
    [ReadOnly][SerializeField] float EffectsBlackscreenTimer;
    [BoxGroup("UI")]
    public Image PulseBlackscreen;

    [BoxGroup("Mask")]
    public bool InMask;
    [BoxGroup("Mask")]
    public Image MaskButton;
    [BoxGroup("Mask")]
    public Animator MaskAnim;
    [BoxGroup("Mask")]
    public Animator ColMaskAnim;

    [BoxGroup("Stats")]
    public int FazCoins;
    [BoxGroup("Stats")]
    public int UsageLevel;
    [BoxGroup("Stats")]
    public int NoiseLevel;
    [BoxGroup("Stats")]
    public float Power;
    bool IsPowerOut;
    bool LockpowerOutage;
    [BoxGroup("Stats")]
    public float Temperature;
    [BoxGroup("Stats")]
    public int CoolAmount;
    [BoxGroup("Stats")]
    public int HeatAmount;
    bool frigidLock;
    public enum SpecialModes { PowerGen, SilentVent, Heater, PowerAC, GMB, Off};
    [Space]
    [BoxGroup("Stats")]
    public SpecialModes CurrSpecialMode;
    [BoxGroup("Stats")]
    public RectTransform[] SpecialModeButtons;
    [BoxGroup("Stats")]
    public RectTransform SelectDotSpecials;
    [Space]
    [BoxGroup("Stats")]
    public bool boughtDeathCoin;
    [Space]
    [BoxGroup("Stats")]
    public bool isDead;
    [Space]
    [BoxGroup("Stats")]
    public bool is5020Mode;

    [BoxGroup("Overall")]
    public float EscapeTime;
    float EscapingTimer;
    [BoxGroup("Overall")]
    [ReadOnly]public bool escaping;

    [BoxGroup("Overall")]
    public bool NightOngoing;
    [BoxGroup("Overall")]
    public bool nightComplete;
    [BoxGroup("Overall")]
    public bool playerDead;

    [BoxGroup("Overall")]
    public int NightPointsValue;

    [BoxGroup("Overall")]
    public bool fastVentelation;
    [BoxGroup("Overall")]
    public int vetolationMutliplier;

    [BoxGroup("Overall")]
    public float flashingTime;
    float redFlashingTimer;

    [BoxGroup("Overall")]
    public shopSlot triple;
    [BoxGroup("Overall")]
    public shopSlot emerald;
    [BoxGroup("Overall")]
    public shopSlot mindcap;

    [BoxGroup("Overall")]
    public GraphicRaycaster raycaster;

    [BoxGroup("Overall")]
    public EventSystem eventSystem;

    [BoxGroup("Overall")]
    public PointerEventData pointerEventData;

    [BoxGroup("Overall")]
    public float vetelationTimer;
    [BoxGroup("Overall")]
    public bool canResetVen;

    [BoxGroup("Overall")]
    public GameObject jumpscarePrefab;
    [BoxGroup("Overall")]
    public Transform jumpscaresCont;
    [BoxGroup("Overall")]
    public List<GameObject> Jumpscares;
    [BoxGroup("Overall")]
    public bool beingJumpscared;
    [BoxGroup("Overall")]
    [ReadOnly][SerializeField] float JumpscareRotation;
    [BoxGroup("Overall")]
    [ReadOnly][SerializeField] bool JumpscareRotationLeft;
    [BoxGroup("Overall")]
    public float JumpscareRotationSpeed;
    [BoxGroup("Overall")]
    public float JumpscareMaxRotation;
    [Space]
    [BoxGroup("Overall")]
    public float preDeathscreenPause;
    [BoxGroup("Overall")]
    public bool oneTimeActivateDeathscreen;
    [BoxGroup("Overall")]
    public Deathscreen deathscreen;
    [BoxGroup("Overall")]
    [ReadOnly]public AudioClip DeathMessage;
    [BoxGroup("Overall")]
    public plushes plushesManager;
    [Space]
    [BoxGroup("Overall")]
    public CompleteScreen completeScreen;
    [Space]
    [BoxGroup("Overall")]
    public Transform AnimatronicsCont;
    [BoxGroup("Overall")]
    public List<AnimatronicBase> Animatronics;

    public List<Animatronic> AnimatronicsNightListSave;

    public DiscordPresence NightPresence;

    private void Awake()
    {
        inctance = this;
    }
    void Start()
    {
        GM = GameManager.get();

        if (GM)
        {
            GM.SetCurrentNM(this);
            GM.FadeToTransparent();
        }
        NightOngoing = true;
        EscapingTimer = EscapeTime;

        foreach (Transform child in AnimatronicsCont)
        {
            if (child.TryGetComponent(out AnimatronicBase animat))
            {
                Animatronics.Add(animat);
            }
        }

        if (GM)
        {
            movement.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = GM.SaveData.VisualEffects;

            int randomMusic = Random.Range(0, 6);

            switch (randomMusic)
            {
                case 0:
                    GM.soundManager.CreateLoopingSound("Background Music", GM.soundManager.GetSoundFromList("WhereDreamsDie"));
                    break;
                case 1:
                    GM.soundManager.CreateLoopingSound("Background Music", GM.soundManager.GetSoundFromList("NoMoreSleep"));
                    break;
                case 2:
                    GM.soundManager.CreateLoopingSound("Background Music", GM.soundManager.GetSoundFromList("SonataForTheFallen"));
                    break;
                case 3:
                    GM.soundManager.CreateLoopingSound("Background Music", GM.soundManager.GetSoundFromList("LastBreath"));
                    break;
                case 4:
                    GM.soundManager.CreateLoopingSound("Background Music", GM.soundManager.GetSoundFromList("HybernatingEvil"));
                    break;
                case 5:

                    break;
                case 6:

                    break;
            }

            GM.soundManager.CreateLoopingSound("Ambiance", GM.soundManager.GetSoundFromList("Ambiance"));

            GM.soundManager.CreateLoopingSound("FanSound", GM.soundManager.GetSoundFromList("fan"));

            GM.soundManager.CreateIdleSource("door", GM.soundManager.GetSoundFromList("door"));
            GM.soundManager.CreateIdleSource("boop", GM.soundManager.GetSoundFromList("nose"));
            GM.soundManager.CreateIdleSource("MaskSounds", null);
            GM.soundManager.CreateIdleSource("CamSounds", null);

            AudioSource boxSource = GM.soundManager.CreateIdleSource("MusicBox", GM.soundManager.GetSoundFromList("musicBox1"));
            boxSource.Play();
            boxSource.loop = true;
            boxSource.Pause();

            AudioSource acSource = GM.soundManager.CreateLoopingSound("BGBuzzSound", GM.soundManager.GetSoundFromList("AC4"));
            acSource.Pause();

            MirrorPortal mirrorP = null;
            int amountOf20s = 0;
            for (int i = 0; i < GM.AnimatronicsNightList.Count; i++)
            {
                AnimatronicsNightListSave.Add(new Animatronic(GM.AnimatronicsNightList[i].Name, GM.AnimatronicsNightList[i].AILevel));
                NightPointsValue += GM.AnimatronicsNightList[i].AILevel * 10;

                if (GM.AnimatronicsNightList[i].AILevel == 20)
                {
                    amountOf20s++;
                }

                for (int b = 0; b < Animatronics.Count; b++)
                {
                    if (Animatronics[b].Name == "Mirror Portal")
                    {
                        mirrorP = (MirrorPortal)Animatronics[b];
                    }
                    if (Animatronics[b].Name == GM.AnimatronicsNightList[i].Name)
                    {
                        Animatronics[b].AILevel = GM.AnimatronicsNightList[i].AILevel;
                        Animatronics[b].GM = GM;
                        Animatronics[b].NM = this;
                        Animatronics[b].AnimatronicStart();
                    }
                }

                float costMultiplier = 1.5f;

                if (!GM.silent)
                    costMultiplier = 1;

                switch (GM.AnimatronicsNightList[i].Name)
                {
                    case "Michigun":
                        if (GM.AnimatronicsNightList[i].AILevel > 0)
                        {
                            triple.SetCost((int)(GM.AnimatronicsNightList[i].AILevel * costMultiplier));
                        }
                        else
                        {
                            triple.setEnabled(false);
                        }
                        break;
                    case "The Gate Keeper":
                        if (GM.AnimatronicsNightList[i].AILevel > 0)
                        {
                            emerald.SetCost((int)(GM.AnimatronicsNightList[i].AILevel * costMultiplier));
                        }
                        else
                        {
                            emerald.setEnabled(false);
                        }
                        break;
                    case "Mindcap":
                        if (GM.AnimatronicsNightList[i].AILevel > 0)
                        {
                            mindcap.SetCost((int)(GM.AnimatronicsNightList[i].AILevel * costMultiplier));
                        }
                        else
                        {
                            mindcap.setEnabled(false);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (GM.SaveData.VisualEffects)
            {
                systemConts[1].transform.rotation = Quaternion.Euler(-16.99f, 0, 0);
                systemConts[1].transform.localScale = new Vector3(1, 1.1f, 1);
                systemConts[2].transform.rotation = Quaternion.Euler(-16.99f, 0, 0);
                systemConts[2].transform.localScale = new Vector3(1, 1.1f, 1);
            }
            
            if (amountOf20s >= 50)
            {
                is5020Mode = true;
            }

            if (mirrorP != null)
            {
                mirrorP.AnimatronicStart();
            }

            if (GM.FrigidActive)
            {
                Temperature = 50;
                GM.SaveData.frigidsCount--;
                frigidLock = true;
            }
            else
            {
                Temperature = 60;
            }
            tempText.text = Temperature.ToString();

            if (GM.BatteryActive)
            {
                GM.SaveData.batteriesCount--;
                Power += 2;
            }

            if (GM.CoinsActive)
            {
                addFazCoins(3);
                GM.SaveData.coinsCount--;
            }

            GM.SaveGameData();
        }

        addToPower(1);
        addToNoise(1);
        CoolAmount++;
        if (GM != null)
        {
            if (GM.SelectedChallenge == "")
            {
                if (is5020Mode)
                {
                    NightPresence.Details = "In Night (Playing 50/20 Mode)";
                }
                else
                {
                    NightPresence.Details = "In Night (Point Value: " + NightPointsValue.ToString() + ")";
                }
            }
            else
            {
                NightPresence.Details = "In Night (Challenge: " + GM.SelectedChallenge + ")";
            }

            if (GM.silent)
                NightPresence.Details = NightPresence.Details.Insert(0, "{Silent Mode} ");

            NightPresence.LargeImage = "mainimage";

            DiscordController.get().SetPrecense(NightPresence, true);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && GM.SaveData.DebugMode)
        {
            debugging.OpenMenu();
        }

        if (NightOngoing)
        {
            WhileNightUpdate();
        }
        if (beingJumpscared)
        {
            JUmpscareShake();
        }
        if (isDead)
        {
            deathUpdate();
        }
        
        if (escaping && GM.FullyBlack)
        {
            GM.soundManager.DeleteAllSources();
            SceneManager.LoadScene(GM.MenuScene);
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.F1))
        {
            GM.AnimatronicsNightList = AnimatronicsNightListSave;

            GM.instaFadeBlack();

            for (int i = 0; i < Animatronics.Count; i++)
            {
                Animatronics[i].OnNightComplete();
            }

            StopAll();

            SceneManager.LoadScene(GM.NightScene);

            GM.FadeToTransparent();
        }
    }

    

    public void WhileNightUpdate()
    {
        //exit the game when holding escape
        if (Input.GetKey(KeyCode.Escape))
        {
            if (EscapingTimer > 0)
            {
                EscapingTimer -= Time.deltaTime;
                if (EscapingTimer <= 0)
                {
                    NightOngoing = false;
                    escaping = true;
                    GM.FadeToBlack();

                }
            }
        }
        else
        {
            EscapingTimer = EscapeTime;
        }

        if (onCamsButton && !zeroFrameCamsFix && !lockCamToggle && MonitorAnim.GetCurrentAnimatorStateInfo(0).IsName("camsDownIdle") || onCamsButton && !zeroFrameCamsFix && !lockCamToggle && MonitorAnim.GetCurrentAnimatorStateInfo(0).IsName("camsUpIdle"))
        {
            if (InCams && CamsFullyOpened)
            {
                lockCamToggle = true;
                ToggleCams();
            }
            else
            {
                lockCamToggle = true;
                ToggleCams();
            }
        }
        keyBinds();

        if (vetelationTimer > 0)
        {
            vetelationTimer -= Time.deltaTime * vetolationMutliplier;
        }
        else
        {
            if (Power > 0)
            {
                canResetVen = true;
                for (int i = 0; i < ResetVentStuff.Length; i++)
                {
                    ResetVentStuff[i].SetBool("needsReset", true);
                }
            }
            else
            {
                for (int i = 0; i < ResetVentStuff.Length; i++)
                {
                    ResetVentStuff[i].SetBool("needsReset", false);
                }
            }
            if (BigBlackscreen.color.a < 1)
            {
                BigBlackscreen.color += new Color(0,0,0, Time.deltaTime * 0.075f * vetolationMutliplier);
            }
            else
            {
                BigBlackscreen.color = new Color(0, 0, 0, 1);
            }
        }

        if (GameManager.DetectClickedOutside(freddyNose, false))
        {
            GM.soundManager.playSoundOnIdleSource("boop");
        }

        if (redFlashingTimer > 0)
        {
            redFlashingTimer -= Time.deltaTime;

            if (redFlashingTimer <= 0)
            {
                redFlashingTimer = flashingTime;
                NoiseBar.RedFlashing();
                UsageBar.RedFlashing();
                if (Temperature >= 100)
                {
                    tempTextScalePoint.gameObject.SetActive(!tempTextScalePoint.gameObject.activeSelf);
                }
            }
        }
        else
        {
            redFlashingTimer = flashingTime;
            NoiseBar.RedFlashing();
            UsageBar.RedFlashing();
            if (Temperature >= 100)
            {
                tempTextScalePoint.gameObject.SetActive(!tempTextScalePoint.gameObject.activeSelf);
            }
        }

        if (GameManager.DetectClickedOutside(signLeft, false) && !signOnRight)
        {
            signOnRight = true;
            signLeft.SetActive(false);
            signRight.SetActive(true);
            GM.soundManager.CreateSoundEffect("signMove", GM.soundManager.GetSoundFromList("sign"));
        }
        else if (GameManager.DetectClickedOutside(signRight, false) && signOnRight)
        {
            signOnRight = false;
            signLeft.SetActive(true);
            signRight.SetActive(false);
            GM.soundManager.CreateSoundEffect("signMove", GM.soundManager.GetSoundFromList("sign"));
        }

        if (EffectsBlackscreenActive)
        {
            if (EffectsBlackscreenTimer > 0)
            {
                EffectsBlackscreenTimer -= Time.deltaTime / 1.1f;
                EffectsBlackscreen.color = new Color(0, 0, 0, ((EffectsBlackscreenTimer * 2050) / 255));
                
                if (EffectsBlackscreenTimer <= 0)
                {
                    EffectsBlackscreenTimer = 0.1f;
                }
            }
            else
            {
                EffectsBlackscreenTimer = 0.1f;
            }
        }
        else
        {
            EffectsBlackscreen.color = new Color(0, 0, 0, 0);
        }

        if (PulseBlackscreen.color.a > 0)
        {
            PulseBlackscreen.color -= new Color(0, 0, 0, Time.deltaTime / 3.5f);
        }

        //calculate the timer
        TimeInSeconds += Time.deltaTime;
        TimeInTenthSeconds = (int)(TimeInSeconds * 10);

        int Minutes5020 = 0;
        int Seconds5020 = 0;
        float TSeconds5020 = TimeInTenthSeconds;

        while (TSeconds5020 >= 10)
        {
            TSeconds5020 -= 10;
            Seconds5020++;
        }

        while (Seconds5020 >= 60)
        {
            Seconds5020 -= 60;
            Minutes5020++;
        }
        if (Seconds5020 < 10)
        {
            timer.text = Minutes5020.ToString() + ":0" + Seconds5020 + "." + TSeconds5020;
        }
        else
        {
            timer.text = Minutes5020.ToString() + ":" + Seconds5020 + "." + TSeconds5020;

        }

        //set teh AMs text every specific amount of seconds
        if (Minutes5020 >= 0 && Minutes5020 < changeAMEvery)
        {
            //12 AM
            amTimer.text = "12";
        }
        else if (Minutes5020 >= changeAMEvery && Minutes5020 < changeAMEvery * 2)
        {
            //1 AM
            amTimer.text = "1";
        }
        else if (Minutes5020 >= changeAMEvery * 2 && Minutes5020 < changeAMEvery * 3)
        {
            //2 AM
            amTimer.text = "2";
        }
        else if (Minutes5020 >= changeAMEvery * 3 && Minutes5020 < changeAMEvery * 4)
        {
            //3 AM
            amTimer.text = "3";
        }
        else if (Minutes5020 >= changeAMEvery * 4 && Minutes5020 < changeAMEvery * 5)
        {
            //4 AM
            amTimer.text = "4";
        }
        else if (Minutes5020 >= changeAMEvery * 5 && Minutes5020 < changeAMEvery * 6)
        {
            //5 AM
            amTimer.text = "5";
        }
        else if (Minutes5020 >= changeAMEvery * 6)
        {
            //6 AM
            amTimer.text = "6";
            //night completed
            NightOngoing = false;
            nightComplete = true;
            movement.MovemendEnabled = false;

            TimeInSeconds = 60*6;
            TimeInTenthSeconds = (int)(TimeInSeconds * 10);

            timer.text = "6:00.0";

            for (int i = 0; i < Animatronics.Count; i++)
            {
                Animatronics[i].OnNightComplete();
            }

            SwitchSpecialMode(SpecialModes.Off);
            StopAll();

            MaskAnim.speed = 0;
            MonitorAnim.speed = 0;
            
            GM.addChallengeCompleted();

            completeScreen.ActivateCompleteScreen(NightPointsValue, TimeInTenthSeconds, is5020Mode, this);
        }
        PowerCalc();
        TempCalc();

        if (pickedDCoin)
        {
            if (getAnimatronicAtPoint(Input.mousePosition) != null)
            {
                dCoinFollowing.GetComponent<Image>().sprite = dcoinon;
                dCoinFollowing.transform.localScale = Vector3.one * 2.3f;
                dcoinText.gameObject.SetActive(true);
            }
            else
            {
                dCoinFollowing.GetComponent<Image>().sprite = dcoinoff;
                dCoinFollowing.transform.localScale = Vector3.one * 2;
                dcoinText.gameObject.SetActive(false);
            }
        }

        if (pickedDCoin && Input.GetMouseButtonUp(0))
        {
            AnimatronicBase animatB = getAnimatronicAtPoint(Input.mousePosition);
            if (animatB != null)
            {
                dCoin.SetActive(false);
                animatB.OnDeathcoined();
                GM.soundManager.CreateSoundEffect("block", GM.soundManager.GetSoundFromList("block"));
                for (int i = 0; i < GM.AnimatronicsNightList.Count; i++)
                {
                    if (GM.AnimatronicsNightList[i].Name == animatB.Name)
                    {
                        GM.AnimatronicsNightList[i].AILevel = 0;
                    }
                }
            }
            else
            {
                dCoin.SetActive(true);
            }
            pickedDCoin = false;
            dCoinFollowing.SetActive(false);
        }

        if (!nightComplete)
        {
            if (GM != null)
            {
                NightPresence.State = "Power: " + PowerText.text + "%, Tempreture: " + tempText.text + "°, Coins: " + FazCoins;
                DiscordController.get().SetPrecense(NightPresence, false);
            }
        }

        debugging.UpdateNightVariables(Power, UsageLevel, NoiseLevel, FazCoins, Temperature, CamSys.CurrentCamera, CurrSpecialMode);
    }

    void PowerCalc()
    {
        if (Power > 0)
        {
            int usageDeMaxed = UsageLevel;
            if (usageDeMaxed > 6)
            {
                usageDeMaxed = 6;
            }
            if (CurrSpecialMode == SpecialModes.PowerGen)
            {
                Power += (Time.deltaTime / 25) / 1.5f;
            }

            Power -= ((Time.deltaTime * ((usageDeMaxed + 1))) / (25 - (usageDeMaxed * 2.5f)) * 5) / 2.75f;
        }
        
        if (Power <= 0)
        {
            Power = 0;
            if (!LockpowerOutage)
            {
                LockpowerOutage = true;
                OnPowerOut();
            }
        }

        PowerText.text = ((int)Power).ToString();
    }

    void TempCalc()
    {
        if (Temperature >= 50 && Temperature <= 120)
        {
            float maxSlowdown = 1;
            if (Temperature >= 100 && Temperature < 110)
            {
                maxSlowdown = 0.65f;
            }
            else if (Temperature >= 110)
            {
                maxSlowdown = 0.45f;
            }
            if (frigidLock)
            {
                int prevTemp = (int)Temperature;
                Temperature += (((Time.deltaTime * (HeatAmount + 1)) * maxSlowdown));

                Temperature -= Time.deltaTime * (CoolAmount * 3);

                if (Temperature < prevTemp)
                {
                    Temperature = prevTemp;
                }
            }
            else
            {
                Temperature += (((Time.deltaTime * (HeatAmount + 1)) * maxSlowdown));

                Temperature -= Time.deltaTime * (CoolAmount * 3);
            }

        }
        if (frigidLock)
        {
            if (Temperature >= 60)
            {
                frigidLock = false;
            }
        }
        else
        {
            if (Temperature < 60)
            {
                Temperature = 60;
            }
        }

        if (Temperature > 120)
        {
            Temperature = 120;
        }
        if (Temperature >= 120)
        {
            if (fastVentelation)
            {
                vetolationMutliplier = 3;
            }
            else
            {
                vetolationMutliplier = 2;
            }
        }
        else
        {
            if (fastVentelation)
            {
                vetolationMutliplier = 2;
            }
            else
            {
                vetolationMutliplier = 1;
            }
        }
        tempText.text = ((int)Temperature).ToString();

        if (Temperature >= 100)
        {
            tempTextScalePoint.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            tempText.color = new Color32(235, 61, 61, 255);
            tempTextTempIndic.color = new Color32(235, 61, 61, 255);
        }
        else
        {
            tempTextScalePoint.localScale = Vector3.one;
            tempText.color = new Color32(255, 255, 255, 145);
            tempTextTempIndic.color = new Color32(255, 255, 255, 145);
            tempTextScalePoint.gameObject.SetActive(true);
        }
    }

    IEnumerator zeroFrameFixWait()
    {
        yield return new WaitForEndOfFrame();
        yield return null;
        zeroFrameCamsFix = false;
    }

    public void StopAll(string dontMuteSpecific = "")
    {
        Desk.anim.speed = 0;

        movement.MovemendEnabled = false;
        leftDoorAnim.speed = 0;
        rightDoorAnim.speed = 0;
        topDoorAnim.speed = 0;
        bottomRightDoorAnim.speed = 0;

        if (dontMuteSpecific == "")
        {
            SoundManager.getSoundManager().DeleteAllSources();
        }
        else
        {
            SoundManager.getSoundManager().DeleteAllSources(dontMuteSpecific);
        }

        for (int i = 0; i < CamSys.CamStatic.Length; i++)
        {
            CamSys.CamStatic[i].GetComponent<Animator>().speed = 0;
        }
    }

    void toggleDoor(doors door)
    {
        switch (door)
        {
            case doors.left:
                if (EnabledDoorControls && !IsPowerOut)
                {
                    if (leftDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftDoorOpenIdle") || leftDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftDoorCloseIdle"))
                    {
                        leftDoorClosed = !leftDoorClosed;
                        leftDoorAnim.SetBool("doorClosed", leftDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        if (leftDoorClosed)
                        {
                            addToPower(1);
                        }
                        else
                        {
                            addToPower(-1);
                        }
                        OnDoorStateChanged(doors.left, leftDoorClosed);
                    }
                }
                else
                {
                    if (leftDoorClosed && leftDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftDoorCloseIdle"))
                    {
                        leftDoorClosed = !leftDoorClosed;
                        leftDoorAnim.SetBool("doorClosed", leftDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        addToPower(-1);
                    }
                    else
                    {
                        GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
                    }
                }
                break;
            case doors.top:
                if (EnabledDoorControls && !IsPowerOut )
                {
                    if (topDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("TopDoorOpenIdle") || topDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("TopDoorCloseIdle"))
                    {
                        topDoorClosed = !topDoorClosed;
                        topDoorAnim.SetBool("doorClosed", topDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        if (topDoorClosed)
                        {
                            addToPower(1);
                        }
                        else
                        {
                            addToPower(-1);
                        }
                        OnDoorStateChanged(doors.top, topDoorClosed);
                    }
                }
                else
                {
                    if (topDoorClosed && topDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("TopDoorCloseIdle"))
                    {
                        topDoorClosed = !topDoorClosed;
                        topDoorAnim.SetBool("doorClosed", topDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        addToPower(-1);
                    }
                    else
                    {
                        GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
                    }
                }
                break;
            case doors.right:
                if (EnabledDoorControls && !IsPowerOut)
                {
                    if (rightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RightDoorOpenIdle") || rightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RightDoorCloseIdle"))
                    {
                        rightDoorClosed = !rightDoorClosed;
                        rightDoorAnim.SetBool("doorClosed", rightDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        if (rightDoorClosed)
                        {
                            addToPower(1);
                        }
                        else
                        {
                            addToPower(-1);
                        }
                        OnDoorStateChanged(doors.right, rightDoorClosed);
                    }
                }
                else
                {
                    if (rightDoorClosed && rightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RightDoorCloseIdle"))
                    {
                        rightDoorClosed = !rightDoorClosed;
                        rightDoorAnim.SetBool("doorClosed", rightDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        addToPower(-1);
                    }
                    else
                    {
                        GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
                    }
                }
                break;
            case doors.bottomRight:
                if (EnabledDoorControls && !IsPowerOut)
                {
                    if (bottomRightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("bottomRightDoorCloseIdle") || bottomRightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("bottomRightDoorOpenIdle"))
                    {
                        bottomRightDoorClosed = !bottomRightDoorClosed;
                        bottomRightDoorAnim.SetBool("doorClosed", bottomRightDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        if (bottomRightDoorClosed)
                        {
                            addToPower(1);
                        }
                        else
                        {
                            addToPower(-1);
                        }
                        OnDoorStateChanged(doors.bottomRight, bottomRightDoorClosed);
                    }
                    
                }
                else
                {
                    if (bottomRightDoorClosed && bottomRightDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("bottomRightDoorCloseIdle"))
                    {
                        bottomRightDoorClosed = !bottomRightDoorClosed;
                        bottomRightDoorAnim.SetBool("doorClosed", bottomRightDoorClosed);
                        GM.soundManager.playSoundOnIdleSource("door");
                        addToPower(-1);
                    }
                    else
                    {
                        GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
                    }
                }

                break;
        }
    }

    public void ToggleMask()
    {
        if (!InCams)
        {
            InMask = !InMask;
            if (InMask)
            {
                CamsButton.enabled = false;
                MaskAnim.SetBool("UsingMask", InMask);
                ColMaskAnim.SetBool("UsingMask", InMask);
                ResetVentStuff[2].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                GM.soundManager.playSoundOnIdleSource("MaskSounds", GM.soundManager.GetSoundFromList("maskOn"));
            }
            else
            {
                CamsButton.enabled = true;
                MaskAnim.SetBool("UsingMask", InMask);
                ColMaskAnim.SetBool("UsingMask", InMask);
                ResetVentStuff[2].GetComponent<Image>().color = Color.white;
                GM.soundManager.playSoundOnIdleSource("MaskSounds", GM.soundManager.GetSoundFromList("maskOff"));
            }
        }
    }

    void toggleFlashlight(bool b)
    {
        if (!FlashlightDisabled)
        {
            if (Power <= 0) return;
            if (!b)
            {
                if (FlashlightOn)
                {
                    FlashlightOn = false;
                    GM.soundManager.CreateSoundEffect("flashclick", GM.soundManager.GetSoundFromList("flashclick"));
                    Flashlight.SetActive(false);
                    addToPower(-1);
                }
            }
            else
            {
                if (!FlashlightOn)
                {
                    FlashlightOn = true;
                    GM.soundManager.CreateSoundEffect("flashclick", GM.soundManager.GetSoundFromList("flashclick"));
                    Flashlight.SetActive(true);
                    addToPower(1);
                }
            }
        }
    }

    public void flashlightDisabled(bool b)
    {
        if (b != FlashlightDisabled)
        {
            if (b)
            {
                toggleFlashlight(false);
                FlashlightDisabled = true;
            }
            else
            {
                FlashlightDisabled = false;
            }
        }
    }

    void keyBinds()
    {
        if (Input.GetKeyDown(KeyCode.J) && GM.SaveData.DebugMode)
        {
            TimeInSeconds += 30;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            toggleDoor(doors.bottomRight);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            toggleDoor(doors.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            toggleDoor(doors.right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            toggleDoor(doors.top);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.PowerGen);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.SilentVent);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.Heater);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.PowerAC);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.GMB);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.Off);
        }

        if (Input.GetKeyDown(KeyCode.X) && Power > 0)
        {
            SwitchSpecialMode(SpecialModes.Off);
        }

        if (Input.GetKeyDown(KeyCode.S) && !zeroFrameCamsFix && MonitorAnim.GetCurrentAnimatorStateInfo(0).IsName("camsDownIdle") || Input.GetKeyDown(KeyCode.S) && !zeroFrameCamsFix && MonitorAnim.GetCurrentAnimatorStateInfo(0).IsName("camsUpIdle"))
        {
            if (InCams && CamsFullyOpened)
            {
                ToggleCams();
            }
            else
            {
                ToggleCams();
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            toggleFlashlight(true);
        }
        else
        {
            if (!FlashlightDisabled) toggleFlashlight(false);
        }
        if (Input.GetKeyDown(KeyCode.Z) && FlashlightDisabled)
        {
            GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !IsPowerOut)
        {
            Desk.TurnFan();

            if (Desk.FanOn)
            {
                addToPower(1);
                addToNoise(1);
                CoolAmount++;
            }
            else
            {
                addToPower(-1);
                addToNoise(-1);
                CoolAmount--;
            }
        }
    }

    public void OnCamsButtonEntered()
    {
        onCamsButton = true;
        lockCamToggle = false;
    }

    public void OnCamsButtonExit()
    {
        onCamsButton = false;
    }

    public void ToggleCams()
    {
        if (!InMask)
        {
            if (CamsAllowed && !IsPowerOut)
            {
                InCams = !InCams;
                movement.MovemendEnabled = false;
                if (InCams)
                {
                    MonitorAnim.SetBool("InCams", InCams);
                    ResetVentStuff[1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    ResetVentStuff[2].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    GM.soundManager.playSoundOnIdleSource("CamSounds", GM.soundManager.GetSoundFromList("camsOn"));
                    addToPower(1);
                }
                else
                {
                    MaskButton.enabled = true;
                    movement.MovemendEnabled = true;
                    MonitorAnim.SetBool("InCams", InCams);
                    CamsContainer.SetActive(false);
                    CamsFullyOpened = false;
                    ResetVentStuff[1].GetComponent<Image>().color = Color.white;
                    ResetVentStuff[2].GetComponent<Image>().color = Color.white;
                    GM.soundManager.playSoundOnIdleSource("CamSounds", GM.soundManager.GetSoundFromList("camsOff"));
                    addToPower(-1);
                    if (CamSys.CurrentCamera == cameraSystem.Cameras.Cam04 && CurrSpecialMode != SpecialModes.GMB)
                    {
                        if (GM.soundManager.getActiveSource("MusicBox") != null)
                        {
                            GM.soundManager.getActiveSource("MusicBox").Pause();
                            coinDispCont.SetActive(true);
                        }
                    }

                    if (GM.SaveData.VisualEffects)
                    {
                        PostUICamera.GetUniversalAdditionalCameraData().renderPostProcessing = false;
                        movement.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = true;
                        GlobalPostVol.profile = WorldPostVol;
                    }
                }
            }
            else
            {
                GM.soundManager.CreateSoundEffect("error", GM.soundManager.GetSoundFromList("error"));
            }
        }
    }

    public void LockCams(bool b)
    {
        if (b)
        {
            if (InCams)
            {
                ToggleCams();
            }
        }
        CamsAllowed = b;
    }

    public void OnFullyEnteredCams()
    {
        CamsContainer.SetActive(true);
        CamsFullyOpened = true;
        MaskButton.enabled = false;
        CamSys.PulseStatic();
        if (CamSys.CurrentCamera == cameraSystem.Cameras.Cam04)
        {
            if (GM.soundManager.getActiveSource("MusicBox") != null)
            {
                GM.soundManager.getActiveSource("MusicBox").UnPause();
            }
            coinDispCont.SetActive(false);
        }
        CamSys.addCoinsToCamera(CamSys.CurrentCamera);
        if (GM.SaveData.VisualEffects)
        {
            PostUICamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            movement.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = false;
            GlobalPostVol.profile = CamsPostVol;
        }

        zeroFrameCamsFix = true;
        StartCoroutine(zeroFrameFixWait());
    }

    public void addToPower(int amount)
    {
        UsageLevel += amount;
        UsageBar.changeValue(UsageLevel);
    }
    public void addToNoise(int amount)
    {
        NoiseLevel += amount;
        NoiseBar.changeValue(NoiseLevel);
    }

    void OnPowerOut()
    {
        GM.soundManager.CreateSoundEffect("powerDown", GM.soundManager.GetSoundFromList("powerDown"));
        vetelationTimer = -1;
        fastVentelation = true;
        canResetVen = false;
        if (bottomRightDoorClosed)
        {
            toggleDoor(doors.bottomRight);
        }

        if (leftDoorClosed)
        {
            toggleDoor(doors.left);
        }

        if (rightDoorClosed)
        {
            toggleDoor(doors.right);
        }

        if (topDoorClosed)
        {
            toggleDoor(doors.top);
        }

        if (InCams)
        {
            ToggleCams();
        }

        if (FlashlightOn)
        {
            FlashlightOn = false;
            GM.soundManager.CreateSoundEffect("flashclick", GM.soundManager.GetSoundFromList("flashclick"));
            Flashlight.SetActive(false);
            addToPower(-1);
        }

        if (CurrSpecialMode != SpecialModes.Off)
        {
            SwitchSpecialMode(SpecialModes.Off);
        }

        if (Desk.FanOn)
        {
            Desk.TurnFan();

            addToPower(-1);
            addToNoise(-1);
            CoolAmount--;
        }

        Office.lighting = offcie.officeLightings.PowerDown;
        GM.soundManager.DeleteSource("Background Music");

        IsPowerOut = true;
    }

    public void addFazCoins(int amount)
    {
        FazCoins += amount;
        fazCoinstext.text = FazCoins.ToString();
    }

    public void SwitchSpecialMode(SpecialModes mode)
    {
        SpecialModes prefiMode = CurrSpecialMode;
        CurrSpecialMode = mode;
        GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));

        if (prefiMode == mode && mode != SpecialModes.Off)
        {
            SwitchSpecialMode(SpecialModes.Off);
        }
        else
        {
            //remove previ mode
            switch (prefiMode)
            {
                case SpecialModes.PowerGen:
                    addToNoise(-2);
                    break;
                case SpecialModes.SilentVent:
                    addToPower(-1);
                    break;
                case SpecialModes.Heater:
                    addToPower(-1);
                    addToNoise(-1);
                    HeatAmount -= 5;
                    break;
                case SpecialModes.PowerAC:
                    addToPower(-1);
                    addToNoise(-1);
                    CoolAmount -= 1;
                    break;
                case SpecialModes.GMB:

                    if (!CamsFullyOpened)
                    {
                        if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").Pause();
                    }
                    else
                    {
                        if (CamSys.CurrentCamera != cameraSystem.Cameras.Cam04)
                        {
                            if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").Pause();
                        }
                    }
                    if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").volume = 1;
                    addToPower(-1);
                    break;

                default:
                    break;
            }
            //add the current selected mode
            switch (mode)
            {
                case SpecialModes.PowerGen:
                    addToNoise(2);
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[0].GetChild(0).transform.position.x, SpecialModeButtons[0].GetChild(0).transform.position.y, 0);
                    break;
                case SpecialModes.SilentVent:
                    addToPower(1);
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[1].GetChild(0).transform.position.x, SpecialModeButtons[1].GetChild(0).transform.position.y, 0);
                    break;
                case SpecialModes.Heater:
                    addToPower(1);
                    addToNoise(1);
                    HeatAmount += 5;
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[2].GetChild(0).transform.position.x, SpecialModeButtons[2].GetChild(0).transform.position.y, 0);
                    if (Desk.FanOn)
                    {
                        Desk.TurnFan();

                        addToPower(-1);
                        addToNoise(-1);
                        CoolAmount--;
                    }
                    break;
                case SpecialModes.PowerAC:
                    addToPower(1);
                    addToNoise(1);
                    CoolAmount += 1;
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[3].GetChild(0).transform.position.x, SpecialModeButtons[3].GetChild(0).transform.position.y, 0);
                    break;
                case SpecialModes.GMB:
                    if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").UnPause();
                    if (GM.soundManager.getActiveSource("MusicBox") != null) GM.soundManager.getActiveSource("MusicBox").volume = 0.5f;
                    addToPower(1);
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[4].GetChild(0).transform.position.x, SpecialModeButtons[4].GetChild(0).transform.position.y, 0);
                    break;
                case SpecialModes.Off:
                    SelectDotSpecials.position = new Vector3(SpecialModeButtons[5].GetChild(0).transform.position.x, SpecialModeButtons[5].GetChild(0).transform.position.y, 0);
                    break;
            }
        }

        if (CurrSpecialMode != SpecialModes.Off && CurrSpecialMode != SpecialModes.GMB && CurrSpecialMode != SpecialModes.SilentVent)
        {
            GM.soundManager.getActiveSource("BGBuzzSound").UnPause();
        }
        else
        {
            GM.soundManager.getActiveSource("BGBuzzSound").Pause();
        }
    }

    public void SwitchSpecialMode(int mode)
    {
        SwitchSpecialMode((SpecialModes)mode);
    }

    public void ItemBought(string itemName)
    {
        switch (itemName)
        {
            case "DeathCoin":
                boughtDeathCoin = true;
                dCoin.SetActive(true);
                break;
            case "triple":
                plushesManager.BoughtTriple();
                break;
            case "emerald":
                plushesManager.BoughtEmerald();
                break;
            case "mindcap":
                plushesManager.BoughtMindcap();
                break;
        }
    }

    public void OnCamSysChanged(camSystems sysType)
    {
        for (int i = 0; i < systemConts.Length; i++)
        {
            if (i == (int)sysType)
            {
                systemConts[i].SetActive(true);
            }
            else
            {
                systemConts[i].SetActive(false);
            }
        }
        if (sysType == camSystems.ventSys)
        {
            foreach (Transform child in VentIconCont)
            {
                if (child.TryGetComponent(out ventPath icon))
                {
                    icon.UpdatePath();
                }
            }
        }
        if (sysType == camSystems.ductSys)
        {
            foreach (Transform child in DuctIconCont)
            {
                if (child.TryGetComponent(out ductPath icon))
                {
                    icon.UpdatePos();
                }
            }
        }
    }

    public void ResetVent()
    {
        GM.soundManager.CreateSoundEffect("goofierClick", GM.soundManager.GetSoundFromList("mouseSnap"));
        if (canResetVen)
        {
            canResetVen = false;
            vetelationTimer = 30;
            for (int i = 0; i < ResetVentStuff.Length; i++)
            {
                ResetVentStuff[i].SetBool("needsReset", false);
            }
            BigBlackscreen.color = new Color(0, 0, 0, 0);
        }
    }

    public void pulseBlackscreen()
    {
        PulseBlackscreen.color = Color.black;
    }

    public void StartEffectsBlackscreen(bool b)
    {
        if (b)
        {
            isEffectsActive++;
        }
        else
        {
            isEffectsActive--;
        }
        if (isEffectsActive > 0)
        {
            EffectsBlackscreenActive = true;
        }
        else
        {
            EffectsBlackscreenActive = false;
        }
    }

    public GameObject Jumpscare(AnimationClip anim, AudioClip sound, bool isLethal, bool TakeDownCams, bool TakeDownMask, AudioClip[] deathVoices, string characterName, Vector2? _SizeDelta = null, float Scale = 8, string dontMuteSpecific = "", float JumpscareVolume = 1, System.Action callback = null)
    {
        if (beingJumpscared || isDead) return null;
        Vector2 SizeDelta;
        if (_SizeDelta == null)
        {
            SizeDelta = new Vector2(480, 270);
        }
        else
        {
            SizeDelta = _SizeDelta.Value;
        }

        GameObject Jumpscare = Instantiate(jumpscarePrefab, jumpscaresCont);
        Jumpscares.Add(Jumpscare);

        Jumpscare.GetComponent<jumpscareTrackerS>().Mycallback = callback;

        if (TakeDownCams)
        {
            MonitorAnim.Play("Force Cams Down");
        }

        if (TakeDownMask)
        {
            if (InMask)
            {
                ToggleMask();
            }
        }

        if (isLethal)
        {
            SwitchSpecialMode(SpecialModes.Off);
            beingJumpscared = true;
            NightOngoing = false;
            StopAll(dontMuteSpecific);
            movement.MovemendEnabled = false;
            for (int i = 0; i < Jumpscares.Count; i++)
            {
                if (Jumpscares[i] != Jumpscare)
                {
                    Destroy(Jumpscares[i]);
                    Jumpscares.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < Animatronics.Count; i++)
            {
                Animatronics[i].OnPlayerDied();
            }

            NightPresence.State = "DEAD - killed by " + characterName + ", Time reached: " + timer.text;
            DiscordController.get().SetPrecense(NightPresence, true);
        }

        if (deathVoices != null)
        {
            if (deathVoices.Length > 0)
            {
                int randomVoice = Random.Range(0, deathVoices.Length);

                DeathMessage = deathVoices[randomVoice];
            }
        }

        AnimatorOverrideController aoc = new AnimatorOverrideController(Jumpscare.GetComponent<Animator>().runtimeAnimatorController);
        aoc[Jumpscare.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name] = anim;
        Jumpscare.GetComponent<Animator>().runtimeAnimatorController = aoc;
        List<KeyValuePair<AnimationClip, AnimationClip>>  overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        aoc.GetOverrides(overrides);

        for (int i = 0; i < overrides.Count; ++i)
        {
            overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, anim);
        }
        
        aoc.ApplyOverrides(overrides);

        Jumpscare.GetComponent<RectTransform>().sizeDelta = SizeDelta;
        Jumpscare.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);
        if (sound != null) GM.soundManager.CreateSoundEffect("JumpscareSound", sound).volume = JumpscareVolume;
        return Jumpscare;
    }

    bool jumpShakeDisabled = false;
    public GameObject Jumpscare(VideoClip video, AudioClip sound, bool isLethal, bool TakeDownCams, bool TakeDownMask, AudioClip[] deathVoices, string characterName, float Scale = 8)
    {
        if (beingJumpscared) return null;

        GameObject Jumpscare = Instantiate(GM.emptyObject, jumpscaresCont);
        Jumpscares.Add(Jumpscare);

        if (TakeDownCams)
        {
            if (InCams)
            {
                ToggleCams();
                MonitorAnim.Play("camsUpIdle 0");
            }
        }

        if (TakeDownMask)
        {
            if (InMask)
            {
                ToggleMask();
            }
        }

        if (isLethal)
        {
            SwitchSpecialMode(SpecialModes.Off);
            beingJumpscared = true;
            NightOngoing = false;
            jumpShakeDisabled = true;
            StopAll();
            for (int i = 0; i < Jumpscares.Count; i++)
            {
                if (Jumpscares[i] != Jumpscare)
                {
                    Destroy(Jumpscares[i]);
                    Jumpscares.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < Animatronics.Count; i++)
            {
                Animatronics[i].OnPlayerDied();
            }

            NightPresence.State = "DEAD - killed by " + characterName + ", Time reached: " + timer.text;
            DiscordController.get().SetPrecense(NightPresence, true);
        }

        if (deathVoices != null)
        {
            if (deathVoices.Length > 0)
            {
                int randomVoice = Random.Range(0, deathVoices.Length);

                DeathMessage = deathVoices[randomVoice];
            }
        }

        VideoPlayer player = Jumpscare.AddComponent<VideoPlayer>();
        player.playOnAwake = false;
        player.clip = video;
        player.targetTexture = new RenderTexture((int)video.width, (int)video.height, 5);
        player.SetDirectAudioMute(0, true);
        player.SetDirectAudioMute(1, true);

        RawImage myImage = Jumpscare.AddComponent<RawImage>();
        myImage.texture = player.targetTexture;
        myImage.SetNativeSize();
        player.Play();
        player.loopPointReached += EndReached;

        Jumpscare.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);
        if (sound != null) GM.soundManager.CreateSoundEffect("JumpscareSound", sound);
        return Jumpscare;
    }

    void EndReached(VideoPlayer vp)
    {
        GameObject theJumpscareInQustion = vp.gameObject;
        if (beingJumpscared)
        {
            beingJumpscared = false;
            theJumpscareInQustion.GetComponent<VideoPlayer>().enabled = false;
            isDead = true;
        }
        else
        {
            for (int i = 0; i < Jumpscares.Count; i++)
            {
                if (Jumpscares[i] == theJumpscareInQustion)
                {
                    Destroy(Jumpscares[i]);
                    Jumpscares.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void JUmpscareShake()
    {
        if (jumpShakeDisabled) return;

        if (JumpscareRotationLeft)
        {
            JumpscareRotation += Time.deltaTime * JumpscareRotationSpeed;
        }
        else
        {
            JumpscareRotation -= Time.deltaTime * JumpscareRotationSpeed;
        }

        if (JumpscareRotation >= JumpscareMaxRotation)
        {
            JumpscareRotationLeft = false;
            JumpscareRotation = JumpscareMaxRotation;
        }

        if (JumpscareRotation <= -JumpscareMaxRotation)
        {
            JumpscareRotationLeft = true;
            JumpscareRotation = -JumpscareMaxRotation;
        }

        var rot = movement.transform.eulerAngles;
        rot.z = JumpscareRotation;
        movement.transform.eulerAngles = rot;

        var rot2 = AllUICont.eulerAngles;
        rot2.z = -JumpscareRotation;
        AllUICont.eulerAngles = rot2;
    }

    public void JumpscareEnded(GameObject theJumpscareInQustion)
    {
        if (beingJumpscared)
        {
            beingJumpscared = false;
            theJumpscareInQustion.GetComponent<Animator>().enabled = false;
            isDead = true;
        }
        else
        {
            for (int i = 0; i < Jumpscares.Count; i++)
            {
                if (Jumpscares[i] == theJumpscareInQustion)
                {
                    Destroy(Jumpscares[i]);
                    Jumpscares.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void deathUpdate()
    {
        if (preDeathscreenPause > 0)
        {
            preDeathscreenPause -= Time.deltaTime;
        }
        else
        {
            if (!oneTimeActivateDeathscreen)
            {
                oneTimeActivateDeathscreen = true;
                deathscreen.ActivateDeathscreen(timer.text, TimeInTenthSeconds, is5020Mode, NightPointsValue, DeathMessage);
            }
        }
    }

    public void SetPower(string power)
    {
        Power = float.Parse(power);
    }

    public void SetUsage(string Usage)
    {
        UsageLevel = int.Parse(Usage);
        UsageBar.changeValue(UsageLevel);
    }

    public void SetNoise(string Noise)
    {
        NoiseLevel = int.Parse(Noise);
        NoiseBar.changeValue(NoiseLevel);
    }

    public void SetFazCoins(string FazCoin)
    {
        FazCoins += int.Parse(FazCoin);
        fazCoinstext.text = FazCoins.ToString();
    }

    public void SetTemperature(string _Temperature)
    {
        Temperature = float.Parse(_Temperature);
    }

    public bool lookingAtLeft()
    {
        if (movement.transform.localPosition.x <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void pickUpDeathcoin()
    {
        dCoin.SetActive(false);
        dCoinFollowing.SetActive(true);
        dCoinFollowing.transform.position = NoPostUICamera.ScreenToWorldPoint(Input.mousePosition);
        pickedDCoin = true;
    }

    public AnimatronicBase getAnimatronicAtPoint(Vector3 pointInScreenSpace)
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out animatronicRelated an))
            {
                if (an.animatronic.AILevel == 0)
                {
                    return null;
                }
                else
                {
                    dcoinText.text = an.animatronic.Name;
                    return an.animatronic;
                }
            }
        }


        RaycastHit2D[] ray = Physics2D.RaycastAll(movement.GetComponent<Camera>().ScreenToWorldPoint(pointInScreenSpace), new Vector2(0, 0), 1000);

        float biggestZ = 0;

        GameObject hitObj = null;
        for (int i = 0; i < ray.Length; i++)
        {
            if (ray[i].transform.GetComponent<animatronicRelated>() != null)
            {
                if (biggestZ > ray[i].transform.position.z)
                {
                    biggestZ = ray[i].transform.position.z;
                    hitObj = ray[i].transform.gameObject;
                }
            }
        }

        if (hitObj != null)
            if (biggestZ < hitObj.transform.position.z)
            {
                return null;
            }

        if (hitObj == null) return null;

        if (hitObj.TryGetComponent(out animatronicRelated animat))
        {
            if (animat.animatronic.Name == "Solar Flare Sun")
            {
                if (((solarFlareSun)animat.animatronic).Sun.color.a <= 0)
                {
                    return null;
                }
            }
            else if (animat.animatronic.Name == "Bloodbath")
            {
                if (((bloodbath)animat.animatronic).Face.color.a <= 0)
                {
                    return null;
                }
            }
            else if (animat.animatronic.Name == "Magma Bound")
            {
                if (((magmaBound)animat.animatronic).Spaceicon.color.a <= 0)
                {
                    return null;
                }
            }
        }

        Vector2 pos = (Vector2)movement.GetComponent<Camera>().ScreenToWorldPoint(pointInScreenSpace);
        if (hitObj.activeSelf && hitObj.GetComponent<Collider2D>().OverlapPoint(pos))
        {
            if (hitObj.TryGetComponent(out animatronicRelated animatt))
            {
                dcoinText.text = animatt.animatronic.Name;
                return animatt.animatronic;
            }
        }
        return null;
    }

    public void OnAnyCamSysChange(camSystems sys)
    {
        OnCamSysSwitch(sys);
    }
}