using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class customShopkeeper : AnimatronicBase
{
    [ReadOnly] public AudioSource myMusic;
    public int currentBGMusic;
    public bool musicStopped;
    public float killTimer;

    bool clickSaftyLock;
    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        myMusic = GM.soundManager.CreateIdleSource("shoptheme", GM.soundManager.GetSoundFromList("shoptheme"));
        myMusic.loop = true;
        if (AILevel > 0)
        {
            myMusic.Play();
        }
        myMusic.volume = 0.4f;

        killTimer = 14.5f;
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing && AILevel > 0)
        {
            if (NM.CamSys.IsLookingAtCamera(cameraSystem.Cameras.Cam04))
            {
                myMusic.volume = 1;
            }
            else
            {
                myMusic.volume = 0.4f;
            }

            if (musicStopped)
            {
                killTimer -= Time.deltaTime;

                if (killTimer <= 0)
                {
                    Jumpscare();
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!musicStopped)
        {
            musicStopped = true;
            myMusic.Pause();
            killTimer = 14.5f;
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        myMusic.Stop();
        musicStopped = false;
        killTimer = 14.5f;
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        myMusic.Pause();
    }

    public virtual void OnChangeMusicClicked()
    {
        GM.soundManager.CreateSoundEffect("blip", GM.soundManager.GetSoundFromList("blip"));
        SwitchMusicBoxMuic();
        if (AILevel > 0)
        {
            if (musicStopped)
            {
                myMusic.UnPause();
                musicStopped = false;
                killTimer = 14.5f;
                clickSaftyLock = true;

                StartCoroutine(lockTimer());
                IEnumerator lockTimer()
                {
                    yield return new WaitForSeconds(1);
                    clickSaftyLock = false;
                }
            }
            else if (!clickSaftyLock)
            {
                Jumpscare();
            }
        }
    }

    public void SwitchMusicBoxMuic()
    {
        switch (currentBGMusic)
        {
            case 0:
                GM.soundManager.playSoundOnIdleSource("MusicBox", GM.soundManager.GetSoundFromList("musicBox1"));
                currentBGMusic = 1;
                break;
            case 1:
                GM.soundManager.playSoundOnIdleSource("MusicBox", GM.soundManager.GetSoundFromList("musicBox2"));
                currentBGMusic = 2;
                break;
            case 2:
                GM.soundManager.playSoundOnIdleSource("MusicBox", GM.soundManager.GetSoundFromList("musicBox3"));
                currentBGMusic = 3;
                break;
            case 3:
                GM.soundManager.playSoundOnIdleSource("MusicBox", GM.soundManager.GetSoundFromList("musicBox4"));
                currentBGMusic = 0;
                break;
        }
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);

        myMusic.Play();

        myMusic.volume = 0.4f;

        killTimer = 14.5f;
    }
}
