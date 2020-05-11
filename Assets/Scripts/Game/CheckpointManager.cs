using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : InitializeSingleton<CheckpointManager>
{
    public Vector3 Checkpoint { get; set; }
    private string m_SceneName;

    private void Awake()
    {
        DontDestroyOnLoad(Instance);

        m_SceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //If this is a new scene, reset checkpoints
        if (m_SceneName != scene.name && this != null)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(Instance);

            m_SceneName = scene.name;
        }
    }
}
