using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float dropPullStrength;

    public Raindrop swimmingDrop;
    


    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (swimmingDrop != null)
        {
            Vector2 toDrop = swimmingDrop.transform.position - transform.position;
            Vector2.ClampMagnitude(toDrop, dropPullStrength);
            body.AddForce(toDrop);
        }

        
	}

    public void EnterDrop(Raindrop r)
    {
        body.gravityScale = 0f;
        swimmingDrop = r;
    }

    public void LeapFromRaindrop()
    {
        body.gravityScale = 1f;
    }
}
