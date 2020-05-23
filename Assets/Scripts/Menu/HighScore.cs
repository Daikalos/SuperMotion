using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    [SerializeField, Tooltip("How many levels that are used in the game")]
    private int m_LevelCount = 0;

    [SerializeField]
    private GameObject
        m_HighscoreElements = null,
        m_HighscoreTemplate = null;

    private void Start()
    {
        for (int i = 1; i < m_LevelCount + 1; i++)
        {
            float highscore = PlayerPrefs.GetFloat("Highscore-" + i, 0.0f);

            GameObject highscoreObject = Instantiate(m_HighscoreTemplate, m_HighscoreElements.transform) as GameObject;
            highscoreObject.transform.localScale = Vector3.one;
            highscoreObject.SetActive(true);

            highscoreObject.transform.Find("LevelText").GetComponent<TMP_Text>().text = "Level-" + Utilities.NumberFormat(i);
            highscoreObject.transform.Find("TimeText").GetComponent<TMP_Text>().text = (highscore != 0.0f) ? Utilities.TimeFormat(highscore, "0.8") : "-";
        }
    }
}
