using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
    [SerializeField]
    private Button[] m_Levels = null;
    [SerializeField, Tooltip("If levels are to be locked or not, used for debugging")]
    private bool m_LockLevels;

    private void Start()
    {
        if (m_LockLevels)
        {
            int currentLevel = 0;

            //Lock each level if it has not been completed; Skip first level to lock
            for (int i = 1; i < m_Levels.Length; i++)
            {
                //Use Highscore as a way to see if level has been completed
                float highscore = PlayerPrefs.GetFloat("HighScore-" + i, 0.0f);
                bool levelCompleted = (highscore > Mathf.Epsilon);

                //Unlock next level if current level is completed
                m_Levels[currentLevel + 1].interactable = levelCompleted;
                currentLevel++;
            }
        }
    }
}
