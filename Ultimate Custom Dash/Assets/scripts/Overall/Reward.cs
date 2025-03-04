using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reward : MonoBehaviour
{
    public GameObject
        frigid,
        coins,
        battery,
        ddrepel
    ;
    public float escapeTimer;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.get().instaFadeTransparent();
        int selectedReward = Random.Range(0, 4);
        if (selectedReward == 4)
        {
            selectedReward = 3;
        }

        switch (selectedReward)
        {
            case 0:
                frigid.SetActive(true);
                if (GameManager.get().SaveData.frigidsCount < 9)
                {
                    GameManager.get().SaveData.frigidsCount++;
                }
                break;
            case 1:
                coins.SetActive(true);
                if (GameManager.get().SaveData.coinsCount < 9)
                {
                    GameManager.get().SaveData.coinsCount++;
                }
                break;
            case 2:
                battery.SetActive(true);
                if (GameManager.get().SaveData.batteriesCount < 9)
                {
                    GameManager.get().SaveData.batteriesCount++;
                }
                break;
            case 3:
                ddrepel.SetActive(true);
                if (GameManager.get().SaveData.DDRepelsCount < 9)
                {
                    GameManager.get().SaveData.DDRepelsCount++;
                }
                break;
        }
        GameManager.get().SaveGameData();
        SoundManager.getSoundManager().CreateSoundEffect("poweredUp", SoundManager.getSoundManager().GetSoundFromList("PowerUp"));
    }

    // Update is called once per frame
    void Update()
    {
        if (escapeTimer > 0)
        {
            escapeTimer -= Time.deltaTime;
        }
        else
        {
            GameManager.get().instaFadeBlack();
            SceneManager.LoadScene(GameManager.get().MenuScene);
        }
    }
}
