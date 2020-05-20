using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    public GameObject panel;
    float m_timer = 0;
    const float m_timerMaxTimeOpen = 10;
    bool m_isOpen;

    private void Start()
    {
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
    }

    public void HandlePanel()
    {
        if (panel != null)
        {
            Animator animator = panel.GetComponent<Animator>();

            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                animator.SetBool("open", false);
                m_isOpen = false;
            }
            else
            {
                if (animator != null)
                {
                    animator.SetBool("open", true);
                }
            }
        }
    }
}
