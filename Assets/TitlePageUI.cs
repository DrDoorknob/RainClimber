using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePageUI : MonoBehaviour {

    public Animator screenAnimator;
    public Image fadeOverPanel;

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

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
