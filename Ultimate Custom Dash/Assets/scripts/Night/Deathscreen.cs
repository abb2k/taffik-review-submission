using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Deathscreen : MonoBehaviour
{
    public TextMeshProUGUI TimeSurvived;
    public Image RPulse;
    public Image RVignette;
    public float RednessOpacity;
    public float RednessOpacitySpeed;
    bool escaping;
    int PointsReceved;
    AudioClip deathVoiceline;
    public bool storytime;

    bool funnyConglol;
    bool oneTimeActClip;

    public VideoPlayer congVid;
    public List<VideoClip> congClips;
    public VideoClip storytimeClip;
    public GameObject[] disableOnCong;

    public void ActivateDeathscreen(string SurvivedTime, int TenthSecsSurTime, bool Is5020, int roundPointValue, AudioClip _deathVoiceline)
    {
        gameObject.SetActive(true);
        TimeSurvived.text = SurvivedTime;

        PointsReceved = roundPointValue;

        if (Is5020)
        {
            if (GameManager.get().SaveData.TenthSecsIn5020Mode < TenthSecsSurTime)
            {
                GameManager.get().SaveData.TenthSecsIn5020Mode = TenthSecsSurTime;
                GameManager.get().SaveGameData();
            }
        }
        deathVoiceline = _deathVoiceline;

        funnyConglol = Random.Range(0, 1000) == 0;

        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = congVid.targetTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;

        if (funnyConglol || storytime)
            for (int i = 0; i < disableOnCong.Length; i++)
            {
                disableOnCong[i].SetActive(false);
            }
        if (storytime)
            congVid.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (RednessOpacity > 0)
        {
            RednessOpacity -= Time.deltaTime * RednessOpacitySpeed;

            if (RednessOpacity <= 0)
            {
                RednessOpacity = 0;
                if (!funnyConglol && deathVoiceline)
                {
                    SoundManager.getSoundManager().CreateSoundEffect("deathLine", deathVoiceline, true);

                    if (storytime)
                    {
                        congVid.clip = storytimeClip;
                        congVid.Play();
                        congVid.isLooping = true;

                        StartCoroutine(waitForEnd());
                        IEnumerator waitForEnd()
                        {
                            yield return new WaitForSeconds(deathVoiceline.length);
                            storytime = false;
                            escaping = true;
                            GameManager.get().FadeToBlack();
                        }
                    }
                }
            }
        }
        if (funnyConglol && RednessOpacity == 0 && !oneTimeActClip)
        {
            oneTimeActClip = true;
            StartCoroutine(waitForAllowEscape());
            IEnumerator waitForAllowEscape()
            {
                yield return new WaitForSeconds(4);
                congVid.gameObject.SetActive(true);
                congVid.Stop();
                int selectedClip = Random.Range(0, congClips.Count);
                congVid.clip = congClips[selectedClip];
                congVid.Play();
                yield return new WaitForSeconds(5);
                funnyConglol = false;
            }
        }
        
        RPulse.color = new Color32(255, 255, 255, (byte)RednessOpacity);
        RVignette.color = new Color32(255, 255, 255, (byte)(RednessOpacity / 2));
        if (RednessOpacity == 0 && Input.anyKey && !escaping && !funnyConglol && !storytime)
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
}
