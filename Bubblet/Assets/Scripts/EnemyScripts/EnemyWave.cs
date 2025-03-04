using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] private WaveSet waveSets;

    public event UnityAction onWaveComplete;

    public void startWave()
    {
        Progress();
    }

    IEnumerator nextSetTime(WaveSet set, UnityAction callback)
    {
        yield return new WaitForSeconds(set.newSetTime);
        set.didSetTimeEnd = true;
        callback.Invoke();
    }

    public void Progress()
    {
        SpawnSet(waveSets);
    }

    public void DestroyMeAndClear()
    {
        for (int i = 0; i < waveSets.livingEnemies.Count; i++)
        {
            Destroy(waveSets.livingEnemies[i].gameObject);
        }

        Destroy(gameObject);
    }

    public void onSetComplete(WaveSet setCompleted)
    {
        onWaveComplete.Invoke();
        return;
    }

    public void SpawnSet(WaveSet set)
    {
        //whenever spawning subscribe to OnDeath with this sets "SetEnemyDied"

        set.onSetEnded += onSetComplete;

        for (int i = 0; i < set.enemies.Count; i++)
        {
            GameObject spawnedEnemy = Instantiate(GameManager.get().getPrefabBasedOnEnum(set.enemies[i].type));
            EnemyBase myEnemy = spawnedEnemy.GetComponent<EnemyBase>();
            myEnemy.onDeath += set.OnEnemyDied;
            set.livingEnemies.Add(myEnemy);

            //positioning

            int side = Random.Range(0, 2) == 0 ? -1 : 1;

            if (Random.Range(0, 2) == 0)
            {
                //on top / bottom

                float xPos = Random.Range(-8f, 8f);

                spawnedEnemy.transform.position = new Vector3(xPos, 6 * side, 0);

                Vector3 to = spawnedEnemy.transform.position + Vector3.down * 2.5f * side;

                StartCoroutine(myEnemy.transitionIntoArena(to, .5f, set.onEnemySpanwed));
            }
            else
            {
                //on right / left

                float yPos = Random.Range(-4f, 4f);

                spawnedEnemy.transform.position = new Vector3(10 * side, yPos, 0);

                Vector3 to = spawnedEnemy.transform.position + Vector3.left * 2.5f * side;

                StartCoroutine(myEnemy.transitionIntoArena(to, .5f, set.onEnemySpanwed));

            }
        }
    }


    public int getLivingEnemiesAmount()
    {
        int toReturn = waveSets.livingEnemies.Count;

        return toReturn;
    }

    public void resetAndCreateBy(GameValues gameValues)
    {
        WaveSet currSet = new WaveSet();

        for (int e = 0; e < gameValues.randomsCount; e++)
            currSet.addEnemy(new WaveEnemy(Random.Range(0, System.Enum.GetValues(typeof(Enemies)).Length)));

        for (int e = 0; e < gameValues.turrentsCount; e++)
            currSet.addEnemy(new WaveEnemy(Enemies.Turret));

        for (int e = 0; e < gameValues.treesCount; e++)
            currSet.addEnemy(new WaveEnemy(Enemies.Tree));

        for (int e = 0; e < gameValues.cactiCount; e++)
            currSet.addEnemy(new WaveEnemy(Enemies.Cactus));

        waveSets = currSet;
    }
}


[System.Serializable]
public class WaveSet
{
    public List<WaveEnemy> enemies = new List<WaveEnemy>();
    public float newSetTime = 20;
    public event UnityAction<WaveSet> onSetEnded;
    public bool didSetTimeEnd;
    public Coroutine nextSetCoroutine;

    public List<EnemyBase> livingEnemies = new List<EnemyBase>();

    public void addEnemy(WaveEnemy enemy)
    {
        enemies.Add(enemy);
    }

    public void OnEnemyDied(EnemyBase enemy)
    {
        if (livingEnemies.Contains(enemy))
            livingEnemies.Remove(enemy);

        if (livingEnemies.Count == 0)
        {
            //set completed!
            onSetEnded.Invoke(this);
        }
    }

    public void onEnemySpanwed(EnemyBase enemy)
    {
        enemy.startShootAndLifeTimer();
    }
}

[System.Serializable]
public class WaveEnemy
{
    public Enemies type;

    public WaveEnemy(Enemies enemyType)
    {
        this.type = enemyType;
    }

    public WaveEnemy(int enemyType)
    {
        this.type = (Enemies)enemyType;
    }
}

public enum Enemies
{
    Tree,
    Turret,
    Cactus
}

