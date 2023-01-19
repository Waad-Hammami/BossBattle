using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("button");
        Debug.Log("QUIT !");
        Application.Quit();

    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}
