using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    public GameObject panel;
    float m_timer = 0;
    const float m_timerMaxTimeOpen = 10;
    bool m_isOpen;

    private Animator m_Animator;
    private CanvasGroup m_PopDownCanvasGroup;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PopDownCanvasGroup = GetComponent<CanvasGroup>();

        //Show at start
        m_timer = m_timerMaxTimeOpen; //max time open set to 10 seconds, in case user forgets to close (keep or nah?)
        m_isOpen = true;
    }

    private void Update()
    {
        //Show if "Tab" is pressed and the panel is not currently visible
        if (Input.GetKeyDown(KeyCode.Tab) && !m_isOpen)
        {
            m_timer = m_timerMaxTimeOpen;
            m_isOpen = true;
        }
        //Close if "Tab" is pressed and the panel is currently visible
        else if (Input.GetKeyDown(KeyCode.Tab) && m_isOpen)
        {
            m_timer = 0;
        }

        HandlePanel();
        HidePanel();
    }

    public void HandlePanel()
    {
        if (panel != null)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_Animator.SetBool("open", false);
                m_isOpen = false;
            }
            else
            {
                if (m_Animator != null)
                {
                    m_Animator.SetBool("open", true);
                }
            }
        }
    }

    private void HidePanel()
    {
        if (GameManager.Instance.GameState == GameState.Paused)
        {
            m_Animator.speed = 0.0f;
            m_PopDownCanvasGroup.alpha = 0.0f;
        }
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            m_Animator.speed = 1.0f;
            m_PopDownCanvasGroup.alpha = 1.0f;
        }
    }
}
