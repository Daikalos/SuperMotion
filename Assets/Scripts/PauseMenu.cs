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

        m_ResumeButton.onClick.AddListener(ResumeButton);
        m_ControlsButton.onClick.AddListener(ControlsButton);
        m_MainMenuButton.onClick.AddListener(MainMenuButton);
        m_BackButton.onClick.AddListener(BackButton);

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
            ResumeButton();
        }
    }

    public void ResumeButton()
    {
        IsPaused = !IsPaused;
        Time.timeScale = 1.0f - Time.timeScale;

        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsPaused;

        Debug.Log(m_HUD.activeSelf);

        m_HUD.SetActive(!IsPaused);
        m_PauseMenu.SetActive(IsPaused);
        m_ControlsMenu.SetActive(false);
    }

    public void ControlsButton()
    {
        m_PauseMenu.SetActive(false);
        m_ControlsMenu.SetActive(true);
    }

    public void BackButton()
    {
        m_PauseMenu.SetActive(true);
        m_ControlsMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
