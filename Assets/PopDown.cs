using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    public GameObject panel;
    float m_timer = 0;
    bool m_isOpen;

    private void Start()
    {
        //Show at start
        m_timer = 3;
        m_isOpen = true;
    }

    private void Update()
    {
        //Show if "5" is pressed and the panel is not currently visible
        if (Input.GetKeyDown(KeyCode.Alpha5) && !m_isOpen)
        {
            m_timer = 3;
            m_isOpen = true;
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
