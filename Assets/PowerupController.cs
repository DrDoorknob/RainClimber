using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    Jump = 0
};

public class PowerupController : MonoBehaviour {

    PowerupCounter[] counters;
    public static PowerupController Instance { get; private set; }

	// Use this for initialization
	void Start () {
        Instance = this;
        counters = GetComponentsInChildren<PowerupCounter>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Obtain(Powerup powerup, int quantity = 1)
    {
        counters[(int)powerup].Add(quantity);
    }

    public int GetNumberOwned(Powerup powerup)
    {
        return counters[(int)powerup].Quantity;
    }

    public bool HasAny(Powerup powerup)
    {
        return GetNumberOwned(powerup) != 0;
    }

    public void Use(Powerup powerup)
    {
        counters[(int)powerup].Use();
    }
}
