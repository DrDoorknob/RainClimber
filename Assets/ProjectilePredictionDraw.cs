using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePredictionDraw : MonoBehaviour {

    public Vector3 originPoint;
    public Vector3 aimPoint;
    LineRenderer lineRenderer;

    public int numPoints;

    Vector3[] points;

    public Player playerRef;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        points = new Vector3[numPoints];
	}

    const float TIME_EST_PER_VERTEX = 0.1f;

	// Update is called once per frame
	void Update () {
        Vector2 position = originPoint;
        Vector2 velocity = aimPoint - originPoint;
        velocity.Normalize();
        velocity *= playerRef.leapStrength;
        
        for (var pointidx = 0; pointidx < numPoints; pointidx++)
        {
            velocity += (Physics2D.gravity * TIME_EST_PER_VERTEX);
            position += (velocity * TIME_EST_PER_VERTEX);
            points[pointidx] = position;
        }
        lineRenderer.SetPositions(points);
	}
}
