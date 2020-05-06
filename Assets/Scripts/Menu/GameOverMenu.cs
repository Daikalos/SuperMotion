using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Button
        m_RetryButton = null,
        m_MainMenuButton = null;
    [SerializeField]
    private GameObject
        m_HUD = null,
        m_GameOverOptions = null;

    private CanvasGroup m_GUI;

    private bool m_CoroutineIsRunning;

    void Start()
    {
        m_RetryButton.onClick.AddListener(Retry);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);

        m_GUI = m_GameOverOptions.GetComponent<CanvasGroup>();

        m_CoroutineIsRunning = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.GameOver)
        {
            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(FadeInGUI());

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                m_HUD.SetActive(false);
            }
        }
    }

    private IEnumerator FadeInGUI()
    {
        m_GameOverOptions.SetActive(true);
        m_GUI.alpha = 0.0f;

        while (m_GUI.alpha < 1.0f)
        {
            yield return null;
            m_GUI.alpha = (1.0f - Time.timeScale);
        }
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
