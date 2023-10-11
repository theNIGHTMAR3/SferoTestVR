using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Dungeon");
    }
    public void QuitGame()
    {
        Debug.Log("Application exited!");
        Application.Quit();
    }
}
