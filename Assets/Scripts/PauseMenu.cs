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

    private GameObject m_HUD;
    private GameObject m_PauseMenu;
    private GameObject m_ControlsMenu;

    public static bool IsPaused { get; private set; }

    void Start()
    {
        m_HUD = transform.parent.Find("HUD").gameObject;
        m_PauseMenu = transform.Find("PauseOptions").gameObject;
        m_ControlsMenu = transform.Find("ControlsMenu").gameObject;

        m_ResumeButton.onClick.AddListener(ResumeGame);
        m_ControlsButton.onClick.AddListener(OpenControls);
        m_MainMenuButton.onClick.AddListener(OpenMainMenu);
        m_BackButton.onClick.AddListener(Back);

        IsPaused = false;

        m_HUD.SetActive(!IsPaused);
        m_PauseMenu.SetActive(IsPaused);
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
        IsPaused = !IsPaused;
        Time.timeScale = 1.0f - Time.timeScale;

        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsPaused;

        m_HUD.SetActive(!IsPaused);
        m_PauseMenu.SetActive(IsPaused);
        m_ControlsMenu.SetActive(false);
    }

    public void OpenControls()
    {
        m_PauseMenu.SetActive(false);
        m_ControlsMenu.SetActive(true);
    }

    public void Back()
    {
        m_PauseMenu.SetActive(true);
        m_ControlsMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
