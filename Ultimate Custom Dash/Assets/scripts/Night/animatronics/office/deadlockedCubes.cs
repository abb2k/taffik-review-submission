using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deadlockedCubes : AnimatronicBase
{
    public Image[] cubes;
    public float sharedOpacity;
    public float fadeSpeed;
    public AudioClip music;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (sharedOpacity > 0)
        {
            sharedOpacity -= Time.deltaTime * fadeSpeed;

            if (sharedOpacity <= 0)
            {
                sharedOpacity = 0;
            }
            for (int i = 0; i < cubes.Length; i++)
            {
                cubes[i].color = GameManager.setColorAlpha(cubes[i].color, sharedOpacity);
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        SetCharedOpacity(1);
        GM.soundManager.CreateSoundEffect("deadlocked", music);
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();

    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].gameObject.SetActive(false);
        }
    }

    public void SetCharedOpacity(float opacity)
    {
        sharedOpacity = opacity;
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].color = GameManager.setColorAlpha(cubes[i].color, sharedOpacity);
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);
    }
}
