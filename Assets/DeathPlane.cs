using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour {

    public float minPlayerDistance;
    public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        pos.y = Mathf.Max(transform.position.y, player.transform.position.y - minPlayerDistance);
        transform.position = pos;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p == null)
        {
            Debug.Log("Collided with a non-player rigidbody. ignoring.");
        }
        p.Kill();
    }
}
