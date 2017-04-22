using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

    public Player player;
    Canvas canvas;
    bool isPaused;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
	}

    public void OpenMenu()
    {
        isPaused = true;
        Time.timeScale = 0;
        canvas.enabled = true;
        player.ControlsEnabled = false;
    }

    public void CloseMenu()
    {
        isPaused = false;
        Time.timeScale = 1;
        canvas.enabled = false;
        player.ControlsEnabled = true;
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
