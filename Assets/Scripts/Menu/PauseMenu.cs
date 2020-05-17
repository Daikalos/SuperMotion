﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button 
        m_ResumeButton = null,
        m_CheckpointButton = null,
        m_ControlsButton = null,
        m_MainMenuButton = null,
        m_BackButton = null;
    [SerializeField]
    private GameObject 
        m_HUD = null,
        m_ConfirmPanel = null,
        m_PauseOptions = null,
        m_ControlsMenu = null;

    private bool m_IsPaused;

    void Start()
    {
        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_CheckpointButton.onClick.AddListener(LoadCheckpoint);
        m_ControlsButton.onClick.AddListener(OpenControls);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);
        m_BackButton.onClick.AddListener(Back);
        
        m_IsPaused = false;

        m_HUD.SetActive(!m_IsPaused);
        m_PauseOptions.SetActive(m_IsPaused);
        m_ControlsMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        if (GameManager.Instance.GameState == GameState.Playing || GameManager.Instance.GameState == GameState.Paused)
        {
            m_IsPaused = !m_IsPaused;
            Time.timeScale = 1.0f - Time.timeScale;

            GameState newState = (m_IsPaused) ? GameState.Paused : GameState.Playing;
            GameManager.Instance.SetState(newState);

            Cursor.lockState = m_IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = m_IsPaused;

            m_HUD.SetActive(!m_IsPaused);
            m_ConfirmPanel.SetActive(false);
            m_PauseOptions.SetActive(m_IsPaused);
            m_ControlsMenu.SetActive(false);
        }
    }

    private void LoadCheckpoint()
    {
        if (!m_ConfirmPanel.activeSelf)
        {
            m_ConfirmPanel.SetActive(true);
            m_PauseOptions.SetActive(false);

            ConfirmMenu confirmMenu = m_ConfirmPanel.GetComponent<ConfirmMenu>();

            confirmMenu.YesAction(new UnityAction(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }));

            confirmMenu.NoAction(new UnityAction(() =>
            {
                m_ConfirmPanel.SetActive(false);
                m_PauseOptions.SetActive(true);
            }));
        }
    }

    private void OpenControls()
    {
        m_PauseOptions.SetActive(false);
        m_ControlsMenu.SetActive(true);
    }

    private void Back()
    {
        m_PauseOptions.SetActive(true);
        m_ControlsMenu.SetActive(false);
    }

    private void OpenMainMenu()
    {
        if (!m_ConfirmPanel.activeSelf)
        {
            m_ConfirmPanel.SetActive(true);
            m_PauseOptions.SetActive(false);

            ConfirmMenu confirmMenu = m_ConfirmPanel.GetComponent<ConfirmMenu>();

            confirmMenu.YesAction(new UnityAction(() =>
            {
                SceneManager.LoadScene("Main_Menu");
            }));

            confirmMenu.NoAction(new UnityAction(() =>
            {
                m_ConfirmPanel.SetActive(false);
                m_PauseOptions.SetActive(true);
            }));
        }
    }
}
