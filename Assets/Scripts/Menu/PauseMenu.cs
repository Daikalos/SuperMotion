using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button 
        m_ResumeButton = null,
        m_ControlsButton = null,
        m_MainMenuButton = null,
        m_BackButton = null;
    [SerializeField]
    private GameObject 
        m_HUD = null,
        m_PauseOptions = null,
        m_ControlsMenu = null;

    private bool m_IsPaused;

    void Start()
    {
        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_ControlsButton.onClick.AddListener(OpenControls);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);
        m_BackButton.onClick.AddListener(Back);
        
        m_IsPaused = false;

        m_HUD.SetActive(!m_IsPaused);
        m_PauseOptions.SetActive(m_IsPaused);
        m_ControlsMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
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
            m_PauseOptions.SetActive(m_IsPaused);
            m_ControlsMenu.SetActive(false);
        }
    }

    public void OpenControls()
    {
        m_PauseOptions.SetActive(false);
        m_ControlsMenu.SetActive(true);
    }

    public void Back()
    {
        m_PauseOptions.SetActive(true);
        m_ControlsMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
