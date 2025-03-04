using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int currentWaveProgress = 0;
    private List<EnemyWave> waves = new List<EnemyWave>();

    public BuffCradSpawner cardSpawner;
    [SerializeField] private Transform cleanerObject;

    private GameValues currentValues;
    public GameValues CurrentValues 
    { 
        get { return currentValues; }
        private set { currentValues = value; } 
    }

    private MenuManager menuManager;

    private player bubble;

    private List<Bullet> bullets = new List<Bullet>();

    private float scoreMulti = 1;
    private float baseScore;
    private int stagesPassed;

    private void Start()
    {
        cardSpawner.onCardSelected += onCardSelected;
        menuManager = GameManager.get().getMenuManager();
        bubble = GameManager.get().Bubble;
    }

    public void progressWave()
    {
        if (currentWaveProgress == currentValues.waveCount)
        {
            //all waves complete, buff selections:

            for (int i = 0; i < waves.Count; i++)
            {
                Destroy(waves[i].gameObject);
            }

            waves.Clear();

            cardSpawner.CreateCards();
            stagesPassed++;
            menuManager.playStageCompleteAnim();
            bubble.isInUI = true;
            return;
        }

        waves[currentWaveProgress].startWave();
        waves[currentWaveProgress].onWaveComplete += progressWave;

        currentWaveProgress++;
    }

    //call on new wave bunch spawn
    public void startNewLevel()
    {
        waves.Clear();
        currentWaveProgress = 0;

        for (int i = 0; i < currentValues.waveCount; i++)
        {
            GameObject currWave = Instantiate(GameManager.get().wavePrefab);
            EnemyWave currWaveS = currWave.GetComponent<EnemyWave>();
            waves.Add(currWaveS);

            currWaveS.resetAndCreateBy(currentValues);
        }

        progressWave();
    }

    public void onCardSelected()
    {
        bubble.isInUI = false;
        startNewLevel();
    }

    public async void onReset()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            waves[i].DestroyMeAndClear();
        }

        cardSpawner.clearCards();

        ResetStats();
        bubble.revive();
        bubble.isInUI = false;

        CleanUpQueue();

        await Task.Delay(1000);

        startNewLevel();
    }

    public void addToCleanup(Transform other)
    {
        other.SetParent(cleanerObject);
    }

    public void CleanUpQueue()
    {
        cleanerObject.Cast<Transform>().ToList().ForEach(toClean => Object.Destroy(toClean.gameObject));
    }

    public void ResetStats()
    {
        currentValues = GameManager.get().defaultValues;
        stagesPassed = 0;
        baseScore = 0;
        scoreMulti = 1;
    }

    public void AddValues(GameValues values)
    {
        currentValues += values;
    }

    public float getBaseScore() { return baseScore; }

    public void addBaseScore(float score)
    {
        baseScore += score;
    }

    public float getScoreMultiplier() { return scoreMulti; }

    public void addScoreMultiplier(float multi)
    {
        scoreMulti += multi;
    }

    public int getStage() { return stagesPassed; }
}
