using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    [SerializeField, Tooltip("How many levels that are used in the game")]
    private int m_LevelCount = 0;

    [SerializeField]
    private GameObject
        m_HighScoreElements = null,
        m_HighScoreTemplate = null;

    private List<GameObject> m_HighScores;

    private void Awake()
    {
        m_HighScores = new List<GameObject>();
    }

    private void OnEnable()
    {
        for (int i = 1; i < m_LevelCount + 1; i++)
        {
            float highScore = PlayerPrefs.GetFloat("HighScore-" + i, 0.0f);

            GameObject highScoreObject = Instantiate(m_HighScoreTemplate, m_HighScoreElements.transform) as GameObject;
            highScoreObject.transform.localScale = Vector3.one;
            highScoreObject.SetActive(true);

            highScoreObject.transform.Find("LevelText").GetComponent<TMP_Text>().text = "Level-" + NumberFormat(i);
            highScoreObject.transform.Find("TimeText").GetComponent<TMP_Text>().text = highScore != 0.0f ? TimeFormat(highScore) : "-";

            m_HighScores.Add(highScoreObject);
        }
    }

    private void OnDisable()
    {
        m_HighScores.ForEach(o => Destroy(o));
        m_HighScores.Clear();
    }

    private string NumberFormat(int number)
    {
        return (number < 10) ? "0" + number : number.ToString();
    }

    private string TimeFormat(float number)
    {
        return string.Format("{0:00}:{1:00}.{2:000}", (int)(number / 60), (int)(number % 60), (number * 1000) % 1000);
    }
}
