using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlePause : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    
    
    private void Update()
    {
        if (GameIsPaused && Input.GetKeyDown(KeyCode.Q))
        {
            Resume();
            SceneManager.LoadScene("StartScene");
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
