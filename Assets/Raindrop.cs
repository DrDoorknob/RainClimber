using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {

    public Vector2 velocity;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        var framevel = velocity * Time.deltaTime;
        var framevel3 = new Vector3(framevel.x, framevel.y, 0);
        transform.position = transform.position + framevel3;
	}
}
