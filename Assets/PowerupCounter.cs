using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupCounter : MonoBehaviour {

    CanvasGroup canvasGroup;

    public Text quantityTextDisplay;

    public int Quantity { get; private set; }

	// Use this for initialization
	void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Add(int quantity)
    {
        canvasGroup.alpha = 1;
        Quantity = Quantity + quantity;
        UpdateNumber();
    }

    private void UpdateNumber()
    {
        if (Quantity == 0)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            quantityTextDisplay.text = Quantity > 0 ? Quantity.ToString() : "";
        }
    }

    public void Use()
    {
        Quantity = Quantity - 1;
        UpdateNumber();
    }
}
