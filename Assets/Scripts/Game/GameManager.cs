using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        GameState = GameState.Playing;
        Time.timeScale = 1.0f;
    }

    public void SetState(GameState newState)
    {
        GameState = newState;
    }
}
