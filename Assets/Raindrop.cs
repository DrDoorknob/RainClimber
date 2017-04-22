using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {

    public Vector2 velocity;

    public static int idIncrement = 0;

    public int id;

	// Use this for initialization
	void Start () {
        id = idIncrement++;
        name = "Raindrop" + id;
	}
	
	// Update is called once per frame
	void Update () {
        var framevel = velocity * Time.deltaTime;
        var framevel3 = new Vector3(framevel.x, framevel.y, 0);
        transform.position = transform.position + framevel3;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p == null)
        {
            Debug.Log("Collided with a non-player rigidbody. ignoring.");
        }
        p.EnterDrop(this);
    }
}
