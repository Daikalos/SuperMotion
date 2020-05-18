using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{
    [SerializeField, Tooltip("How fast the game slows down when level complete"), Range(0.0f, 15.0f)]
    private float m_SlowDownSpeed = 0.95f;
    [SerializeField]
    private Timer m_Timer = null;

    private bool m_CoroutineIsRunning;

    private void Start()
    {
        m_CoroutineIsRunning = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.LevelComplete)
        {
            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(LevelCompleted());
            }
        }
    }

    private IEnumerator LevelCompleted()
    {
        SaveHighScore();

        while (Time.timeScale > 0.0f)
        {
            yield return null;

            //Slow down effect
            float newTimeScale = Time.timeScale - m_SlowDownSpeed * (Time.deltaTime / Time.timeScale);
            Time.timeScale = Mathf.Clamp(newTimeScale, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Save completion time if it is lower than highscore
    /// </summary>
    private void SaveHighScore()
    {
        if (Extensions.InLevel())
        {
            float levelHS = PlayerPrefs.GetFloat("HighScore-" + Extensions.LevelNumber(), Mathf.Infinity);
            float currentHS = m_Timer.TimePassed;

            //If impossible HighScore is already set, reset it
            if (levelHS < Mathf.Epsilon)
            {
                levelHS = Mathf.Infinity;
            }

            //New HighScore is achieved
            if (currentHS < levelHS)
            {
                PlayerPrefs.SetFloat("HighScore-" + Extensions.LevelNumber(), currentHS);
            }
        }
    }
}
