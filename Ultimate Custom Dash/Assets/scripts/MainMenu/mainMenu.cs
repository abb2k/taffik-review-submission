using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static offcieLayer;

public class mainMenu : MonoBehaviour
{
    GameManager GM;

    [BoxGroup("References")]
    public AudioClip MenuMusic;

    [BoxGroup("References")]
    public TextMeshProUGUI pointValText;
    [BoxGroup("References")]
    public TextMeshProUGUI HighScoreText;
    [BoxGroup("References")]
    public TextMeshProUGUI best5020Mode;
    [BoxGroup("References")]
    public Slider volumeSlider;
    [BoxGroup("References")]
    public TextMeshProUGUI volumeText;
    [BoxGroup("References")]
    public Toggle showcharInfoToggle;
    [BoxGroup("References")]
    public Toggle VisualEffectsToggle;
    [BoxGroup("References")]
    public EventSystem eventSys;
    [BoxGroup("References")]
    public GameObject glitch;
    [BoxGroup("References")]
    public Shake shake;
    [BoxGroup("References")]
    public Shake troShake;
    [BoxGroup("References")]
    public Volume gloablVol;
    [BoxGroup("References")]
    public Button silentButton;
    [BoxGroup("References")]
    public Sprite silentButtonOff, silentButtonOffHover, silentButtonOn, silentButtonOnHover;
    [BoxGroup("References")]
    public Image pointValLabel;

    [BoxGroup("Menus")]
    public GameObject SideMenu;
    [BoxGroup("Menus")]
    public GameObject officesMenu;
    [BoxGroup("Menus")]
    public GameObject PowerUpsMenu;
    [BoxGroup("Menus")]
    public GameObject ChallengesMenu;
    [BoxGroup("Menus")]
    public GameObject settinsMenu;
    [BoxGroup("Menus")]
    public GameObject CreditsMenu;

    [BoxGroup("Other")]
    public DiscordPresence MainMenuPresence;
    [BoxGroup("Other")]
    public GameObject trophies;
    [BoxGroup("Other")]
    [ShowAssetPreview]
    public Sprite brozeRank;
    [BoxGroup("Other")]
    [ShowAssetPreview]
    public Sprite silverRank;
    [BoxGroup("Other")]
    [ShowAssetPreview]
    public Sprite goldRank;
    [BoxGroup("Other")]
    [ShowAssetPreview]
    public Sprite diamondRank;
    [BoxGroup("Other")]
    [ReadOnly][SerializeField] bool StartingNight;
    [BoxGroup("Other")]
    public bool allowedToShowCellUI;
    [BoxGroup("Other")]
    public float CellOnOpacity;
    [BoxGroup("Other")]
    public float CellOnOpacitySpeed;
    [BoxGroup("Other")]
    public float CellOnOpacityMinimum;
    [BoxGroup("Other")]
    [ReadOnly][SerializeField] float EraseDataTimer = 2;
    [BoxGroup("Other")]
    [ReadOnly][SerializeField] bool CellOnOpacityGoUp;
    [BoxGroup("Other")]
    public Transform animatronicCellContainer;
    [BoxGroup("Other")]
    public List<AnimatronicCell> animatronicList;
    [BoxGroup("Other")]
    public Transform challengessContainer;
    [BoxGroup("Other")]
    public List<Challenge> challengesList;

    float nightStartCooldown = 0.1f;

    bool EscBlocked = true;

    bool lockGlitch = false;

    Mesh mesh;
    Vector3[] verticies;
    Mesh originalMesh = null;

    void Start()
    {
        eventSys.SetSelectedGameObject(gameObject);
        foreach (Transform child in animatronicCellContainer)
        {
            if (child.TryGetComponent(out AnimatronicCell cell))
            {
                cell.MainMenu = this;
                animatronicList.Add(cell);
            }
        }
        foreach (Transform child in challengessContainer)
        {
            if (child.TryGetComponent(out Challenge chal))
            {
                chal.menu = this;
                challengesList.Add(chal);
            }
        }
        GM = GameManager.get();
        GM.soundManager.CreateLoopingSound("Menu Music", MenuMusic);
        GM.FadeToTransparent();

        offcieLayer officelayer = officesMenu.GetComponent<offcieLayer>();
        officelayer.SetOffice(GM.SaveData.SelectedOffcie);
        officelayer.officeSelected = (offices)GM.SaveData.SelectedOffcie;

        for (int i = 0; i < GM.AnimatronicsNightList.Count; i++)
        {
            for (int b = 0; b < animatronicList.Count; b++)
            {
                if (animatronicList[b].AnimatronicSettings.Name == GM.AnimatronicsNightList[i].Name)
                {
                    animatronicList[b].AnimatronicSettings.AILevel = GM.AnimatronicsNightList[i].AILevel;
                    animatronicList[b].AIText.text = animatronicList[b].AnimatronicSettings.AILevel.ToString();
                }
            }
        }

        if (GM.SelectedChallenge != "")
        {
            EnterChallengesMenuQuiet();
            for (int i = 0; i < challengesList.Count; i++)
            {
                if (challengesList[i].challengeName == GM.SelectedChallenge)
                {
                    challengesList[i].SetChallengeQuiet();
                }
            }
        }
        for (int i = 0; i < challengesList.Count; i++)
        {
            for (int b = 0; b < GM.SaveData.completedChallenges.Count; b++)
            {
                string c = GM.SaveData.completedChallenges[b];
                if (c == challengesList[i].challengeName)
                {
                    challengesList[i].Completed = true;
                }
                else if (c[0] == '{' && c[1] == 'S' && c[2] == 'M' && c[3] == '}' && c.Remove(0, 4) == challengesList[i].challengeName)
                {
                    challengesList[i].Completed = true;
                    challengesList[i].silent = true;
                }
            }
        }
        volumeSlider.value = GM.SaveData.GameVolume;
        GM.soundManager.changeVolume(GM.SaveData.GameVolume);
        volumeText.text = ((float)System.Math.Round(GM.SaveData.GameVolume + 80, 2)).ToString() + "%";

        showcharInfoToggle.isOn = GM.SaveData.showCharInfo;
        VisualEffectsToggle.isOn = GM.SaveData.VisualEffects;

        int Minutes5020 = 0;
        int Seconds5020 = 0;
        float TSeconds5020 = GM.SaveData.TenthSecsIn5020Mode;

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
            best5020Mode.text = Minutes5020.ToString() + ":0" + Seconds5020 + "." + TSeconds5020;
        }
        else
        {
            best5020Mode.text = Minutes5020.ToString() + ":" + Seconds5020 + "." + TSeconds5020;
        }

        if (GM.SaveData.batteriesCount == 0)
        {
            GM.BatteryActive = false;
            
        }
        if (GM.SaveData.coinsCount == 0)
        {
            GM.CoinsActive = false;
        }
        if (GM.SaveData.DDRepelsCount == 0)
        {
            GM.DDRepelActive = false;
        }
        if (GM.SaveData.frigidsCount == 0)
        {
            GM.FrigidActive = false;
        }
        foreach (Transform child in PowerUpsMenu.transform)
        {
            if (child.gameObject.name == "powersAlignment")
            {
                foreach (Transform ChildChild in child)
                {
                    if (ChildChild.TryGetComponent(out powerUp power))
                    {
                        power.sendUpdate();
                    }
                }
            }
        }

        if (GM.SaveData.HighScore >= 7000 && GM.SaveData.HighScore < 8000)
        {
            trophies.SetActive(true);
            trophies.GetComponent<SpriteRenderer>().sprite = brozeRank;
        }
        else if (GM.SaveData.HighScore >= 8000 && GM.SaveData.HighScore < 9000)
        {
            trophies.SetActive(true);
            trophies.GetComponent<SpriteRenderer>().sprite = silverRank;
        }
        else if (GM.SaveData.HighScore >= 9000 && GM.SaveData.HighScore < 10600)
        {
            trophies.SetActive(true);
            trophies.GetComponent<SpriteRenderer>().sprite = goldRank;
        }
        else if (GM.SaveData.HighScore >= 10600)
        {
            trophies.SetActive(true);
            trophies.GetComponent<SpriteRenderer>().sprite = diamondRank;
        }
        else
        {
            trophies.SetActive(false);
        }

        if (GM.silent)
            ToggleSilent(true);

        StartCoroutine(unblockEsc());
        IEnumerator unblockEsc()
        {
            yield return null;
            yield return null;
            yield return null;

            EscBlocked = false;
        }

        MainMenuPresence.State = GameManager.ReplaceKeyInString("50/20Time", MainMenuPresence.State, best5020Mode.text);
        MainMenuPresence.State = GameManager.ReplaceKeyInString("Score", MainMenuPresence.State, GM.SaveData.HighScore.ToString());

        DiscordController.get().SetPrecense(MainMenuPresence, true);

        

        /*
        the stuff for the chaos challenges:
          
        //1: 22, 1 - 5
        //2: 24, 5 - 10 - 20
        //3: 32, 5 - 10 - 20;

        List<int> chosenAnimatronics = new List<int>();

        while (chosenAnimatronics.Count < 32)
        {
            int selectedAnim = Random.Range(0, animatronicList.Count);
            bool doesCont = false;
            for (int i = 0; i < chosenAnimatronics.Count; i++)
            {
                if (chosenAnimatronics[i] == selectedAnim) doesCont = true;
            }

            if (!doesCont) 
            {
                chosenAnimatronics.Add(selectedAnim);
            }
        }

        for (int i = 0; i < chosenAnimatronics.Count; i++)
        {
            int randomDiff = Random.Range(0, 10);

            Animatronic meRn = animatronicList[chosenAnimatronics[i]].AnimatronicSettings;

            if (randomDiff >= 0 && randomDiff < 1)
            {
                meRn.AILevel = 5;
            }
            else if (randomDiff >= 1 && randomDiff < 3)
            {
                meRn.AILevel = 10;
            }
            else if (randomDiff >= 3 && randomDiff < 10)
            {
                meRn.AILevel = 20;
            }

            setSpecific(meRn, false);

            for (int b = 0; b < challengesList.Count; b++)
            {
                if (challengesList[b].challengeName == "Chaos 3")
                {
                    challengesList[b].Animatronics.Add(meRn);
                }
            }
        }
        */

    }

    public void toggleWindowed()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    private void Update()
    {
        int pointVal = 0;

        for (int i = 0; i < animatronicList.Count; i++)
        {
            pointVal += animatronicList[i].AnimatronicSettings.AILevel * 10;
        }

        pointValText.text = pointVal.ToString();

        HighScoreText.text = GM.SaveData.HighScore.ToString();

        if (CellOnOpacityGoUp)
        {
            CellOnOpacity += Time.deltaTime * CellOnOpacitySpeed;
        }
        else
        {
            CellOnOpacity -= Time.deltaTime * CellOnOpacitySpeed;
        }

        if (CellOnOpacity >= 255)
        {
            CellOnOpacityGoUp = false;
            CellOnOpacity = 255;
        }
        if (CellOnOpacity <= CellOnOpacityMinimum)
        {
            CellOnOpacityGoUp = true;
            CellOnOpacity = CellOnOpacityMinimum;
        }

        if (GM.FullyBlack && StartingNight)
        {
            SceneManager.LoadScene(GM.NightScene);
        }
        if (nightStartCooldown > 0)
        {
            nightStartCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !EscBlocked)
        {
            Application.Quit();
        }

        if (settinsMenu.activeSelf && Input.GetKey(KeyCode.Delete))
        {
            EraseDataTimer -= Time.deltaTime;
            if (EraseDataTimer <= 0)
            {
                ExitSettingsMenu();
                GM.SaveData.SelectedOffcie = 0;
                GM.SaveData.TenthSecsIn5020Mode = 0;
                GM.SaveData.completedChallenges.Clear();
                GM.SaveData.DDRepelsCount = 0;
                GM.SaveData.frigidsCount = 0;
                GM.SaveData.coinsCount = 0;
                GM.SaveData.batteriesCount = 0;
                GM.SaveData.HighScore = 0;

                best5020Mode.text = "0:00.0";

                for (int i = 0; i < challengesList.Count; i++)
                {
                    challengesList[i].Completed = false;
                }
                officesMenu.GetComponent<offcieLayer>().SetOffice(0);
                HighScoreText.text = GM.SaveData.HighScore.ToString();
                GM.BatteryActive = false;
                GM.CoinsActive = false;
                GM.DDRepelActive = false;
                GM.FrigidActive = false;
                if (GM.SaveData.HighScore >= 7000 && GM.SaveData.HighScore < 8000)
                {
                    trophies.SetActive(true);
                    trophies.GetComponent<SpriteRenderer>().sprite = brozeRank;
                }
                else if (GM.SaveData.HighScore >= 8000 && GM.SaveData.HighScore < 9000)
                {
                    trophies.SetActive(true);
                    trophies.GetComponent<SpriteRenderer>().sprite = silverRank;
                }
                else if (GM.SaveData.HighScore >= 9000 && GM.SaveData.HighScore < 10600)
                {
                    trophies.SetActive(true);
                    trophies.GetComponent<SpriteRenderer>().sprite = goldRank;
                }
                else if (GM.SaveData.HighScore >= 10600)
                {
                    trophies.SetActive(true);
                    trophies.GetComponent<SpriteRenderer>().sprite = diamondRank;
                }
                else
                {
                    trophies.SetActive(false);
                }
                GM.SaveGameData();
            }
        }
        else
        {
            EraseDataTimer = 2;
        }



        if (GM.silent)
        {
            pointValText.ForceMeshUpdate();
            mesh = pointValText.mesh;
            verticies = mesh.vertices;
            if (originalMesh == null)
                originalMesh = mesh;

            for (int i = 0; i < pointValText.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo c = pointValText.textInfo.characterInfo[i];

                int index = c.vertexIndex;

                Vector3 offset = wobble(Time.time * i * 20) * 3;

                verticies[index] += offset;
                verticies[index + 1] += offset;
                verticies[index + 2] += offset;
                verticies[index + 3] += offset;
            }

            mesh.vertices = verticies;
            pointValText.canvasRenderer.SetMesh(mesh);
        }
        else if (originalMesh != null)
        {
            pointValText.ForceMeshUpdate();
            pointValText.canvasRenderer.SetMesh(originalMesh);
            originalMesh = null;
        }
    }

    Vector2 wobble(float t)
    {
        return new Vector2(Mathf.Sin(t * 1.1f), Mathf.Cos(t * 0.8f));
    }

    public void addAll(int Level)
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        for (int i = 0; i < animatronicList.Count; i++)
        {
            animatronicList[i].AnimatronicSettings.AILevel += Level;
            if (animatronicList[i].AnimatronicSettings.AILevel > 20)
            {
                animatronicList[i].AnimatronicSettings.AILevel = 20;
            }
            animatronicList[i].AIText.text = animatronicList[i].AnimatronicSettings.AILevel.ToString();
        }
    }

    public void setAll(int Level)
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        for (int i = 0; i < animatronicList.Count; i++)
        {
            if (Level >= 0)
            {
                animatronicList[i].AnimatronicSettings.AILevel = Level;
            }
            else
            {
                animatronicList[i].AnimatronicSettings.AILevel = Random.Range(0, 20);
            }
            animatronicList[i].AIText.text = animatronicList[i].AnimatronicSettings.AILevel.ToString();
        }
    }

    public void setAllQuiet(int Level)
    {
        for (int i = 0; i < animatronicList.Count; i++)
        {
            if (Level >= 0)
            {
                animatronicList[i].AnimatronicSettings.AILevel = Level;
            }
            else
            {
                animatronicList[i].AnimatronicSettings.AILevel = Random.Range(0, 20);
            }
            animatronicList[i].AIText.text = animatronicList[i].AnimatronicSettings.AILevel.ToString();
        }
    }

    public void setSpecific(Animatronic animatronic, bool makesSound = true)
    {
        if (makesSound) GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        for (int i = 0; i < animatronicList.Count; i++)
        {
            if (animatronicList[i].AnimatronicSettings.Name == animatronic.Name)
            {
                if (animatronic.AILevel >= 0)
                {
                    animatronicList[i].AnimatronicSettings.AILevel = animatronic.AILevel;
                }
                else
                {
                    animatronicList[i].AnimatronicSettings.AILevel = Random.Range(0, 20);
                }
                animatronicList[i].AIText.text = animatronicList[i].AnimatronicSettings.AILevel.ToString();
            }
        }
    }

    public void EnterOfficeMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        SideMenu.SetActive(false);
        officesMenu.SetActive(true);
        allowedToShowCellUI = false;
    }

    public void ExitOfficeMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        SideMenu.SetActive(true);
        officesMenu.SetActive(false);
        allowedToShowCellUI = true;
    }

    public void EnterPowerUpsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        SideMenu.SetActive(false);
        PowerUpsMenu.SetActive(true);
        allowedToShowCellUI = false;
    }

    public void ExitPowerUpsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        SideMenu.SetActive(true);
        PowerUpsMenu.SetActive(false);
        allowedToShowCellUI = true;
    }

    public void EnterChallengesMenu()
    {
        SideMenu.SetActive(false);
        ChallengesMenu.SetActive(true);
        allowedToShowCellUI = false;
        setAll(0);
    }

    public void EnterChallengesMenuQuiet()
    {
        SideMenu.SetActive(false);
        ChallengesMenu.SetActive(true);
        allowedToShowCellUI = false;
        setAllQuiet(0);
    }

    public void ExitChallengesMenu()
    {
        SideMenu.SetActive(true);
        ChallengesMenu.SetActive(false);
        allowedToShowCellUI = true;
        GM.SelectedChallenge = "";
        setAll(0);

        for (int i = 0; i < challengesList.Count; i++)
        {
            challengesList[i].selected = false;
        }
    }

    public void volumeSliderChanged(float value)
    {
        GM.SaveData.GameVolume = value;
        GM.soundManager.changeVolume(value);
        volumeText.text = ((float)System.Math.Round(GM.SaveData.GameVolume + 80, 2)).ToString() + "%";
        GM.SaveGameData();
    }

    public void CharInfoToggle(bool b)
    {
        GM.SaveData.showCharInfo = b;
        GM.SaveGameData();
    }

    public void VEffectsToggle(bool b)
    {
        GM.SaveData.VisualEffects = b;
        GM.SaveGameData();
    }

    public void EnterSettingsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        settinsMenu.SetActive(true);
    }

    public void ExitSettingsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        settinsMenu.SetActive(false);
    }

    public void EnterCreditsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        CreditsMenu.SetActive(true);
    }

    public void ExitCreditsMenu()
    {
        GM.soundManager.CreateSoundEffect("minipip", GM.soundManager.GetSoundFromList("minipip"));
        CreditsMenu.SetActive(false);
    }

    public void StartNight()
    {
        if (!StartingNight && nightStartCooldown <= 0)
        {
            GM.AnimatronicsNightList.Clear();
            for (int i = 0; i < animatronicList.Count; i++)
            {
                GM.AnimatronicsNightList.Add(animatronicList[i].AnimatronicSettings);
            }
            GM.AnimatronicsNightList.Add(new Animatronic("Bass Cave Demon", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("KingSammelot", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("Samifying", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("Devil Vortex Saws", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("Spu7nix", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("GDColon", 0));
            GM.AnimatronicsNightList.Add(new Animatronic("Mirror Portal", 0));

            StartingNight = true;
            GM.FadeToBlack();
        }
    }

    public async void ToggleSilent(bool quite = false)
    {
        if (lockGlitch) return;
        lockGlitch = true;

        Vignette vignette = null;
        ChromaticAberration ca = null;

        for (int i = 0; i < gloablVol.profile.components.Count; i++)
        {
            if (gloablVol.profile.components[i] is Vignette)
                vignette = (Vignette)gloablVol.profile.components[i];

            if (gloablVol.profile.components[i] is ChromaticAberration)
                ca = (ChromaticAberration)gloablVol.profile.components[i];
        }

        if (quite)
        {
            for (int i = 0; i < animatronicList.Count; i++)
            {
                if (!GM.silent)
                {
                    animatronicList[i].outlineRenderer.color = Color.white;
                }
                else
                {
                    animatronicList[i].outlineRenderer.color = Color.red;
                }
            }

            if (!GM.silent)
            {
                vignette.intensity.value = 0;
                ca.intensity.value = 0;
                SoundManager.getSoundManager().getActiveSource("Menu Music").pitch = 1;

                var ss = silentButton.spriteState;
                ss.disabledSprite = silentButtonOff;
                ss.selectedSprite = silentButtonOff;
                ss.pressedSprite = silentButtonOffHover;
                ss.highlightedSprite = silentButtonOffHover;
                silentButton.spriteState = ss;

                pointValText.color = Color.white;
                pointValLabel.color = Color.white;

                silentButton.GetComponent<Image>().sprite = silentButtonOff;
            }
            else
            {
                vignette.intensity.value = 0.4f;
                ca.intensity.value = 0.1f;
                SoundManager.getSoundManager().getActiveSource("Menu Music").pitch = -1.05f;

                var ss = silentButton.spriteState;
                ss.disabledSprite = silentButtonOn;
                ss.selectedSprite = silentButtonOn;
                ss.pressedSprite = silentButtonOnHover;
                ss.highlightedSprite = silentButtonOnHover;
                silentButton.spriteState = ss;

                pointValText.color = new Color32(170, 0, 0, 255);
                pointValLabel.color = new Color32(170, 0, 0, 255);

                silentButton.GetComponent<Image>().sprite = silentButtonOn;
            }


            lockGlitch = false;
            return;
        }

        GM.silent = !GM.silent;

        AudioSource g = SoundManager.getSoundManager().CreateIdleSource("glitch", SoundManager.getSoundManager().GetSoundFromList("glitchSPT"));
        g.Play();
        g.time = Random.Range(0f, 5f);
        glitch.SetActive(true);

        List<AnimatronicCell> cells = GM.ShuffleList(animatronicList);

        int waitTIme = 300 / cells.Count;

        float waitTimeSeconds = waitTIme / 1000;

        shake.startShake(10, waitTimeSeconds);
        troShake.startShake(0.25f, waitTimeSeconds);

        float vignetteTime = 0.4f / cells.Count;

        float caTime = 0.1f / cells.Count;

        if (GM.silent)
        {
            SoundManager.getSoundManager().getActiveSource("Menu Music").pitch = -1.05f;
            var ss = silentButton.spriteState;
            ss.disabledSprite = silentButtonOn;
            ss.selectedSprite = silentButtonOn;
            ss.pressedSprite = silentButtonOnHover;
            ss.highlightedSprite = silentButtonOnHover;
            silentButton.spriteState = ss;

            silentButton.GetComponent<Image>().sprite = silentButtonOn;
        }

        for (int i = 0; i < cells.Count; i++)
        {
            if (!GM.silent)
                cells[i].outlineRenderer.color = Color.white;
            else
                cells[i].outlineRenderer.color = Color.red;

            if (vignette)
            {
                if (!GM.silent)
                    vignette.intensity.value -= vignetteTime;
                else
                    vignette.intensity.value += vignetteTime;
            }

            if (ca)
            {
                if (!GM.silent)
                    ca.intensity.value -= caTime;
                else
                    ca.intensity.value += caTime;
            }

            await Task.Delay(waitTIme);
        }

        shake.stopShake();
        troShake.stopShake();

        g.Stop();
        glitch.SetActive(false);
        SoundManager.getSoundManager().DeleteSource("glitch");
        if (!GM.silent)
        {
            SoundManager.getSoundManager().getActiveSource("Menu Music").pitch = 1;
            var ss = silentButton.spriteState;
            ss.disabledSprite = silentButtonOff;
            ss.selectedSprite = silentButtonOff;
            ss.pressedSprite = silentButtonOffHover;
            ss.highlightedSprite = silentButtonOffHover;
            silentButton.spriteState = ss;

            pointValText.color = Color.white;
            pointValLabel.color = Color.white;

            silentButton.GetComponent<Image>().sprite = silentButtonOff;
        }
        else
        {
            pointValText.color = new Color32(170, 0, 0, 255);
            pointValLabel.color = new Color32(170, 0, 0, 255);
        }
        await Task.Yield();
        lockGlitch = false;
    }
}
