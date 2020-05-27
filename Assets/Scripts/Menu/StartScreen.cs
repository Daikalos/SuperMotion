using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_CountdownText = null;
    [SerializeField]
    private GameObject 
        m_StartPanel = null,
        m_HUD = null;

    private Image m_PanelImage;
    private float m_PanelAlpha;

    private void Start()
    {
        if (LevelHandler.Instance.Countdown)
        {
            m_StartPanel.SetActive(true);
            m_HUD.SetActive(false);

            m_PanelImage = m_StartPanel.GetComponent<Image>();
            m_PanelAlpha = m_PanelImage.color.a;

            StartCoroutine(FadeOutPanel());
        }
        else
        {
            m_CountdownText.text = string.Empty;
            m_StartPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (LevelHandler.Instance.Countdown)
        {
            int currentTime = Mathf.CeilToInt(GameStart.Instance.CountdownTimer);
            int totalWaitTime = (int)GameStart.Instance.CountdownTime;

            //Formatting option for displaying ceiled rounded time, first is skipped due to being rounded
            m_CountdownText.text = (currentTime == 0) ? (totalWaitTime - 1).ToString() : (totalWaitTime - currentTime).ToString();

            if (GameStart.Instance.CountdownTimer > GameStart.Instance.CountdownTime)
            {
                m_CountdownText.text = string.Empty;

                gameObject.SetActive(false);
                m_HUD.SetActive(true);
            }
        }
    }

    private IEnumerator FadeOutPanel()
    {
        while (m_PanelImage.color.a > 0.0f)
        {
            yield return null;
            m_PanelImage.color = new Color(m_PanelImage.color.r, m_PanelImage.color.g, m_PanelImage.color.b, m_PanelAlpha - ((m_PanelAlpha / GameStart.Instance.CountdownTime) * (GameStart.Instance.CountdownTimer)));
        }
    }
}
