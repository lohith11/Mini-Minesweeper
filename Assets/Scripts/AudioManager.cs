using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] music;
    public static AudioManager audioManagerInstance;

    void Awake()
    {
        if (audioManagerInstance == null)
            audioManagerInstance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    void Start()
    {
        ChangeTrack();
    }

    public void Play(string name)
    {
        if (!DataCarrier.musicEnabled)
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void ChangeTrack()
    {
        if (!DataCarrier.musicEnabled)
            return;
        int trackNumber = UnityEngine.Random.Range(0, music.Length);
        Sound m = music[trackNumber];
        m.source.Play();
        Invoke("ChangeTrack", m.clip.length);
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
        foreach (Sound m in music)
        {
            m.source.Stop();
        }
    }
}
