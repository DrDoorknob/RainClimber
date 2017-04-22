using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropController : MonoBehaviour {

    public GameObject raindropTemplate;
    public Player player;

    List<Raindrop> drops = new List<Raindrop>(100);
    List<Raindrop> inactiveDrops = new List<Raindrop>(100);

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MakeRain()
    {
        Raindrop raindrop;
        if (inactiveDrops.Count != 0)
        {
            raindrop = inactiveDrops[0];
            inactiveDrops.Remove(raindrop);
            raindrop.gameObject.SetActive(true);
        }
        
    }

    void DeactivateDrop(Raindrop r)
    {

    }
}
