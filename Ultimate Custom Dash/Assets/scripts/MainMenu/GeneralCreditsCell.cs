using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GeneralCreditsCell : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public GeneralCreditsCellSettings mysettings;
    public Transform linksCont;
    public Sprite uiSprite;
    public Sprite youtube;
    public Sprite X;
    public Sprite twitch;
    public void SetCell(GeneralCreditsCellSettings settings)
    {
        nameText.text = settings.name;
        descText.text = settings.description;

        for (int i = 0; i < settings.links.Count; i++)
        {
            GameObject currLink = Instantiate(GameManager.get().emptyObject, linksCont);

            Image myImage = currLink.AddComponent<Image>();
            if (!settings.links[i].Icon)
            {
                if (settings.links[i].link.Contains("www.youtube.com"))
                {
                    myImage.sprite = youtube;
                }
                else if (settings.links[i].link.Contains("twitter.com"))
                {
                    myImage.sprite = X;
                }
                else if (settings.links[i].link.Contains("www.twitch.tv"))
                {
                    myImage.sprite = twitch;
                }
                else
                {
                    myImage.sprite = uiSprite;
                }
            }
            else
            {
                myImage.sprite = settings.links[i].Icon;
            }
            

            Button currLinkButton = currLink.AddComponent<Button>();
            string localLink = settings.links[i].link;
            currLinkButton.onClick.AddListener(delegate
            {
                SendToLink(localLink);
            });
        }
    }

    public void SendToLink(string link)
    {
        Application.OpenURL(link);
    }
}
