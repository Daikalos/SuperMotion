using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    LevelComplete
}

public class GameManager : InitializeSingleton<GameManager>
{
    public GameState GameState { get; private set; }
    public static int LevelCount { get; private set; }

    void Start()
    {
        GameState = GameState.Playing;
        Time.timeScale = 1.0f;
    }

    public void SetState(GameState newState)
    {
        if (GameState == GameState.LevelComplete || GameState == GameState.GameOver)
        {
            //Don't allow gamestate to be changed when end state is set
            return;
        }

        GameState = newState;
    }
}
