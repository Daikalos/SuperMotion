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
    private float m_CheckpointTime;

    public float TimePassed => m_TimePassed;

    void Start()
    {
        m_StartTime = Time.time;
        m_CheckpointTime = CheckpointManager.Instance.CheckpointTime;
    }

    void Update()
    {
        m_TimePassed = (Time.time + m_CheckpointTime) - m_StartTime;
        m_Timer.SetText(Utilities.TimeFormat(m_TimePassed, "0.62"));
    }
}
