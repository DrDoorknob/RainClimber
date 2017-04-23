using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeightTracker : MonoBehaviour {

    float topPlayerHeight;
    int topPlayerHeightInt;
    Vector2 playerStartPos;
    public Transform player;
    public Text display;

    public float ftPerGameUnit;

	// Use this for initialization
	void Start () {
        playerStartPos = player.position;
        if (display == null)
        {
            display = GetComponent<Text>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        topPlayerHeight = Mathf.Max((player.position.y - playerStartPos.y) * ftPerGameUnit, topPlayerHeight);
        int playerHeightInt = Mathf.FloorToInt(topPlayerHeight);
        if (playerHeightInt > topPlayerHeightInt)
        {
            GainedAFoot();
            topPlayerHeightInt = playerHeightInt;
        }
	}

    void GainedAFoot()
    {
        display.text = topPlayerHeightInt.ToString();
        // maybe add like a particle effect or sound effect or something
    }
}
