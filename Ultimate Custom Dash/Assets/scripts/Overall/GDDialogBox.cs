using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GDDialogBox : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI nametext;
    public TextMeshProUGUI dialogtext;
    string alltext;
    int currStep;
    float TextappearTimer;
    bool destroyMe;
    float destroTime;

    public void CreateDialog(Sprite icon, string Name, string Text)
    {
        Icon.sprite = icon;
        nametext.text = Name;
        alltext = Text;
        dialogtext.text = "";
        transform.localScale = Vector3.zero;
        currStep = 0;
    }

    private void Update()
    {
        var scale = transform.localScale;
        scale.x = EasingFunction.EaseOutBounce(scale.x, 1, 0.2f);
        scale.y = EasingFunction.EaseOutBounce(scale.y, 1, 0.2f);
        transform.localScale = scale;

        if (TextappearTimer > 0)
        {
            if (currStep < alltext.Length)
            {
                dialogtext.text += alltext[currStep];
                currStep++;
            }
        }
        else
        {
            TextappearTimer = 0.075f;
        }

        if (destroyMe)
        {
            if (destroTime > 0)
            {
                destroTime -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void destroyMeTimer(float time)
    {
        destroyMe = true;
        destroTime = time;
    }
}
