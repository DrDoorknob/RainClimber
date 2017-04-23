using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Player player;
    Animator anim;

    public Text gameOverClimbStats;
    public Text timeDisplay;
    public CanvasGroup gameOverCanvasGroup;
    public GameObject pauseFirstSelect;
    public EventSystem eventSystem;

    float gameStartTime;
    float topPlayerHeight;
    int topPlayerHeightInt;
    Vector2 playerStartPos;
    public Text heightIntDisplay;

    public float ftPerGameUnit;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        playerStartPos = player.transform.position;
        gameStartTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        var levelTime = Time.time - gameStartTime;
        var minutes = Mathf.Round(levelTime / 100f);
        var secs = Math.Round(levelTime % 60);
        timeDisplay.text = String.Format("{0}:{1}", minutes, secs);
        topPlayerHeight = Mathf.Max((player.transform.position.y - playerStartPos.y) * ftPerGameUnit, topPlayerHeight);
        int playerHeightInt = Mathf.FloorToInt(topPlayerHeight);
        if (playerHeightInt > topPlayerHeightInt)
        {
            GainedAFoot();
            topPlayerHeightInt = playerHeightInt;
        }

        if (Input.GetButtonDown("Pause") && !anim.GetBool("GameOver"))
        {
            if (anim.GetBool("Paused"))
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
        if (Input.anyKeyDown && gameOverCanvasGroup.interactable)
        {
            SceneManager.LoadScene(1);
        }
    }
    
    void GainedAFoot()
    {
        heightIntDisplay.text = topPlayerHeightInt.ToString();
        // maybe add like a particle effect or sound effect or something
    }

    public void OpenMenu()
    {
        anim.SetBool("Paused", true);
        Time.timeScale = 0;
        player.ControlsEnabled = false;
        StartCoroutine("highlightBtn");
    }

    IEnumerator highlightBtn()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(pauseFirstSelect);
    }

    public void CloseMenu()
    {
        anim.SetBool("Paused", false);
        Time.timeScale = 1;
        Input.ResetInputAxes();

        player.ControlsEnabled = true;
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        var lastBest = PlayerPrefs.GetFloat("BestHeight", -1f);
        topPlayerHeight = (float)System.Math.Round((Double)topPlayerHeight, 2);
        if (topPlayerHeight > lastBest)
        {
            PlayerPrefs.SetFloat("BestHeight", topPlayerHeight);
        }
        if (lastBest > 0f)
        {
            gameOverClimbStats.text = String.Format("You climbed {0} feet - your previous best was {1}", topPlayerHeight, lastBest);
        }
        else
        {
            gameOverClimbStats.text = String.Format("You climbed {0} feet!", topPlayerHeight);
        }
        anim.SetBool("GameOver", true);
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
