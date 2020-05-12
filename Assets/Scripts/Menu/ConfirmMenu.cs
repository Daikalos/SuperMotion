using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmMenu : MonoBehaviour
{
    [SerializeField]
    private Button
        m_YesButton = null,
        m_NoButton = null;

    public void YesAction(UnityAction yesAction)
    {
        m_YesButton.onClick.RemoveAllListeners();
        m_YesButton.onClick.AddListener(yesAction);
    }

    public void NoAction(UnityAction noAction)
    {
        m_NoButton.onClick.RemoveAllListeners();
        m_NoButton.onClick.AddListener(noAction);
    }
}
