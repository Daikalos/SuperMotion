using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string m_Name;

    public AudioClip m_Clip;
    public AudioMixerGroup m_Output;

    [Range(0f, 1f)]
    public float m_Volume;
    [Range(0.1f, 3f)]
    public float m_Pitch;

    public bool m_Loop;

    [HideInInspector]
    public AudioSource source;
}
