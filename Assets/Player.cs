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
    public float leapStrengthFromDrop;
    public float leapStrengthPowerup;
    public float thumbstickTurnSensitivity;
    public float thumbstickTriggerScale;

    public PowerupController powerups;

    bool hasDoubleJump = true;

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

        
        if (Input.GetButtonDown("Jump"))
        {
            if (swimmingDrop != null)
            {
                LeapFromRaindrop();
            }
            else if (powerups.HasAny(Powerup.Jump) && hasDoubleJump)
            {
                LeapFromMidair();
            }
        }
    }

    public void EnterDrop(Raindrop r)
    {
        
        if (swimmingDrop != null || dead)
        {
            return; // ignore
        }
        hasDoubleJump = true;
        Debug.Log("Entered raindrop");
        body.gravityScale = 0f;
        swimmingDrop = r;
        playerAnimator.SetBool("InWater", true);
        dropletSoundEmitter.PlayOne();
        leapGuide.impulseSpeed = leapStrengthFromDrop;
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
        body.AddForce(leapDirection * leapStrengthFromDrop, ForceMode2D.Impulse);
        if (!powerups.HasAny(Powerup.Jump))
        {
            leapGuide.gameObject.SetActive(false);
        }
        else
        {
            leapGuide.impulseSpeed = leapStrengthPowerup;
        }
    }

    public void LeapFromMidair()
    {
        powerups.Use(Powerup.Jump);
        Debug.Log("Midair leap");
        body.AddForce((body.velocity * -1f) + ((Vector2)leapOrientation.forward * leapStrengthPowerup), ForceMode2D.Impulse);
        hasDoubleJump = false;
        leapGuide.gameObject.SetActive(false);
    }
}
