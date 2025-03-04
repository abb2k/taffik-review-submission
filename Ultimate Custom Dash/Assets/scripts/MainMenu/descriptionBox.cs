using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class descriptionBox : MonoBehaviour
{
    public TextMeshProUGUI DescText;

    public void setDiscText(Animatronic animatronic, string descriptipn)
    {
        DescText.text = animatronic.Name + ": " + descriptipn;
    }
}
