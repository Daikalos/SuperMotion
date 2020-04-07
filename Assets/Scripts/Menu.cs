using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayLevel1() { SceneManager.LoadScene(1); }
    public void PlayLevel2() { SceneManager.LoadScene(2); }
    public void PlayLevel3() { SceneManager.LoadScene(3); }

    // Developer levels
    public void PlayAlbin() { SceneManager.LoadScene("Albin_Scene"); }
    public void PlayPhilip() { SceneManager.LoadScene("Philip_Scene"); }
    public void PlayTinea() { SceneManager.LoadScene("Tinea_Scene"); }
    public void PlayTobias() { SceneManager.LoadScene("Tobias_Scene"); }
    public void PlayYashi() { SceneManager.LoadScene("Yashi_Scene"); }

    public void QuitGame() { Application.Quit(); }
}