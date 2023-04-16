using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Pause : MonoBehaviour
{
    public InputField playerName;
    public Button resumeButton;
    private bool isPaused = false;




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC key pauses AND resumes
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        resumeButton.gameObject.SetActive(true); // show the Resume button
        playerName.gameObject.SetActive(true);
    }
    public void ResumeGame() // called from ESC; also attached to the resume button
    {
        Time.timeScale = 1;
        isPaused = false;
        resumeButton.gameObject.SetActive(false);// hide the Resume button while playing
        playerName.gameObject.SetActive(false);
    }
}
