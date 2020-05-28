using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Starting,
    Playing,
    Paused,
    GameOver,
    LevelComplete
}

public class GameManager : InitializeSingleton<GameManager>
{
    public GameState GameState { get; private set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameState = GameState.Starting;
        Time.timeScale = 0.0f;

        GameStart.Instance.StartCountdown();
    }

    public void SetState(GameState newState)
    {
        if (GameState == GameState.LevelComplete || GameState == GameState.GameOver)
        {
            //Do not allow gamestate to be changed when end state is set
            return;
        }

        GameState = newState;
    }
}
