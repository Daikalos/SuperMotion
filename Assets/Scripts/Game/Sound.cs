using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string m_Name;

    public AudioClip m_Clip;

    [Range(0f, 1f)]
    public float m_Volume;
    [Range(0.1f, 3f)]
    public float m_Pitch;

    public bool m_Loop;

    [HideInInspector]
    public AudioSource source;
}
