using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void HoverSound()
    {
        AudioManager.m_Instance.PlayOnce("Hover");
    }
    public void ClickSound()
    {
        AudioManager.m_Instance.PlayOnce("Click");
    }
}
