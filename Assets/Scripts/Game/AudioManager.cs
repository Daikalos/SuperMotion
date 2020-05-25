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
            SetAudioSource(sound.source, sound);
        }
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

    public AudioSource AddSpatialAudioSource(GameObject gameObject, string name)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        SetAudioSource(audioSource, GetSound(name));
        audioSource.spatialBlend = 1.0f;

        return audioSource;
    }

    public void PlaySoundAtPoint(Vector3 position, string name)
    {
        GameObject soundObject = new GameObject();
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        SetAudioSource(audioSource, GetSound(name));
        audioSource.spatialBlend = 1.0f;

        audioSource.Play();

        Destroy(soundObject, audioSource.clip.length);
    }

    private Sound GetSound(string name)
    {
        return Array.Find(m_Sounds, sound => sound.m_Name == name);
    }

    private void SetAudioSource(AudioSource audioSource, Sound sound)
    {
        if (audioSource != null)
        {
            audioSource.clip = sound.m_Clip;
            audioSource.outputAudioMixerGroup = sound.m_Output;

            audioSource.volume = sound.m_Volume;
            audioSource.pitch = sound.m_Pitch;
            audioSource.loop = sound.m_Loop;
        }
    }
}
