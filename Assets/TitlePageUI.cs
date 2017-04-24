using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePageUI : MonoBehaviour {

    public Animator screenAnimator;
    public Image fadeOverPanel;

    public GameObject[] pages;
    public EventSystem eventSystem;

    public float fadeInTime;

	// Use this for initialization
	void Start () {
		// TODO: Remove "Quit" if on a phone
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time < fadeInTime) {
            var color = fadeOverPanel.color;
            color.a = 1f - (Time.time / fadeInTime);
            fadeOverPanel.color = color;
        }
        else if (fadeOverPanel.color.a != 0f)
        {
            fadeOverPanel.color = new Color(0f, 0f, 0f, 0f);
        }
	}

    public void SetPageNumber(int pageN)
    {
        screenAnimator.SetInteger("Page", pageN);
        var buttons = pages[pageN].GetComponentsInChildren<Button>();
        if (buttons.Length > 0)
        {
            eventSystem.SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    public void NewGame()
    {
        Debug.Log("Starting the game");
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
}
