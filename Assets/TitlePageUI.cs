using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePageUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// TODO: Remove "Quit" if on a phone
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    void Exit()
    {
        Application.Quit();
    }
}
