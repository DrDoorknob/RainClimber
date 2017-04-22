using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropController : MonoBehaviour {

    public GameObject raindropTemplate;
    public Player player;
    public BoxCollider2D playAreaBox;

    public GameObjectPool<Raindrop> dropPool;

    // Use this for initialization
    void Start () {
        dropPool = new GameObjectPool<Raindrop>(20, raindropTemplate);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
