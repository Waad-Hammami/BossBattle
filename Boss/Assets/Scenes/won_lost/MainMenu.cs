using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("QUIT !");
        Application.Quit();

    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MultiPhase"); ;

    }

}
