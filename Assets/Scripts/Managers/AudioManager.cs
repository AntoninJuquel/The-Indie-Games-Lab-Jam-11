using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;
    [HideInInspector]
    public List<AudioSource> sources;
    public AudioMixerGroup outputAudioMixerGroup;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float spatialBlend;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            foreach (AudioClip a in s.clips)
            {
                s.sources.Add(gameObject.AddComponent<AudioSource>());
                s.sources[s.sources.Count-1].clip = a;
                s.sources[s.sources.Count - 1].volume = s.volume;
                s.sources[s.sources.Count - 1].pitch = s.pitch;
                s.sources[s.sources.Count - 1].loop = s.loop;
                s.sources[s.sources.Count - 1].spatialBlend = s.spatialBlend;
                s.sources[s.sources.Count - 1].outputAudioMixerGroup = s.outputAudioMixerGroup;
            }
        }

        if (instance == null)
            instance = this;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.sources[UnityEngine.Random.Range(0,s.sources.Count)].Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        foreach (AudioSource source in s.sources)
        {
            source.Stop();
        }
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        foreach (AudioSource source in s.sources)
        {
            source.Pause();
        }
    }
}
