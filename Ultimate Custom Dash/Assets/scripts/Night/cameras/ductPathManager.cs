using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ductPathManager : MonoBehaviour
{
    public List<ductPoint> points = new List<ductPoint>();
    public RectTransform lure;
    public RectTransform opneIndic;
    public RectTransform closedIndic;
    public Image leftOpen;
    public Image rightOpen;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ductPoint point))
            {
                points.Add(point);
            }
        }
    }

    public void setAudioLure(ductPoint pointTO)
    {
        SoundManager.getSoundManager().CreateSoundEffect("blip", SoundManager.getSoundManager().GetSoundFromList("blip"));
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i] == pointTO)
            {
                points[i].HasAudioLure = true;
                lure.position = points[i].transform.position;
            }
            else
            {
                points[i].HasAudioLure = false;
            }
        }
    }

    public void openLeft()
    {
        NightManager.inctance.LeftDuctClosed = false;
        leftOpen.color = new Color32(255, 255, 255, 118);
        rightOpen.color = Color.white;
        opneIndic.transform.localPosition = new Vector3(-830.9f, -501.5f, 0); 
        closedIndic.transform.localPosition = new Vector3(-330, -517.7f, 0);
        SoundManager.getSoundManager().CreateSoundEffect("blip", SoundManager.getSoundManager().GetSoundFromList("blip"));
    }

    public void openRight()
    {
        NightManager.inctance.LeftDuctClosed = true;
        rightOpen.color = new Color32(255, 255, 255, 118);
        leftOpen.color = Color.white;
        opneIndic.transform.localPosition = new Vector3(-330.3f, -501.5f, 0);
        closedIndic.transform.localPosition = new Vector3(-830.59f, -517.7f, 0);
        SoundManager.getSoundManager().CreateSoundEffect("blip", SoundManager.getSoundManager().GetSoundFromList("blip"));
    }
}
