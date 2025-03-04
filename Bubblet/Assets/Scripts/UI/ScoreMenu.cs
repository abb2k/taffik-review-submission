using System.Collections;
using TMPro;
using UnityEngine;


public class ScoreMenu : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private float TotalScore;
    [Header("ScoreText")]
    [SerializeField] private TextMeshProUGUI BaseScoreText;
    [SerializeField] private TextMeshProUGUI ScoreMultText;
    [SerializeField] private TextMeshProUGUI TotalScoreText;
    [SerializeField] private TextMeshProUGUI Stage;
    [SerializeField] private AudioClip scoreUp;

    [SerializeField] private float time = 2;
    [SerializeField] private float scoreUpSoundEvery = 0.1f;

    private bool finishedCounting;    

    public void startCounting()
    {
        LevelManager levelManager = GameManager.get().getLevelManager();

        StartCoroutine(countUpScore(time, levelManager.getBaseScore(), BaseScoreText));
        StartCoroutine(countUpScore(time, levelManager.getScoreMultiplier(), ScoreMultText));
        StartCoroutine(MakeScoreSounds(time));
        CalculateScore();
        StartCoroutine(waitAndPopTotal(time, TotalScore, TotalScoreText));

        Stage.text = (levelManager.getStage() + 1).ToString();
    }

    private void CalculateScore()
    {
        TotalScore = GameManager.get().getLevelManager().getBaseScore() * GameManager.get().getLevelManager().getScoreMultiplier();
    }
    private void Update()
    {
        if (GameManager.GetInputDown(GameManager.get().Bubble.selectButtonKey) && finishedCounting)
        {
            GameManager.get().getLevelManager().onReset();
            finishedCounting = false;
        }
    }

    IEnumerator countUpScore(float countTime, float CountTo, TextMeshProUGUI txt)
    {
        float startingScore = 0;

        for (float t = 0; t < countTime; t += Time.deltaTime)
        {
            float fixedNum = Mathf.Lerp(startingScore, CountTo, t / countTime);
            txt.text = string.Format("{0:0.##}", fixedNum);

            int a = (int)(t / scoreUpSoundEvery);

            yield return null;
        }
    }

    IEnumerator MakeScoreSounds(float countTime)
    {
        int prevBoop = -1;

        for (float t = 0; t < countTime; t += Time.deltaTime)
        {
            int interval = (int)(t / scoreUpSoundEvery);

            if (prevBoop != interval)
            {
                prevBoop = interval;
                SoundManager.getSoundManager().CreateSoundEffect("scoreUp", scoreUp);
            }

            yield return null;
        }
    }

    IEnumerator waitAndPopTotal(float countTime, float score, TextMeshProUGUI txt)
    {
        txt.text = "0";
        yield return new WaitForSeconds(countTime);

        txt.text = ((int)score).ToString();

        finishedCounting = true;
    }
}
