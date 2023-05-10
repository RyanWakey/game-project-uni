using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    public static bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name == "Game")
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }
}