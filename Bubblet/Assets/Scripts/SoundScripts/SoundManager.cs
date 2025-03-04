using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    static SoundManager Inctance;

    public AudioMixer mainMixer;
    public AudioMixerGroup mainGroup;

    public AllSound allSounds;

    bool focused;

    [SerializeField] List<NamedAudioSource> audioSources = new List<NamedAudioSource>();
    private void Awake()
    {
        if (Inctance == null)
        {
            Inctance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        focused = Application.isFocused;
    }

    private void OnApplicationFocus(bool focus)
    {
        focused = focus;
    }

    void Update()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].source == null)
            {
                audioSources.RemoveAt(i);
                i--;
            }
            if (i >= audioSources.Count - 1)
            {
                if (!audioSources[i].source.isPlaying && audioSources[i].SoundEffect)
                {

                    Destroy(audioSources[i].source);
                    audioSources.RemoveAt(i);
                    i--;
                }
            }
        }
        if (focused)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].source.mute = false;
            }
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].source.mute = true;
            }
        }
    }

    public static SoundManager getSoundManager() { return Inctance; }

    public AudioSource CreateSoundEffect(string sourceName, AudioClip Sound, bool deleteOnSceneExit = false)
    {
        AudioSource source = null;
        if (!deleteOnSceneExit)
        {
            source = this.AddComponent<AudioSource>();
            audioSources.Add(new NamedAudioSource(sourceName, source, true));
        }
        else
        {
            source = createTempSoundSource().AddComponent<AudioSource>();
            source.GetComponent<localAudioSource>().source = source;
            audioSources.Add(new NamedAudioSource(sourceName, source, true));
        }

        source.clip = Sound;
        source.loop = false;
        source.Play();

        if (!focused)
        {
            source.mute = true;
        }

        source.outputAudioMixerGroup = mainGroup;

        for (int b = 0; b < allSounds.allSounds.Count; b++)
        {
            if (allSounds.allSounds[b].Clip == Sound)
            {
                source.volume = allSounds.allSounds[b].VolumeBalance;
            }
        }

        return source;
    }

    public AudioSource CreateIdleSource(string sourceName, AudioClip Sound, bool deleteOnSceneExit = true)
    {
        AudioSource source = null;
        if (!deleteOnSceneExit)
        {
            source = this.AddComponent<AudioSource>();
            audioSources.Add(new NamedAudioSource(sourceName, source, false));
        }
        else
        {
            source = createTempSoundSource().AddComponent<AudioSource>();
            source.GetComponent<localAudioSource>().source = source;
            audioSources.Add(new NamedAudioSource(sourceName, source, false));
        }

        source.clip = Sound;
        source.loop = false;
        source.outputAudioMixerGroup = mainGroup;

        if (!focused)
        {
            source.mute = true;
        }

        for (int b = 0; b < allSounds.allSounds.Count; b++)
        {
            if (allSounds.allSounds[b].Clip == Sound)
            {
                source.volume = allSounds.allSounds[b].VolumeBalance;
            }
        }

        return source;
    }

    public AudioSource CreateLoopingSound(string sourceName, AudioClip Sound, bool deleteOnSceneExit = true)
    {
        AudioSource source = null;
        if (!deleteOnSceneExit)
        {
            source = this.AddComponent<AudioSource>();
            audioSources.Add(new NamedAudioSource(sourceName, source, false));
        }
        else
        {
            source = createTempSoundSource().AddComponent<AudioSource>();
            source.GetComponent<localAudioSource>().source = source;
            audioSources.Add(new NamedAudioSource(sourceName, source, false));
        }

        source.clip = Sound;
        source.loop = true;
        source.Play();
        source.outputAudioMixerGroup = mainGroup;

        if (!focused)
        {
            source.mute = true;
        }

        for (int b = 0; b < allSounds.allSounds.Count; b++)
        {
            if (allSounds.allSounds[b].Clip == Sound)
            {
                source.volume = allSounds.allSounds[b].VolumeBalance;
            }
        }

        return source;
    }

    public void changeVolume(float volume)
    {
        mainMixer.SetFloat("MainVolume", volume);
    }

    GameObject createTempSoundSource()
    {
        GameObject localSound = Instantiate(GameManager.get().emptyObject);
        localSound.AddComponent<localAudioSource>();
        localSound.name = "Sound Source";
        return localSound;
    }

    public AudioClip GetSoundFromList(string soundName)
    {
        for (int i = 0; i < allSounds.allSounds.Count; i++)
        {
            if (allSounds.allSounds[i].SoundName == soundName)
            {
                return allSounds.allSounds[i].Clip;
            }
        }
        return null;
    }

    public AudioSource getActiveSource(string sourceName)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].name == sourceName)
            {
                return audioSources[i].source;
            }
        }
        return null;
    }

    public AudioSource getActiveSource(string sourceName, out NamedAudioSource activeSource)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].name == sourceName)
            {
                activeSource = audioSources[i];
                return audioSources[i].source;
            }
        }
        activeSource = null;
        return null;
    }

    public List<AudioSource> getAllActiveSourceNamed(string sourceName)
    {
        List<AudioSource> audio = new List<AudioSource>();

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].name == sourceName)
            {
                audio.Add(audioSources[i].source);
            }
        }
        return audio;
    }

    public void DeleteSource(string sourceName)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].name == sourceName && audioSources[i].source.GetComponent<localAudioSource>())
            {
                Destroy(audioSources[i].source.gameObject);
                audioSources.RemoveAt(i);
                i--;
            }
            else if (audioSources[i].name == sourceName && audioSources[i].source.GetComponent<SoundManager>())
            {
                Destroy(audioSources[i].source);
                audioSources.RemoveAt(i);
                i--;
            }
        }
    }

    public void DeleteSource(NamedAudioSource namedSource)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i] == namedSource && audioSources[i].source.GetComponent<localAudioSource>())
            {
                Destroy(audioSources[i].source.gameObject);
                audioSources.RemoveAt(i);
                i--;
            }
            else if (audioSources[i] == namedSource && audioSources[i].source.GetComponent<SoundManager>())
            {
                Destroy(audioSources[i].source);
                audioSources.RemoveAt(i);
                i--;
            }
        }
    }

    public void DeleteAllSources()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].source.GetComponent<localAudioSource>())
            {
                Destroy(audioSources[i].source.gameObject);
                audioSources.RemoveAt(i);
                i--;
            }
            else if (audioSources[i].source.GetComponent<SoundManager>())
            {
                Destroy(audioSources[i].source);
                audioSources.RemoveAt(i);
                i--;
            }
        }
    }

    public void DeleteAllSources(string exeption)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].source.GetComponent<localAudioSource>())
            {
                if (audioSources[i].name != exeption)
                {
                    Destroy(audioSources[i].source.gameObject);
                    audioSources.RemoveAt(i);
                    i--;
                }
            }
            else if (audioSources[i].source.GetComponent<SoundManager>())
            {
                if (audioSources[i].name != exeption)
                {
                    Destroy(audioSources[i].source);
                    audioSources.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void playSoundOnIdleSource(string sourceName, AudioClip clip = null)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].name == sourceName)
            {
                if (clip != null)
                {
                    audioSources[i].source.clip = clip;

                    for (int b = 0; b < allSounds.allSounds.Count; b++)
                    {
                        if (allSounds.allSounds[b].Clip == clip)
                        {
                            audioSources[i].source.volume = allSounds.allSounds[b].VolumeBalance;
                        }
                    }
                }
                audioSources[i].source.Stop();
                audioSources[i].source.Play();
            }
        }
    }
}
