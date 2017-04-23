using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropController : MonoBehaviour {

    public GameObject raindropTemplate;
    public Player player;
    public BoxCollider2D playAreaBox;
    public Camera cam;
    public Transform deathPlane;

    public GameObjectPool<Raindrop> dropPool;

    public int minRaindropsAbovePlayer;
    public float minRaindropSpeed;
    public float maxRaindropSpeed;
    public Vector2 spawnAreaSize;
    public float optimalJumpDistance;


    // Use this for initialization
    void Start () {
        Raindrop.idIncrement = 0;
        dropPool = new GameObjectPool<Raindrop>(20, raindropTemplate);
        Vector2 firstDrop = player.transform.position;
        firstDrop += 2 * Vector2.down;
        SpawnRaindropAt(firstDrop);
        //firstDrop.x += 1;
        //float scoreDebug = ScoreSpawnPosition(firstDrop);
        var firstSpawnArea = new Rect((Vector2)player.transform.position - (spawnAreaSize * .5f), spawnAreaSize);
        for (var i = 0; i < minRaindropsAbovePlayer; i++)
        {
            SpawnRaindropByRect(firstSpawnArea);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Raindrop dying = null;
        foreach (var r in dropPool.Items)
        {
            if (r.transform.position.y - deathPlane.position.y < -3f)
            {
                dying = r;
                break;
            }
        }
        if (dying != null)
        {
            dropPool.MakeInactive(dying);
        }
        int abovePlayer = NumRaindropsAbovePosition(player.transform.position.y);

        if (abovePlayer < minRaindropsAbovePlayer)
        {
            SpawnRaindrops(minRaindropsAbovePlayer - abovePlayer);
        }
	}

    private Raindrop SpawnRaindropAt(Vector2 v)
    {
        float randomSpeed = Random.Range(minRaindropSpeed, maxRaindropSpeed);
        return SpawnRaindropAt(v, randomSpeed);
    }

    private Raindrop SpawnRaindropAt(Vector2 v, float speed)
    {
        Vector3 v3 = v;
        Raindrop r = dropPool.Make(v3);
        r.velocity = new Vector2(0, -1 * speed);
        return r;
    }

    static readonly int LOCATION_CHOICE_SEED_SIZE = 50;

    private Rect CameraRect()
    {
        Vector2 camAreaLeftCorner = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 10f));
        Vector2 camAreaRightCorner = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 10f));
        Rect r = new Rect(camAreaLeftCorner, camAreaRightCorner - camAreaLeftCorner);
        return r;
    }

    private void SpawnRaindrops(int quantity = 1)
    {
        Rect camRect = CameraRect();
        float camAreaWidth = camRect.width;
        int dropsAboveCamera = NumRaindropsAbovePosition(camRect.yMax);
        //Vector2 spawnAreaLeft = camAreaLeftCorner;
        var spawnArea = new Rect(
            camRect.center.x - spawnAreaSize.x * .5f,
            camRect.yMax + 1f,
            spawnAreaSize.x,
            spawnAreaSize.y);

        // If there are no raindrops above the camera, quick-spawn one close to the bottom that's a bit faster-moving
        for (var i = 0; i < quantity; i++)
        {
            if (dropsAboveCamera == 0)
            {
                float xPos = Random.value;
                Vector2 spawnPoint = cam.ViewportToWorldPoint(new Vector3(xPos, 1f, 10f));
                spawnPoint += Vector2.up * Random.Range(1f, 12f);
                float randomSpeed = Random.Range(minRaindropSpeed, maxRaindropSpeed);
                SpawnRaindropAt(spawnPoint, randomSpeed);
            }
            else
            {
                SpawnRaindropByRect(spawnArea);
            }
        }

    }

    private void SpawnRaindropByRect(Rect spawnArea)
    {
        // Generate 50 random locations, then score them
        var locationChoices = new Vector3[LOCATION_CHOICE_SEED_SIZE];
        float totalScore = 0f;
        for (var i = 0; i < LOCATION_CHOICE_SEED_SIZE; i++)
        {
            // the third part of the vector will be the scoring for its usage
            locationChoices[i] = new Vector3(
                spawnArea.x + (Random.value * spawnArea.width),
                spawnArea.y + (Random.value * spawnArea.height),
                0);
            locationChoices[i].z = ScoreSpawnPosition(locationChoices[i]);
            totalScore += locationChoices[i].z;
        }
        if (totalScore == 0f)
        {
            // area is flooded - no spawn
            return;
        }
        float pickedScore = Random.Range(0f, totalScore);
        Vector2 finalPosition = locationChoices[LOCATION_CHOICE_SEED_SIZE - 1];
        foreach (var choice in locationChoices)
        {
            pickedScore -= choice.z;
            if (pickedScore <= 0f)
            {
                finalPosition = choice;
            }
        }

        float randomSpeed = Random.Range(minRaindropSpeed, maxRaindropSpeed);

        SpawnRaindropAt(finalPosition, randomSpeed);

    }

    const float DISTANCE_AMP = 1.5f;

    float ScoreSpawnPosition(Vector2 pos)
    {
        float score = 1f;

        // Goal is to score the position high if it's in perfect jumping distance from another raindrop, low if it's very close or far from all raindrops,
        // and zero if it intersects another raindrop.

        foreach (var r in dropPool.Items)
        {
            var distance = Vector2.Distance(pos, r.transform.position);
            if (distance <= (2f * r.GetComponent<CircleCollider2D>().radius))
            {
                return 0f;
            }
            // Score on a parabola, in format y = ax^2 + bx + c
            // a is "DISTANCE_AMP"
            var b = 2 * DISTANCE_AMP * optimalJumpDistance;
            var y = Mathf.Max(20f, DISTANCE_AMP * Mathf.Pow(distance, 2) + b * distance + 8);
            score += y;
        }

        return score;
    }

    int NumRaindropsAbovePosition(float y)
    {
        int n = 0;
        foreach (var r in dropPool.Items) {
            if (player.swimmingDrop != r && r.transform.position.y > y)
            {
                n++;
            }
        }
        return n;
    }
}
