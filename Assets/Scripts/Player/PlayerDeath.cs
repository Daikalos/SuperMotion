using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField, Tooltip("How fast the game slows down when game over"), Range(0.0f, 15.0f)]
    private float m_SlowDownSpeed = 0.95f;

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.GameOver)
        {
            if (Time.timeScale > 0.0f)
            {
                float newTimeScale = Time.timeScale - m_SlowDownSpeed * (Time.deltaTime / Time.timeScale);
                Time.timeScale = Mathf.Clamp(newTimeScale, 0.0f, 1.0f);
            }
        }
    }
}
