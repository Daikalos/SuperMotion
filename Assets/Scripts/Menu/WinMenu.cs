﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private Button
        m_ReplayButton = null,
        m_NextLevelButton = null,
        m_MainMenuButton = null;
    [SerializeField]
    private TMP_Text
        m_WinTime = null,
        m_HighscoreTime = null;
    [SerializeField]
    private GameObject
        m_HUD = null,
        m_WinOptions = null;
    [SerializeField]
    private Timer m_Timer = null;

    private CanvasGroup m_OptionsCanvasGroup;

    private bool m_CoroutineIsRunning;

    void Start()
    {
        m_ReplayButton.onClick.AddListener(Replay);
        m_NextLevelButton.onClick.AddListener(NextLevel);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);

        m_OptionsCanvasGroup = m_WinOptions.GetComponent<CanvasGroup>();
        
        //Can only press next level if next level exists
        m_NextLevelButton.interactable = Utilities.NextLevelExists();

        m_CoroutineIsRunning = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.LevelComplete)
        {
            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(FadeInOptions());

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                m_HUD.SetActive(false);
                m_WinTime.text = Utilities.TimeFormat(m_Timer.TimePassed, "0.557");
            }

            float highscore = PlayerPrefs.GetFloat("Highscore-" + Utilities.LevelNumber(), 0.0f);
            m_HighscoreTime.text = (highscore > Mathf.Epsilon) ? Utilities.TimeFormat(highscore, "0.557") : "-";
        }
    }

    private IEnumerator FadeInOptions()
    {
        m_WinOptions.SetActive(true);
        m_OptionsCanvasGroup.alpha = 0.0f;

        while (m_OptionsCanvasGroup.alpha < 1.0f)
        {
            yield return null;
            m_OptionsCanvasGroup.alpha = (1.0f - Time.timeScale);
        }
    }

    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        LevelHandler.Instance.CheckpointSet = false;
        LevelHandler.Instance.Countdown = Utilities.InLevel();
    }

    private void NextLevel()
    {
        //Go to next level in build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OpenMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
