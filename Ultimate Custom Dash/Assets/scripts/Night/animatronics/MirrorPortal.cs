using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorPortal : AnimatronicBase
{
    public bool corrupt;
    bool corruptSpawnLock;

    public Transform newChalCont;
    public Animator bluePAnim;
    public Animator orangePAnim;

    public List<AnimatronicBase> secrets;
    int currentCorruptSummon;

    Coroutine curruptSummoningSeq;
    bool playerDiedStopSummoninglolpls;

    //as corrupt, 33% every 10 seconds

    //as normal, every hour 10% chance

    //DDRepeal stops them

    //100% to be corrupt at 50/20, and a 0.01% chance to become corrupt in a normal night


    /* 8
     * 
     * 7
     * 
     * 7
     * 
     * 7
     * 
     * 7
     * 
     * 7
     */

    [SerializeField]float corruptSpawnTime;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new BoolValue(corrupt, "corrupt"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        if (NM.is5020Mode)
        {
            corrupt = true;
            OppretunityEvery = 10;
        }
        else if (Random.Range(0, 10000) == 1)
        {
            corrupt = true;
            OppretunityEvery = 10;
        }
        if (GM.DDRepelActive && !corrupt)
        {
            AILevel = 0;
        }
        else
        {
            AILevel = 100;
        }

        if (corrupt)
            corruptSpawnTime = Random.Range(7, 26);
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (corrupt && corruptSpawnTime > 0)
            corruptSpawnTime -= Time.deltaTime;

        if (corrupt && corruptSpawnTime <= 0 && !corruptSpawnLock)
        {
            //spawn corrupt

            orangePAnim.Play("mirrorPortalO");

            corruptSpawnLock = true;
            GM.soundManager.CreateSoundEffect("OrangeMirror", GM.soundManager.GetSoundFromList("OrangeMirror"));

            curruptSummoningSeq = StartCoroutine(spawning());
            IEnumerator spawning()
            {
                yield return new WaitForSeconds(1);
                while (currentCorruptSummon < 6)
                {
                    yield return new WaitForSeconds(7);
                    Instantiate(GM.newChal, newChalCont);

                    GM.soundManager.CreateSoundEffect("chalSummoned", GM.soundManager.GetSoundFromList("chalSummoned"));
                    int selectedAI = 10;

                    NM.NightPointsValue += selectedAI * 10;
                    if (GM.SelectedChallenge == "")
                    {
                        NM.NightPresence.Details = "In Night (Point Value: " + NM.NightPointsValue.ToString() + ")";
                        DiscordController.get().SetPrecense(NM.NightPresence, false);
                    }

                    secrets[currentCorruptSummon].OnMirrorSummon(selectedAI);

                    currentCorruptSummon++;
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        if (!corrupt && Random.Range(0, 10) == 1)
        {
            //spawn normal

            bluePAnim.SetBool("animPlaying", true);

            if (Random.Range(0, 2) == 1)
            {
                GM.soundManager.CreateSoundEffect("BlueMirror1", GM.soundManager.GetSoundFromList("BlueMirror1"));
            }
            else
            {
                GM.soundManager.CreateSoundEffect("BlueMirror2", GM.soundManager.GetSoundFromList("BlueMirror2"));
            }
        }
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        if (curruptSummoningSeq != null) StopCoroutine(curruptSummoningSeq);

        playerDiedStopSummoninglolpls = true;
    }

    //
    public virtual void AddACharacter()
    {
        if (playerDiedStopSummoninglolpls) return;
        int characterSelected = Random.Range(0, NM.Animatronics.Count - 2);

        for (int i = 0; i < NM.Animatronics.Count; i++)
        {
            if (i == characterSelected)
            {
                if (NM.Animatronics[i].AILevel == 0)
                {
                    Instantiate(GM.newChal, newChalCont);

                    GM.soundManager.CreateSoundEffect("chalSummoned", GM.soundManager.GetSoundFromList("chalSummoned"));
                    int selectedAI = Random.Range(0, 21);

                    NM.NightPointsValue += selectedAI * 10;
                    if (GM.SelectedChallenge == "")
                    {
                        NM.NightPresence.Details = "In Night (Point Value: " + NM.NightPointsValue.ToString() + ")";
                        DiscordController.get().SetPrecense(NM.NightPresence, false);
                        if (GM.silent)
                            NM.NightPresence.Details = NM.NightPresence.Details.Insert(0, "{Silent Mode} ");
                    }

                    NM.Animatronics[i].OnMirrorSummon(selectedAI);
                    for (int c = 0; c < GM.AnimatronicsNightList.Count; c++)
                    {
                        if (GM.AnimatronicsNightList[c].Name == NM.Animatronics[i].Name)
                        {
                            GM.AnimatronicsNightList[c].AILevel = selectedAI;
                        }
                    }
                }
                //else, none added
            }
        }
    }

    public override void SetCustomValue(BoolValue value)
    {
        if (value.keyName == "corrupt")
        {
            corrupt = value.value;
        }

        base.SetCustomValue(value);
    }

    virtual public void SummonDD()
    {
        if (!bluePAnim.GetBool("animPlaying") && curruptSummoningSeq == null)
        {
            bluePAnim.SetBool("animPlaying", true);

            if (Random.Range(0, 2) == 1)
            {
                GM.soundManager.CreateSoundEffect("BlueMirror1", GM.soundManager.GetSoundFromList("BlueMirror1"));
            }
            else
            {
                GM.soundManager.CreateSoundEffect("BlueMirror2", GM.soundManager.GetSoundFromList("BlueMirror2"));
            }
        }
    }
    virtual public void SummonSDD()
    {
        if (!corruptSpawnLock)
        {
            //spawn corrupt

            orangePAnim.Play("mirrorPortalO");

            corruptSpawnLock = true;
            GM.soundManager.CreateSoundEffect("OrangeMirror", GM.soundManager.GetSoundFromList("OrangeMirror"));

            curruptSummoningSeq = StartCoroutine(spawning());
            IEnumerator spawning()
            {
                yield return new WaitForSeconds(1);
                while (currentCorruptSummon < 6)
                {
                    yield return new WaitForSeconds(7);
                    Instantiate(GM.newChal, newChalCont);

                    GM.soundManager.CreateSoundEffect("chalSummoned", GM.soundManager.GetSoundFromList("chalSummoned"));
                    int selectedAI = 10;

                    NM.NightPointsValue += selectedAI * 10;
                    if (GM.SelectedChallenge == "")
                    {
                        NM.NightPresence.Details = "In Night (Point Value: " + NM.NightPointsValue.ToString() + ")";
                        DiscordController.get().SetPrecense(NM.NightPresence, false);
                        if (GM.silent)
                            NM.NightPresence.Details = NM.NightPresence.Details.Insert(0, "{Silent Mode} ");
                    }

                    secrets[currentCorruptSummon].OnMirrorSummon(selectedAI);

                    currentCorruptSummon++;
                }
            }
        }
    }
}
