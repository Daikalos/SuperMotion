using System.Collections;
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
        m_TimerText,
        m_WinTimerText;
    [SerializeField]
    private GameObject
        m_HUD = null,
        m_WinOptions = null;
    [SerializeField]
    private Timer m_Timer;

    private CanvasGroup m_GUI;

    private bool m_CoroutineIsRunning;

    void Start()
    {
        m_ReplayButton.onClick.AddListener(Replay);
        m_NextLevelButton.onClick.AddListener(NextLevel);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);

        m_GUI = m_WinOptions.GetComponent<CanvasGroup>();
        
        //Can only press next level if next level exists
        m_NextLevelButton.interactable = NextLevelExists();

        m_CoroutineIsRunning = false;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.LevelComplete)
        {
            if (!m_CoroutineIsRunning)
            {
                m_CoroutineIsRunning = true;
                StartCoroutine(FadeInGUI());

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                m_HUD.SetActive(false);
                m_WinTimerText.text = "Time: " + m_TimerText.text;

                SaveHighScore();
            }
        }
    }

    private IEnumerator FadeInGUI()
    {
        m_WinOptions.SetActive(true);
        m_GUI.alpha = 0.0f;

        while (m_GUI.alpha < 1.0f)
        {
            yield return null;
            m_GUI.alpha = (1.0f - Time.timeScale);
        }
    }

    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    /// <summary>
    /// Save completion time if it is lower than highscore
    /// </summary>
    private void SaveHighScore()
    {
        if (OnLevel())
        {
            float levelHS = PlayerPrefs.GetFloat("HighScore-" + LevelNumber(), Mathf.Infinity);
            float currentHS = 0.0f;

            //New HighScore is achieved
            if (currentHS < levelHS)
            {
                //PlayerPrefs.SetFloat("HighScore-" + levelNumber, )
            }
        }
    }

    /// <summary>
    /// Check if next level exists or not
    /// </summary>
    private bool NextLevelExists()
    {
        if (OnLevel())
        {
            return SceneExists("Level_" + (LevelNumber() + 1));
        }
        return false;
    }

    /// <summary>
    /// If player is currently in a level
    /// </summary>
    private bool OnLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        return levelName.Contains("Level");
    }

    /// <summary>
    /// Get the current number identifier of the level
    /// </summary>
    private int LevelNumber()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (levelName.Contains("Level"))
        {
            levelName = levelName.Replace("Level_", string.Empty);
            if (int.TryParse(levelName, out int levelNumber))
            {
                return levelNumber;
            }
        }
        return 0;
    }

    /// <summary>
    /// Go through all scene names and check if scene exists
    /// </summary>
    private bool SceneExists(string scene)
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            string sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (sceneName == scene)
            {
                return true;
            }
        }
        return false;
    }
}
