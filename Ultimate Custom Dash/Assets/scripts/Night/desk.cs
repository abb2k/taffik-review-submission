using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static offcieLayer;

public class desk : MonoBehaviour
{
    public bool FanOn;
    public Animator anim;
    public GameObject Nose;

    private void Start()
    {
        GameManager GM = GameManager.get();
        if (GM)
        {
            setType(GM.SaveData.SelectedOffcie);
        }
        
    }

    public void TurnFan() 
    {
        SoundManager.getSoundManager().CreateSoundEffect("fanClicking", SoundManager.getSoundManager().GetSoundFromList("click"));
        FanOn = !FanOn;
        if (FanOn)
        {
            //on
            anim.speed = 1;
            SoundManager.getSoundManager().getActiveSource("FanSound").UnPause();
        }
        else
        {
            //off
            anim.speed = 0;
            SoundManager.getSoundManager().getActiveSource("FanSound").Pause();
        }
    }

    public void setType(int type)
    {
        anim.SetInteger("type", type);
        if (type != 0)
        {
            Nose.SetActive(false);
        }
    }
}
