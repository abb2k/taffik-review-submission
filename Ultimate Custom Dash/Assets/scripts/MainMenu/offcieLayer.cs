using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static offcieLayer;

public class offcieLayer : MonoBehaviour
{
    public enum offices { Default, SL, Fnaf3, Fnaf4 };
    public offices officeSelected;
    [HideInInspector]
    public bool dontplaySound = true;
    GameManager GM;

    public Transform selectedBox;
    public Transform[] offceButtons;
    public GameObject[] locks;

    void Start()
    {
        GM = GameManager.get();
    }

    void Update()
    {
        selectedBox.position = offceButtons[(int)officeSelected].position;

        if (GM.SaveData.HighScore >= 2000)
        {
            offceButtons[1].GetComponent<Image>().color = Color.white;
            locks[0].SetActive(false);
        }
        else
        {
            offceButtons[1].GetComponent<Image>().color = new Color32(106, 106, 106, 255);
            locks[0].SetActive(true);

            if ((int)officeSelected >= 1)
            {
                SetOffice(0);
            }
        }
        if (GM.SaveData.HighScore >= 5000)
        {
            offceButtons[2].GetComponent<Image>().color = Color.white;
            locks[1].SetActive(false);
        }
        else
        {
            offceButtons[2].GetComponent<Image>().color = new Color32(106, 106, 106, 255);
            locks[1].SetActive(true);

            if ((int)officeSelected >= 2)
            {
                SetOffice(0);
            }
        }
        if (GM.SaveData.HighScore >= 8000)
        {
            offceButtons[3].GetComponent<Image>().color = Color.white;
            locks[2].SetActive(false);
        }
        else
        {
            offceButtons[3].GetComponent<Image>().color = new Color32(106, 106, 106, 255);
            locks[2].SetActive(true);

            if ((int)officeSelected >= 3)
            {
                SetOffice(0);
            }
        }
    }

    public void SetOffice(int office)
    {
        GM = GameManager.get();
        bool canSet = false;

        if (office == 1 && GM.SaveData.HighScore >= 2000)
        {
            canSet = true;
        }

        if (office == 2 && GM.SaveData.HighScore >= 5000)
        {
            canSet = true;
        }

        if (office == 3 && GM.SaveData.HighScore >= 8000)
        {
            canSet = true;
        }

        if (office == 0)
        {
            canSet = true;
        }

        if (canSet)
        {
            if (!dontplaySound)
            {
                GM = GameManager.get();
                GM.soundManager.CreateSoundEffect("officeSelect", GM.soundManager.GetSoundFromList("officeSelect"));
                GM.SaveData.SelectedOffcie = office;
            }
            officeSelected = (offices)office;

            if (!dontplaySound)
            {
                GM.SaveGameData();
            }
            else
            {
                dontplaySound = false;
            }
        }
        
    }
}
