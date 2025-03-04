using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteScreen : MonoBehaviour
{
    GameManager GM;
    bool escaping;

    [BoxGroup("settings")]
    public bool newHighScore;
    [BoxGroup("settings")]
    public Image BG;
    bool fgbgSwitch;
    [BoxGroup("settings")]
    public Image FG;

    [BoxGroup("not new best")]
    public GameObject notNewBestCont;
    [BoxGroup("not new best")]
    public AudioClip boringWinSound;
    [BoxGroup("not new best")]
    public RectTransform notUdidit;
    [BoxGroup("not new best")]
    public TextMeshProUGUI notPointVal;
    [BoxGroup("not new best")]
    public TextMeshProUGUI notbestPointVal;

    [BoxGroup("new best")]
    public GameObject NewBestCont;
    [BoxGroup("new best")]
    public RectTransform Udidit;
    [BoxGroup("new best")]
    public AudioClip
        c10s,
        c20s,
        c30s,
        c40s,
        c60s
    ;
    [BoxGroup("new best")]
    public TextMeshProUGUI PointVal;
    [BoxGroup("new best")]
    public GameObject NewHighScore;
    [BoxGroup("new best")]
    public GameObject 
        greatJob,
        fantastic,
        amazing,
        stupendous,
        PERFECT,
        UNBEATABLE
    ;
    [BoxGroup("new best")]
    public RectTransform fireworkCont;
    [BoxGroup("new best")]
    public CanvasScaler cs;
    float CutsceneStartTimer = 3.9f;
    bool pointCountingSceneBegan;

    [SerializeField][ReadOnly]float PointsCounted;
    int PointsReceved;
    bool isCounting;

    float pulsingTimer = 0.175f;
    bool pulseVal;

    float blastTimer;

    float glitchTimer = 10;
    [BoxGroup("glitch")]
    public Vector2 glitchTimerRange;
    [BoxGroup("glitch")]
    public float glitchRange;
    [BoxGroup("glitch")]
    public float notGlitchSpeed;
    [BoxGroup("glitch")]
    public int notGlitchAddition;
    [BoxGroup("glitch")]
    public float glitchSpeed;
    [BoxGroup("glitch")]
    public int glitchAddition;
    [BoxGroup("glitch")]
    public Shake glitchShake;
    [BoxGroup("glitch")]
    public GameObject glitch;
    bool glitching;
    [SerializeField][ReadOnly] float glitchPoints;
    [SerializeField][ReadOnly] float preGlitchPoints;

    bool oneTimeGlitch;

    public void ActivateCompleteScreen(int Points, int TenthSecsSurTime, bool is5020, NightManager nm)
    {
        GM = GameManager.get();

        gameObject.SetActive(true);


        PointsReceved = Points;
        if (GM.silent)
        {
            PointsReceved *= 2;
        }

        blastTimer = Random.Range(0.05f, 0.75f);

        if (is5020)
        {
            if (GameManager.get().SaveData.TenthSecsIn5020Mode < TenthSecsSurTime)
            {
                GameManager.get().SaveData.TenthSecsIn5020Mode = TenthSecsSurTime;
                GameManager.get().SaveGameData();
            }
        }

        glitchTimer = Random.Range(glitchTimerRange.x, glitchTimerRange.y);

        if (PointsReceved > GM.SaveData.HighScore)
        {
            newHighScore = true;
            if (Points >= 0 && Points < 800)
            {
                GM.soundManager.CreateSoundEffect("winSound", boringWinSound, true);
            }
            if (Points >= 800 && Points < 1800)
            {
                GM.soundManager.CreateSoundEffect("winSound", c10s, true);
            }
            if (Points >= 1800 && Points < 3800)
            {
                GM.soundManager.CreateSoundEffect("winSound", c20s, true);
            }
            if (Points >= 3800 && Points < 6200)
            {
                GM.soundManager.CreateSoundEffect("winSound", c30s, true);
            }
            if (Points >= 6200 && Points < 8000)
            {
                GM.soundManager.CreateSoundEffect("winSound", c40s, true);
            }
            if (Points >= 8000)
            {
                GM.soundManager.CreateSoundEffect("winSound", c60s, true);
            }

            GM.SaveData.HighScore = PointsReceved;
            GM.SaveGameData();
        }
        else
        {
            newHighScore = false;
            GM.soundManager.CreateSoundEffect("winSound", boringWinSound, true);
            notPointVal.text = Points.ToString();
            PointsCounted = Points;
            glitchPoints = Points * 2;
            notbestPointVal.text = GM.SaveData.HighScore.ToString();
        }

        if (newHighScore)
        {
            nm.NightPresence.State = "NIGHT COMPLETE - NEW HIGH SCORE! - Points Received: " + nm.NightPointsValue;
        }
        else
        {
            nm.NightPresence.State = "NIGHT COMPLETE - Points Received: " + nm.NightPointsValue;
        }

        if (GM.silent)
            nm.NightPresence.State = nm.NightPresence.State.Insert(14, "{Silent Mode} ");

        DiscordController.get().SetPrecense(nm.NightPresence, true);
    }

    private void Update()
    {
        if (BG.color.a < 1)
        {
            BG.color += new Color(0,0,0, Time.deltaTime * 1.5f);
        }
        else
        {
            if (!fgbgSwitch)
            {
                fgbgSwitch = true;
                FG.color = Color.black;
                notNewBestCont.SetActive(!newHighScore);
                NewBestCont.SetActive(newHighScore);
            }
            if (FG.color.a > 0)
            {
                FG.color -= new Color(0, 0, 0, Time.deltaTime );
            }

            if (newHighScore && GM.silent)
                if (glitchTimer > 0)
                {
                    glitchTimer -= Time.deltaTime;
                }
                else
                {
                    glitchTimer = 0;
                    runGlitch(false);
                }

            if (newHighScore)
            {
                NewUpdate();
            }
            else
            {
                NotNewUpdate();
            }
        }
    }

    void NotNewUpdate()
    {
        if (notUdidit.localPosition.y < 0)
        {
            var pos = notUdidit.localPosition;
            pos.y += Time.deltaTime * 120;

            if (pos.y >= 0)
            {
                pos.y = 0;

                if (GM.silent)
                    if (!oneTimeGlitch)
                    {
                        oneTimeGlitch = true;
                        runGlitch(true);
                    }
            }

            notUdidit.localPosition = pos;
        }

        if (notUdidit.localPosition.y == 0 && Input.anyKey && !escaping)
        {
            escaping = true;
            GameManager.get().FadeToBlack();
        }

        if (escaping && GameManager.get().FullyBlack)
        {
            int random = 0;
            if (PointsReceved >= 500)
            {
                random = Random.Range(0, 2);
            }
            if (random == 0)
            {
                SceneManager.LoadScene(GameManager.get().MenuScene);
            }
            else
            {
                SceneManager.LoadScene(GameManager.get().Rewardcene);
            }
        }
    }

    void NewUpdate()
    {
        if (Udidit.localPosition.y < 100)
        {
            var pos = Udidit.localPosition;
            pos.y += Time.deltaTime * 80;

            if (pos.y >= 100)
            {
                pos.y = 100;
            }

            Udidit.localPosition = pos;
        }

        if (pointCountingSceneBegan)
        {
            if (!isCounting)
            {
                PointsCounted += Time.deltaTime * 20;
                preGlitchPoints += Time.deltaTime * 20;

                if (!GM.silent)
                {
                    if (((int)PointsCounted * 10) > PointsReceved)
                    {
                        PointsCounted = PointsReceved / 10;
                        isCounting = true;
                        NewHighScore.SetActive(true);
                    }
                }
                else
                {
                    if (((int)PointsCounted * 10) > PointsReceved / 2)
                    {
                        PointsCounted = PointsReceved / 2 / 10;
                        isCounting = true;
                        NewHighScore.SetActive(true);
                        if (!oneTimeGlitch)
                        {
                            oneTimeGlitch = true;
                            runGlitch(false);
                        }

                    }
                }

                if (((int)PointsCounted) >= 0 && ((int)PointsCounted) < 200)
                {
                    //greatjob
                }
                else if (((int)PointsCounted) >= 200 && ((int)PointsCounted) < 400)
                {
                    //fantastic
                    greatJob.SetActive(false);
                    fantastic.SetActive(true);
                }
                else if (((int)PointsCounted) >= 400 && ((int)PointsCounted) < 600)
                {
                    //amazing
                    greatJob.SetActive(false);
                    fantastic.SetActive(false);
                    amazing.SetActive(true);
                }
                else if (((int)PointsCounted) >= 600 && ((int)PointsCounted) < 800)
                {
                    //stupendous
                    greatJob.SetActive(false);
                    fantastic.SetActive(false);
                    amazing.SetActive(false);
                    stupendous.SetActive(true);
                }
                else if (((int)PointsCounted) >= 800 && ((int)PointsCounted) < 1000)
                {
                    //perfect
                    greatJob.SetActive(false);
                    fantastic.SetActive(false);
                    amazing.SetActive(false);
                    stupendous.SetActive(false);
                    PERFECT.SetActive(true);
                }
                else
                {
                    //unbeatable
                    greatJob.SetActive(false);
                    fantastic.SetActive(false);
                    amazing.SetActive(false);
                    stupendous.SetActive(false);
                    PERFECT.SetActive(false);
                    UNBEATABLE.SetActive(true);
                }

                PointVal.text = ((int)preGlitchPoints).ToString() + "0";

                if (!glitching)
                    glitchPoints = ((int)PointsCounted) * 2;
            }

            if (pulsingTimer > 0)
            {
                pulsingTimer -= Time.deltaTime;

                if (pulsingTimer <= 0)
                {
                    pulsingTimer = 0.175f;
                    if (!isCounting)
                    {
                        NewHighScore.SetActive(!NewHighScore.activeSelf);
                    }
                    else
                    {
                        if (!pulseVal)
                        {
                            pulseVal = true;
                        }
                        else
                        {
                            pulseVal = false;
                            PointVal.gameObject.SetActive(!PointVal.gameObject.activeSelf);
                        }
                    }
                }
            }

            if (!isCounting)
            {
                if (blastTimer > 0)
                {
                    blastTimer -= Time.deltaTime;

                    if (blastTimer <= 0)
                    {
                        blastTimer = Random.Range(0.05f, 0.75f);
                        GameObject firework = Instantiate(GM.Firework, fireworkCont);
                        firework.transform.localPosition = new Vector3(Random.Range(-1710.0f, 1710.0f), Random.Range(-975.0f, 975.0f), 0);
                        firework.GetComponent<FireworkBlast>().RunBlast(cs);
                    }
                }
            }

            if (isCounting && Input.anyKey && !escaping)
            {
                escaping = true;
                GameManager.get().FadeToBlack();
            }

            if (escaping && GameManager.get().FullyBlack)
            {
                int random = 0;
                if (PointsReceved >= 500)
                {
                    random = Random.Range(0, 2);
                }
                if (random == 0)
                {
                    SceneManager.LoadScene(GameManager.get().MenuScene);
                }
                else
                {
                    SceneManager.LoadScene(GameManager.get().Rewardcene);
                }
            }
        }


        if (CutsceneStartTimer > 0)
        {
            CutsceneStartTimer -= Time.deltaTime;

            if (CutsceneStartTimer <= 0)
            {
                CutsceneStartTimer = 0;
                pointCountingSceneBegan = true;
                Udidit.gameObject.SetActive(false);
                NewHighScore.SetActive(true);
                greatJob.SetActive(true);
                PointVal.gameObject.SetActive(true);
            }
        }
    }

    public async void runGlitch(bool not)
    {
        if (glitching) return;
        glitching = true;

        AudioSource g = SoundManager.getSoundManager().CreateLoopingSound("glitch", SoundManager.getSoundManager().GetSoundFromList("glitchSPT"));
        g.time = Random.Range(0f, 5f);
        glitch.SetActive(true);

        //AudioSource music = GM.soundManager.getActiveSource("winSound");

        //music.pitch = -1.05f;
        BG.color = new Color32(133, 0, 0, 255);

        int gsMillisecond = 0;

        if (!not)
        {
            glitchShake.startShake(glitchRange, glitchSpeed);
            gsMillisecond = (int)(glitchSpeed * 1000);
        }
        else
        {
            glitchShake.startShake(glitchRange, notGlitchSpeed);
            gsMillisecond = (int)(notGlitchSpeed * 1000);
        }

        if (not)
            while (PointsCounted < glitchPoints)
            {
                await Task.Delay(gsMillisecond);

                PointsCounted += notGlitchAddition;
                notPointVal.text = ((int)PointsCounted).ToString();
            }
        else
            while (preGlitchPoints < glitchPoints)
            {
                await Task.Delay(gsMillisecond);

                preGlitchPoints += glitchAddition;
                PointVal.text = ((int)preGlitchPoints).ToString() + "0";
            }
        if (not)
        {
            PointsCounted = glitchPoints;
            notPointVal.text = ((int)PointsCounted).ToString();
        }
        else
        {
            preGlitchPoints = glitchPoints;

            if (isCounting)
            {
                preGlitchPoints = PointsCounted * 2;
                glitchPoints = PointsCounted * 2;
            }

            PointVal.text = ((int)preGlitchPoints).ToString() + "0";
        }

        glitchShake.stopShake();

        g.Stop();
        glitch.SetActive(false);
        SoundManager.getSoundManager().DeleteSource("glitch");

        //if (music)
        //    music.pitch = 1;
        BG.color = Color.black;

        glitchTimer = Random.Range(glitchTimerRange.x, glitchTimerRange.y);

        await Task.Yield();
        glitching = false;
    }
}
