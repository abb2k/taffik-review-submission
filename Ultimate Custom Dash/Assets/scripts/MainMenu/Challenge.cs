using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    public string challengeName;
    public List<Animatronic> Animatronics;
    public TextMeshProUGUI challengeText;
    [HideInInspector]
    public mainMenu menu;
    public bool selected;
    public Sprite Selected;
    public Sprite DeSelected;
    public GameObject star;
    public bool Completed;
    public bool silent;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            image.sprite = Selected;
        }
        else
        {
            image.sprite = DeSelected;
        }
        star.SetActive(Completed);
        if (silent)
            star.GetComponent<Image>().color = Color.red;
    }

    public void SetChallenge()
    {
        menu.setAll(0);
        selected = true;
        GameManager.get().SelectedChallenge = challengeName;
        for (int i = 0; i < Animatronics.Count; i++)
        {
            menu.setSpecific(Animatronics[i], false);
        }
        for (int i = 0; i < menu.challengesList.Count; i++)
        {
            if (menu.challengesList[i] != this)
            {
                menu.challengesList[i].selected = false;
            }
        }
    }

    public void SetChallengeQuiet()
    {
        menu.setAllQuiet(0);
        selected = true;
        GameManager.get().SelectedChallenge = challengeName;
        for (int i = 0; i < Animatronics.Count; i++)
        {
            menu.setSpecific(Animatronics[i], false);
        }
        for (int i = 0; i < menu.challengesList.Count; i++)
        {
            if (menu.challengesList[i] != this)
            {
                menu.challengesList[i].selected = false;
            }
        }
    }
}
