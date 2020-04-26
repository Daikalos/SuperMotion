using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text m_Timer;
    private float m_StartTime;

    void Start()
    {
        m_StartTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - m_StartTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        m_Timer.text = minutes + ":" + seconds;
    }
}
