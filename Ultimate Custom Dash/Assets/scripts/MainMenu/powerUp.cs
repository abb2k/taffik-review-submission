using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static powerUp;

public class powerUp : MonoBehaviour
{
    public GameObject ActiveBox;
    public enum powerUps { firigd, coins, battery, DDRepel };
    public powerUps power;
    GameManager GM;
    public TextMeshProUGUI amountText;

    void Start()
    {
        GM = GameManager.get();
    }

    void Update()
    {
        if (GM == null)
        {
            GM = GameManager.get();
        }
        switch (power)
        {
            case powerUps.firigd:
                amountText.text = GM.SaveData.frigidsCount.ToString();
                ActiveBox.SetActive(GM.FrigidActive);
                if (GM.SaveData.frigidsCount == 0)
                {
                    GetComponent<Image>().color = new Color32(106, 106, 106, 255);
                }
                else
                {
                    GetComponent<Image>().color = Color.white;
                }
                break;
            case powerUps.coins:
                amountText.text = GM.SaveData.coinsCount.ToString();
                ActiveBox.SetActive(GM.CoinsActive);
                if (GM.SaveData.coinsCount == 0)
                {
                    GetComponent<Image>().color = new Color32(106, 106, 106, 255);
                }
                else
                {
                    GetComponent<Image>().color = Color.white;
                }
                break;
            case powerUps.battery:
                amountText.text = GM.SaveData.batteriesCount.ToString();
                ActiveBox.SetActive(GM.BatteryActive);
                if (GM.SaveData.batteriesCount == 0)
                {
                    GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                }
                else
                {
                    GetComponent<Image>().color = Color.white;
                }
                break;
            case powerUps.DDRepel:
                amountText.text = GM.SaveData.DDRepelsCount.ToString();
                ActiveBox.SetActive(GM.DDRepelActive);
                if (GM.SaveData.DDRepelsCount == 0)
                {
                    GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                }
                else
                {
                    GetComponent<Image>().color = Color.white;
                }
                break;
        }
    }

    public void sendUpdate()
    {
        Update();
    }

    public void TogglePowerUp(int power)
    {
        powerUps PowerUp = (powerUps)power;
        switch (PowerUp)
        {
            case powerUps.firigd:
                if (GM.SaveData.frigidsCount == 0)
                {
                    break;
                }
                GM.FrigidActive = !GM.FrigidActive;
                ActiveBox.SetActive(GM.FrigidActive);
                GM.soundManager.CreateSoundEffect("officeSelect", GM.soundManager.GetSoundFromList("officeSelect"));
                break;
            case powerUps.coins:
                if (GM.SaveData.coinsCount == 0)
                {
                    break;
                }
                GM.CoinsActive = !GM.CoinsActive;
                ActiveBox.SetActive(GM.CoinsActive);
                GM.soundManager.CreateSoundEffect("officeSelect", GM.soundManager.GetSoundFromList("officeSelect"));
                break;
            case powerUps.battery:
                if (GM.SaveData.batteriesCount == 0)
                {
                    break;
                }
                GM.BatteryActive = !GM.BatteryActive;
                ActiveBox.SetActive(GM.BatteryActive);
                GM.soundManager.CreateSoundEffect("officeSelect", GM.soundManager.GetSoundFromList("officeSelect"));
                break;
            case powerUps.DDRepel:
                if (GM.SaveData.DDRepelsCount == 0)
                {
                    break;
                }
                GM.DDRepelActive = !GM.DDRepelActive;
                ActiveBox.SetActive(GM.DDRepelActive);
                GM.soundManager.CreateSoundEffect("officeSelect", GM.soundManager.GetSoundFromList("officeSelect"));
                break;
        }
    }
}
