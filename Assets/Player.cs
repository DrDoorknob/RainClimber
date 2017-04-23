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

    public Animator playerAnimator;
    public AudioShuffleDeck dropletSoundEmitter;
    public GameUI gameUI;
    public SpriteRenderer sprite;
    public Raindrop swimmingDrop;
    public ProjectilePredictionDraw leapGuide;
    public Vector2 leapAimPosition;
    public Transform leapOrientation;
    public Transform debugAimPosition;
    public float leapStrength;
    public float thumbstickTurnSensitivity;
    public float thumbstickTriggerScale;

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
            Vector2 toDrop = swimmingDrop.transform.position - transform.position;
            float toDropMagnitude = toDrop.magnitude;
            //float pullForceScale = swimmingDrop.GetComponent<CircleCollider2D>().radius - toDropMagnitude;
            body.velocity = toDrop * dropPullStrength;
        }
        else
        {
            Vector2 rotateDirection = body.velocity.normalized;
            sprite.flipX = rotateDirection.x < 0;
            sprite.transform.rotation = Quaternion.FromToRotation(Vector3.up, rotateDirection);
        }
        if (ControlsEnabled)
        {
            ReadControls();
        }
        leapGuide.aimDirection = leapOrientation.forward;

    }

    public void Kill()
    {
        dead = true;
        cam.GetComponent<CameraFollower2D>().target = null;
        leapGuide.gameObject.SetActive(false);
        gameUI.GameOver();
    }

    private void ReadControls()
    {

        Vector2 mousePosition = Input.mousePosition;

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
        if (isUsingController)
        {
            var turnAmt = thumbstickTurnSensitivity;
            if (Input.GetAxis("FastTurn") < -.5f)
            {
                turnAmt *= thumbstickTriggerScale;
            }
            leapOrientation.Rotate(new Vector3(0f, 0f, 1f), -xAxis * Time.deltaTime * turnAmt, Space.World);
        }
        else {
            Vector3 mouseWorldPos = mousePosition;
            mouseWorldPos.z = transform.position.z - cam.transform.position.z;
            leapAimPosition = cam.ScreenToWorldPoint(mouseWorldPos);
            leapOrientation.rotation = Quaternion.LookRotation((leapAimPosition - (Vector2)transform.position));
        }
        lastMousePosition = mousePosition;
        if (debugAimPosition != null)
        {
            debugAimPosition.transform.position = leapAimPosition;
        }

        if (swimmingDrop != null)
        {
            if (Input.GetButtonDown("Jump"))
            {
                LeapFromRaindrop();
            }
        }
    }

    private static Vector2 AngleToUnitVector(float angle)
    {
        // https://forum.unity3d.com/threads/c-getting-a-vector-2-from-an-angle.273833/
        angle *= Mathf.Deg2Rad;
        var ca = Mathf.Cos(angle);
        var sa = Mathf.Sin(angle);
        var rx = -1 * sa;
        var ry = ca;

        return new Vector2(rx, ry);
    }

    public void EnterDrop(Raindrop r)
    {
        
        if (swimmingDrop != null || dead)
        {
            return; // ignore
        }
        Debug.Log("Entered raindrop");
        body.gravityScale = 0f;
        swimmingDrop = r;
        playerAnimator.SetBool("InWater", true);
        dropletSoundEmitter.PlayOne();
        leapGuide.gameObject.SetActive(true);
        sprite.transform.rotation = Quaternion.identity;
    }

    public void LeapFromRaindrop()
    {
        Debug.Log("Leap!");
        body.gravityScale = 1f;
        swimmingDrop = null;
        playerAnimator.SetBool("InWater", false);
        dropletSoundEmitter.PlayOne();
        Vector2 leapDirection = leapOrientation.forward;
        body.AddForce(leapDirection * leapStrength, ForceMode2D.Impulse);
        leapGuide.gameObject.SetActive(false);
    }
}
