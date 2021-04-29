using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    public GameObject pauseButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        pauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = Difficulty.difficulty;
        isPaused = false;
    }

    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
