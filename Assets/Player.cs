using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float dropPullStrength;

    public Camera cam;

    Vector2 lastMousePosition = new Vector2(0f, -10f);
    bool isUsingController;

    public Raindrop swimmingDrop;
    public Vector2 leapAimPosition;
    public Transform debugAimPosition;
    public float leapStrength;


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
            float toDropMagnitude = toDrop.magnitude;
            //float pullForceScale = swimmingDrop.GetComponent<CircleCollider2D>().radius - toDropMagnitude;
            body.velocity = toDrop * dropPullStrength;
        }
        else
        {
        }
        ReadControls();

    }

    public void Kill()
    {
        cam.GetComponent<CameraFollower2D>().target = null;
    }

    private void ReadControls()
    {

        Vector2 mousePosition = Input.mousePosition;

        if (swimmingDrop != null)
        {
            if (mousePosition != lastMousePosition)
            {
                isUsingController = false;
            }
            if (!isUsingController)
            {
                Vector3 mouseWorldPos = mousePosition;
                mouseWorldPos.z = transform.position.z - cam.transform.position.z;
                leapAimPosition = cam.ScreenToWorldPoint(mouseWorldPos);
            }
            if (debugAimPosition != null)
            {
                debugAimPosition.transform.position = leapAimPosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                LeapFromRaindrop();
            }
        }
    }

    public void EnterDrop(Raindrop r)
    {
        
        if (swimmingDrop != null)
        {
            return; // ignore
        }
        Debug.Log("Entered raindrop");
        body.gravityScale = 0f;
        swimmingDrop = r;
    }

    public void LeapFromRaindrop()
    {
        Debug.Log("Leap!");
        body.gravityScale = 1f;
        //leapedFrom = swimmingDrop;
        swimmingDrop = null;
        Vector2 leapDirection = leapAimPosition - (Vector2)transform.position;
        leapDirection = Vector2.ClampMagnitude(leapDirection, 1f);
        //body.velocity = leapDirection * leapStrength;
        body.AddForce(leapDirection * leapStrength, ForceMode2D.Impulse);
    }
}
