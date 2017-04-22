using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float dropPullStrength;

    public Camera cam;

    Vector2 lastMousePosition = new Vector2(0f, -10f);
    bool isUsingController;
    public bool ControlsEnabled { get; set; }

    public Raindrop swimmingDrop;
    public ProjectilePredictionDraw leapGuide;
    public Vector2 leapAimPosition;
    public Transform debugAimPosition;
    public float leapStrength;

    bool dead;


    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        leapGuide.playerRef = this;
        ControlsEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (swimmingDrop != null)
        {
            leapGuide.originPoint = transform.position;
            Vector2 toDrop = swimmingDrop.transform.position - transform.position;
            float toDropMagnitude = toDrop.magnitude;
            //float pullForceScale = swimmingDrop.GetComponent<CircleCollider2D>().radius - toDropMagnitude;
            body.velocity = toDrop * dropPullStrength;
        }
        else
        {
        }
        if (ControlsEnabled)
        {
            ReadControls();
        }
        else if (dead)
        {

        }
        leapGuide.aimPoint = leapAimPosition;

    }

    public void Kill()
    {
        dead = true;
        cam.GetComponent<CameraFollower2D>().target = null;
        leapGuide.gameObject.SetActive(false);
    }

    private void ReadControls()
    {

        Vector2 mousePosition = Input.mousePosition;
        if (dead)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(1);
            }
        }

        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");

        if (mousePosition != lastMousePosition)
        {
            isUsingController = false;
        }
        if (xAxis != 0f || yAxis != 0f)
        {
            isUsingController = true;
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

        if (swimmingDrop != null)
        {

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
        leapGuide.gameObject.SetActive(true);
    }

    public void LeapFromRaindrop()
    {
        Debug.Log("Leap!");
        body.gravityScale = 1f;
        swimmingDrop = null;
        Vector2 leapDirection = leapAimPosition - (Vector2)transform.position;
        leapDirection.Normalize();
        body.AddForce(leapDirection * leapStrength, ForceMode2D.Impulse);
        leapGuide.gameObject.SetActive(false);
    }
}
