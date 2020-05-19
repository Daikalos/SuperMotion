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

    private List<GameObject> m_Highscores;

    private void Awake()
    {
        m_Highscores = new List<GameObject>();
    }

    private void OnEnable()
    {
        for (int i = 1; i < m_LevelCount + 1; i++)
        {
            float highscore = PlayerPrefs.GetFloat("HighScore-" + i, 0.0f);

            GameObject highscoreObject = Instantiate(m_HighscoreTemplate, m_HighscoreElements.transform) as GameObject;
            highscoreObject.transform.localScale = Vector3.one;
            highscoreObject.SetActive(true);

            highscoreObject.transform.Find("LevelText").GetComponent<TMP_Text>().text = "Level-" + NumberFormat(i);
            highscoreObject.transform.Find("TimeText").GetComponent<TMP_Text>().text = (highscore != 0.0f) ? TimeFormat(highscore) : "-";

            m_Highscores.Add(highscoreObject);
        }
    }

    private void OnDisable()
    {
        m_Highscores.ForEach(o => Destroy(o));
        m_Highscores.Clear();
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
