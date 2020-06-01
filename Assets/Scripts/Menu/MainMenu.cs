using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel1() { SceneManager.LoadScene("Level_1"); }
    public void PlayLevel2() { SceneManager.LoadScene("Level_2"); }
    public void PlayLevel3() { SceneManager.LoadScene("Level_3"); }
    public void PlayTutorial() { SceneManager.LoadScene("Tutorial"); }

    public void QuitGame() { Application.Quit(); }
}