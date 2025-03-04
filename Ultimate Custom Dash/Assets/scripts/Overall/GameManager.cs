using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    static GameManager Inctance;

    [BoxGroup("Settings")]
    public GameSaveData SaveData;
    [BoxGroup("Settings")]
    public bool FrigidActive;
    [BoxGroup("Settings")]
    public bool CoinsActive;
    [BoxGroup("Settings")]
    public bool BatteryActive;
    [BoxGroup("Settings")]
    public bool DDRepelActive;
    [BoxGroup("Settings")]
    public string SelectedChallenge;
    [BoxGroup("Settings")]
    public List<Animatronic> AnimatronicsNightList;
    [BoxGroup("Settings")]
    public bool silent;


    [BoxGroup("Prefab References")]
    public GameObject 
        emptyObject,
        descriptionBox,
        Firework,
        poof,
        newChal
    ;

    [BoxGroup("references")]
    public SoundManager soundManager;
    [BoxGroup("references")]
    public GameObject dontTitle;
    [BoxGroup("references")]
    public GameObject GlobalCanvas;
    [BoxGroup("References")]
    public string NightScene;
    [BoxGroup("References")]
    public string MenuScene;
    [BoxGroup("References")]
    public string Rewardcene;
    [BoxGroup("References")]
    public static NightManager CurrentNM;

    [BoxGroup("FadeEffects")]
    [SerializeField] float FadeTime;
    [BoxGroup("FadeEffects")]
    public Image blackScreen;
    [BoxGroup("FadeEffects")]
    [ReadOnly] public bool FullyBlack;
    [BoxGroup("FadeEffects")]
    [ReadOnly] public bool currentlyFadingToBlack;
    [BoxGroup("FadeEffects")]
    [ReadOnly] public bool currentlyFadingToTransparent;

    static float doubleClickTimer = 0;

    Coroutine fadeEffect;

    private void Awake()
    {
        DontDestroyOnLoad(dontTitle);
        DontDestroyOnLoad(GlobalCanvas);

        if (Inctance == null)
        {
            Inctance = this;
        }
        else
        {
            Destroy(dontTitle);
            Destroy(GlobalCanvas);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        dontTitle.transform.SetAsFirstSibling();
        GlobalCanvas.transform.SetAsLastSibling();
        blackScreen.gameObject.SetActive(true);

        createSaveFileIfDoesntExist();
        LoadGameData();
    }

    public static GameManager get() { return Inctance; }

    void Start()
    {
        soundManager = SoundManager.getSoundManager();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doubleClickTimer = 0.2f;
        }
        if (doubleClickTimer > 0)
        {
            doubleClickTimer -= Time.deltaTime;
        }
    }

    public void FadeToBlack()
    {
        currentlyFadingToBlack = true;
        if (fadeEffect == null)
        {
            fadeEffect = StartCoroutine(transition());
        }
        else {
            StopCoroutine(fadeEffect);
            fadeEffect = StartCoroutine(transition());
        }
            
        IEnumerator transition()
        {
            Color startColor = blackScreen.color;
            for (float t = 0f; t < FadeTime; t += Time.deltaTime)
            {
                float normalizedTime = t / FadeTime;
                //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
                blackScreen.color = Color.Lerp(startColor, Color.black, normalizedTime);

                yield return null;
            }
            blackScreen.color = Color.black; //without this, the value will end at something like 0.9992367
            currentlyFadingToBlack = false;
            FullyBlack = true;
        }
    }

    public void instaFadeBlack()
    {
        if (fadeEffect != null)
        {
            StopCoroutine(fadeEffect);
        }
        blackScreen.color = Color.black;
        FullyBlack = true;
    }

    public void instaFadeTransparent()
    {
        if (fadeEffect != null)
        {
            StopCoroutine(fadeEffect);
        }
        blackScreen.color = new Color(0, 0, 0, 0);
        FullyBlack = false;
    }

    public void FadeToTransparent()
    {
        FullyBlack = false;
        currentlyFadingToTransparent = true;
        if (fadeEffect == null)
        {
            fadeEffect = StartCoroutine(transition());
        }
        else
        {
            StopCoroutine(fadeEffect);
            fadeEffect = StartCoroutine(transition());
        }
        IEnumerator transition()
        {
            Color startColor = blackScreen.color;
            for (float t = 0f; t < FadeTime; t += Time.deltaTime)
            {
                float normalizedTime = t / FadeTime;
                //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
                blackScreen.color = Color.Lerp(startColor, new Color(0, 0, 0, 0), normalizedTime);

                yield return null;
            }
            blackScreen.color = new Color(0, 0, 0, 0); //without this, the value will end at something like 0.9992367
            currentlyFadingToTransparent = false;
        }
    }

    public void SetFadeSpeed(float speed)
    {
        if (!currentlyFadingToTransparent && !currentlyFadingToBlack)
        {
            FadeTime = speed;
        }
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(Application.persistentDataPath + "/UCDSaveFile.json", json);
    }

    public void LoadGameData()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/UCDSaveFile.json");
        SaveData = JsonUtility.FromJson<GameSaveData>(json);
    }

    public void createSaveFileIfDoesntExist()
    {
        if (!File.Exists(Application.persistentDataPath + "/UCDSaveFile.json"))
        {
            SaveGameData();
        }
    }

    public void SetCurrentNM(NightManager nm) { CurrentNM = nm; }

    public static bool DetectClickedOutside(GameObject panel, bool lookForIgnored)
    {
        if (CurrentNM != null)
            if (CurrentNM.CamsFullyOpened) return false;

        Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && panel.activeSelf &&
            panel.GetComponent<Collider2D>().OverlapPoint(pos))
        {
            RaycastHit2D[] ray = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0), 1000);

            float biggestZ = 0;
            for (int i = 0; i < ray.Length; i++)
            {
                if (lookForIgnored)
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() != null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
                else
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() == null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
            }

            if (biggestZ < panel.transform.position.z)
            {
                return false;
            }

            return true;
        }
        return false;
    }

    public static bool DetectClickedOutside(Collider2D panel, bool lookForIgnored)
    {
        if (CurrentNM != null)
            if (CurrentNM.CamsFullyOpened) return false;

        Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && panel.enabled &&
            panel.GetComponent<Collider2D>().OverlapPoint(pos))
        {
            RaycastHit2D[] ray = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0), 1000);

            float biggestZ = 0;
            for (int i = 0; i < ray.Length; i++)
            {
                if (lookForIgnored)
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() != null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
                else
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() == null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
            }

            if (biggestZ < panel.transform.position.z)
            {
                return false;
            }

            return true;
        }
        return false;
    }

    public static bool DetectHoverOutside(GameObject panel, bool lookForIgnored)
    {
        if (CurrentNM != null)
            if (CurrentNM.CamsFullyOpened) return false;

        Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (panel.activeSelf && panel.GetComponent<Collider2D>().OverlapPoint(pos))
        {
            RaycastHit2D[] ray = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0), 1000);

            float biggestZ = 0;
            for (int i = 0; i < ray.Length; i++)
            {
                if (lookForIgnored)
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() != null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
                else
                {
                    if (ray[i].transform.GetComponent<ignoreClickDetect>() == null)
                    {
                        if (biggestZ > ray[i].transform.position.z)
                        {
                            biggestZ = ray[i].transform.position.z;
                        }
                    }
                }
            }

            if (biggestZ < panel.transform.position.z)
            {
                return false;
            }

            return true;
        }
        return false;
    }


    public static bool DidDoubleClick()
    {
        if (doubleClickTimer > 0)
        {
            return true;
        }
        return false;
    }

    public static Color setColorAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Transform setComplexPos(Transform transform, complexPosition position, bool local = true)
    {
        if (local)
        {
            transform.localPosition = position.position;
            transform.localEulerAngles = position.rotation;
            transform.localScale = position.scale;
        }
        else
        {
            transform.position = position.position;
            transform.eulerAngles = position.rotation;
            transform.localScale = position.scale;
        }

        return transform;
    }

    public static complexPosition TransformToCompPos(Transform transform, bool local = true)
    {
        complexPosition pos;
        if (local)
        {
            pos = new complexPosition(transform.localPosition, transform.localEulerAngles, transform.localScale);
        }
        else
        {
            pos = new complexPosition(transform.position, transform.eulerAngles, transform.localScale);
        }

        return pos;
    }

    public static int BoolToInt(bool b) { if (b) return 1; else return 0; }

    public static string ReplaceKeyInString(string Key, string s, string Replacement)
    {
        string CompareKey = "";

        bool startedCheck = false;

        int StartIndex = 0;
        int EndIndex = 0;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '{')
            {
                StartIndex = i;
                startedCheck = true;
                for (int b = 1; b < Key.Length + 1; b++)
                {

                    if (s[i + b] == Key[b - 1])
                    {
                        CompareKey += s[i + b];

                        if (b == Key.Length)
                        {
                            EndIndex = i + b + 2;
                        }
                        
                    }
                    else
                    {
                        startedCheck = false;
                        break;
                    }
                }
            }

            if (s[i] == '}' && startedCheck)
            {
                break;
            }
        }

        if (CompareKey == Key)
        {
            s = s.Remove(StartIndex, EndIndex - StartIndex);
            s = s.Insert(StartIndex, Replacement);
        }


        return s;
    }

    public void addChallengeCompleted()
    {
        string challengeName = SelectedChallenge;
        bool _silent = silent;

        if (challengeName == "") return;

        bool compNormal = false;
        bool alrcompSilent = false;

        for (int i = 0; i < SaveData.completedChallenges.Count ; i++)
        {
            string c = SaveData.completedChallenges[i];
            if (c.Length >= 4)
                if (c[0] == '{' && c[1] == 'S' && c[2] == 'M' && c[3] == '}' && c.Remove(0, 4) == challengeName)
                {
                    alrcompSilent = true;
                }

            if (c == challengeName)
            {
                compNormal = true;
                break;
            }
        }

        if (!compNormal && !_silent && !alrcompSilent)
        {
            SaveData.completedChallenges.Add(challengeName);
        }

        if (_silent && !alrcompSilent)
        {
            if (SaveData.completedChallenges.Contains(challengeName))
                SaveData.completedChallenges.Remove(challengeName);
            SaveData.completedChallenges.Add(challengeName.Insert(0, "{SM}"));
        }
    }

    public List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }
}
