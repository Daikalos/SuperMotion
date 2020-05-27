using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : InitializeSingleton<GameStart>
{
    public float CountdownTime { get; private set; }
    public float CountdownTimer { get; private set; }

    public void StartCountdown()
    {
        CountdownTime = 4.0f;

        //If to start a countdown or instantly start level when loaded
        if (LevelHandler.Instance.Countdown)
        {
            StartCoroutine(Countdown());
        }
        else
        {
            StartGame();
        }
    }

    private IEnumerator Countdown()
    {
        //Wait one second for scene to completely load
        yield return new WaitForSecondsRealtime(1);

        float currentTime = Time.unscaledTime;

        while (CountdownTimer < CountdownTime)
        {
            yield return null;
            CountdownTimer = (Time.unscaledTime - currentTime);
        }

        StartGame();
    }

    private void StartGame()
    {
        GameManager.Instance.SetState(GameState.Playing);
        Time.timeScale = 1.0f;
    }
}
