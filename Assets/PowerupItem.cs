using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupItem : MonoBehaviour {

    Powerup powerup;

	// Use this for initialization
	void Start () {
        powerup = Powerup.Jump;
	}
	
	// Update is called once per frame
	void Update () {		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if (p == null)
        {
            Debug.Log("Collided with a non-player rigidbody. ignoring.");
            return;
        }
        PowerupController.Instance.Obtain(powerup);
        Destroy(gameObject);
    }

}
