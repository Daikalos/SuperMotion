using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : InitializeSingleton<LevelHandler>
{
    private string m_SceneName;

    public Vector3 CheckpointPosition { get; set; }
    public Vector3 CheckpointRotation { get; set; }
    public bool CheckpointSet { get; set; }
    public float CheckpointTime { get; set; }

    public bool ShowControlsPanel { get; set; }

    public bool Countdown { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(Instance);

        InitializeVariables();

        m_SceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeVariables()
    {
        CheckpointPosition = Vector3.zero;
        CheckpointRotation = Vector3.zero;
        CheckpointSet = false;
        CheckpointTime = 0.0f;

        //Used to make sure to display PopDownPanel only once
        ShowControlsPanel = true;

        //If there is to be a countdown or not when level is loaded
        Countdown = Utilities.InLevel();
    }

    /// <summary>
    /// Check if this is a new level or not to reset variables
    /// </summary>
    public void NewLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (m_SceneName != currentScene)
        {
            InitializeVariables();
            m_SceneName = currentScene;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //If this is a new scene and variables has not yet been reset, remove this
        //Will be removed in scenes where there is no player to check if this is new level
        if (m_SceneName != scene.name)
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
