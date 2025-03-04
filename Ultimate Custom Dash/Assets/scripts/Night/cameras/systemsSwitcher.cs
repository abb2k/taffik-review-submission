using Discord;
using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NightManager;

public class systemsSwitcher : MonoBehaviour
{
    public NightManager nm;
    public RectTransform systemsBG;
    public RectTransform[] systemButtons;

    public void switchSys(int sys)
    {
        SoundManager.getSoundManager().CreateSoundEffect("blip", SoundManager.getSoundManager().GetSoundFromList("blip"));
        nm.CamSys.PulseStatic();
        for (int i = 0; i < systemButtons.Length; i++)
        {
            if (i == sys)
            {
                systemsBG.transform.position = systemButtons[i].transform.position;
            }
        }

        nm.OnAnyCamSysChange((NightManager.camSystems)sys);

        if ((int)nm.currSys != sys)
        {
            nm.currSys = (NightManager.camSystems)sys;
            nm.OnCamSysChanged((NightManager.camSystems)sys);
        }
    }
}
