using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower2D : MonoBehaviour {

    public GameObject target;
    public Vector2 offsetFromCenter = Vector2.zero;

	// Use this for initialization
	void Start () {
        var origin = targetPos();
        transform.position = new Vector3(origin.x, origin.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null) {
            var target2d = Vector2.Lerp(transform.position, targetPos(), Time.deltaTime * 8f);
            transform.position = new Vector3(target2d.x, target2d.y, transform.position.z);
        }
	}

    Vector2 targetPos()
    {
        return ((Vector2)target.transform.position) + offsetFromCenter;
    }
}
