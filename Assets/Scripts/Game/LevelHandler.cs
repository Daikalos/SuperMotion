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

        //Used to make sure to display PopDownPanel only once
        ShowControlsPanel = true;

        //If there is to be a countdown or not when level is loaded
        Countdown = Utilities.InLevel();

        m_SceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //If this is a new scene, remove levelhandler to reset variables
        if (m_SceneName != scene.name)
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else
        {
            //If the current scene is loaded, start countdown
            GameStart.Instance.StartCountdown();
        }
    }
}
