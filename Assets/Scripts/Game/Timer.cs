using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Tooltip("TextMeshProUGUI Object to display the timer")]
    public TextMeshProUGUI m_Timer;

    private float m_StartTime;
    private float m_TimePassed;

    public float TimePassed => m_TimePassed;

    void Start()
    {
        m_StartTime = Time.time;
        m_TimePassed = 0.0f;
    }

    void Update()
    {
        m_TimePassed = Time.time - m_StartTime;

        string minutes = ((int)m_TimePassed / 60).ToString();
        string seconds = (m_TimePassed % 60).ToString("f2");

        m_Timer.SetText(minutes + ":" + seconds);
    }
}
