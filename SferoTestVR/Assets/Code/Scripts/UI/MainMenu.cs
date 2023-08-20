using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Michal tests");
    }
    public void QuitGame()
    {
        Debug.Log("Application exited!");
        Application.Quit();
    }
}
