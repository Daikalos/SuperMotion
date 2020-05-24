using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Button
        m_CheckpointButton = null,
        m_RestartButton = null,
        m_MainMenuButton = null;
    [SerializeField]
    private GameObject
        m_HUD = null,
        m_ConfirmPanel = null,
        m_GameOverOptions = null;
    [SerializeField]
    private Timer m_Timer = null;

    private ConfirmMenu m_ConfirmMenu;
    private CanvasGroup m_OptionsCanvasGroup;

    private bool m_CoroutineIsRunning;

    void Start()
    {
        m_CheckpointButton.onClick.AddListener(LoadCheckpoint);
        m_RestartButton.onClick.AddListener(Restart);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);

        m_ConfirmMenu = m_ConfirmPanel.GetComponent<ConfirmMenu>();
        m_OptionsCanvasGroup = m_GameOverOptions.GetComponent<CanvasGroup>();

        m_CoroutineIsRunning = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.GameOver)
        {
            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(FadeInOptions());

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                m_HUD.SetActive(false);

                //Can only press load checkpoint if one has been set
                m_CheckpointButton.interactable = (CheckpointManager.Instance.CheckpointSet);
                CheckpointManager.Instance.CheckpointTime = m_Timer.TimePassed;
            }
        }
    }

    private IEnumerator FadeInOptions()
    {
        m_GameOverOptions.SetActive(true);
        m_OptionsCanvasGroup.alpha = 0.0f;

        while (m_OptionsCanvasGroup.alpha < 1.0f)
        {
            yield return null;
            m_OptionsCanvasGroup.alpha = (1.0f - Time.timeScale);
        }
    }

    private void LoadCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Restart()
    {
        if (!m_ConfirmPanel.activeSelf)
        {
            m_ConfirmPanel.SetActive(true);
            m_GameOverOptions.SetActive(false);

            m_ConfirmMenu.YesAction(new UnityAction(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CheckpointManager.Instance.CheckpointSet = false;
            }));

            m_ConfirmMenu.NoAction(new UnityAction(() =>
            {
                m_ConfirmPanel.SetActive(false);
                m_GameOverOptions.SetActive(true);
            }));
        }
    }

    public void OpenMainMenu()
    {
        if (!m_ConfirmPanel.activeSelf)
        {
            m_ConfirmPanel.SetActive(true);
            m_GameOverOptions.SetActive(false);

            ConfirmMenu confirmMenu = m_ConfirmPanel.GetComponent<ConfirmMenu>();

            confirmMenu.YesAction(new UnityAction(() =>
            {
                SceneManager.LoadScene("Main_Menu");
            }));

            confirmMenu.NoAction(new UnityAction(() =>
            {
                m_ConfirmPanel.SetActive(false);
                m_GameOverOptions.SetActive(true);
            }));
        }
    }
}
