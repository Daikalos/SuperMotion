﻿using System.Collections;
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
    private float m_CheckpointTime;

    public float TimePassed => m_TimePassed;

    void Start()
    {
        m_StartTime = Time.time;

        //Set time to the time when checkpoint was triggered
        m_CheckpointTime = CheckpointManager.Instance.CheckpointTime;
    }

    void Update()
    {
        m_TimePassed = (Time.time + m_CheckpointTime) - m_StartTime;

        string minutes = ((int)m_TimePassed / 60).ToString();
        string seconds = (m_TimePassed % 60).ToString("f3");

        m_Timer.SetText(minutes + ":" + seconds);
    }
}
