using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class teamCreditCell : MonoBehaviour
{
    public Image pfpImage;
    public TextMeshProUGUI nameText;
    [Space]
    public GameObject youtubeButton;
    public GameObject twitterButton;
    public GameObject twitchButton;
    [Space]
    public GameObject devTag;
    public GameObject betaTesterTag;
    public GameObject HelperTag;
    public GameObject GFXTag;
    [Space]
    public TextMeshProUGUI messageText;

    teamCredit mySettings;
    public void SetCell(teamCredit settings)
    {
        pfpImage.sprite = settings.pfp;
        nameText.text = settings.Name;

        //socials

        if (settings.Socials.Youtube == string.Empty)
            youtubeButton.SetActive(false);

        if (settings.Socials.Twitter == string.Empty)
            twitterButton.SetActive(false);

        if (settings.Socials.Twitch == string.Empty)
            twitchButton.SetActive(false);

        //tags

        if (!settings.Tags.Dev)
            devTag.SetActive(false);

        if (!settings.Tags.BetaTester)
            betaTesterTag.SetActive(false);

        if (!settings.Tags.Helper)
            HelperTag.SetActive(false);

        if (!settings.Tags.GFX)
            GFXTag.SetActive(false);

        messageText.text = settings.Message;

        mySettings = settings;
    }

    public void Youtube()
    {
        if (youtubeButton.activeSelf)
            Application.OpenURL(mySettings.Socials.Youtube);
    }

    public void Twitter()
    {
        if (twitterButton.activeSelf)
            Application.OpenURL(mySettings.Socials.Twitter);
    }

    public void Twitch()
    {
        if (twitchButton.activeSelf)
            Application.OpenURL(mySettings.Socials.Twitch);
    }
}
