using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilities
{
    #region LevelManagement
    /// <summary>
    /// If player is currently in a level
    /// </summary>
    public static bool InLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        return levelName.Contains("Level");
    }

    /// <summary>
    /// Get the current number identifier of the level
    /// </summary>
    public static int LevelNumber()
    {
        if (InLevel())
        {
            string levelName = SceneManager.GetActiveScene().name;
            levelName = levelName.Replace("Level_", string.Empty);

            if (int.TryParse(levelName, out int levelNumber))
            {
                return levelNumber;
            }
        }
        return 0;
    }

    /// <summary>
    /// Check if next level exists or not
    /// </summary>
    public static bool NextLevelExists()
    {
        if (InLevel())
        {
            return SceneExists("Level_" + (LevelNumber() + 1));
        }
        return false;
    }

    /// <summary>
    /// Go through all scene names and check if scene exists
    /// </summary>
    public static bool SceneExists(string scene)
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            string sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (sceneName == scene)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Vector3
    public static Vector3 SetX(this Vector3 vector, float newValue)
    {
        vector.x = newValue;
        return vector;
    }

    public static Vector3 SetY(this Vector3 vector, float newValue)
    {
        vector.y = newValue;
        return vector;
    }

    public static Vector3 SetZ(this Vector3 vector, float newValue)
    {
        vector.z = newValue;
        return vector;
    }
    #endregion

    #region StringFormat
    public static string NumberFormat(int number)
    {
        return (number < 10) ? "0" + number : number.ToString();
    }

    public static string TimeFormat(float totalTime, string characterSpace)
    {
        return string.Format(
            "<mspace=" + characterSpace + "em>{0:00}</mspace>" + ":" +
            "<mspace=" + characterSpace + "em>{1:00}</mspace>" + "." +
            "<mspace=" + characterSpace + "em>{2:000}</mspace>",
            (int)(totalTime / 60), (int)(totalTime % 60), (int)(totalTime * 1000) % 1000);
    }
    #endregion
}
