using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] m_Sounds;

    public static AudioManager m_Instance;

    void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in m_Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.m_Clip;

            sound.source.volume = sound.m_Volume;
            sound.source.pitch = sound.m_Pitch;
            sound.source.loop = sound.m_Loop;
        }
    }

    void Start()
    {
        Play("Song");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.m_Name == name);
        s.source.Play();
    }
    public void PlayOnce(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.m_Name == name);
        if (s.source.isPlaying == false)
        {
            s.source.PlayOneShot(s.source.clip);
        }
    }
}
