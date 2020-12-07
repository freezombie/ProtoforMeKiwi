using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    int index;
    Rigidbody rb;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetIndex(int indexNumber)
    {
        index = indexNumber;
        if (gameObject.GetComponent("Rigidbody") != null)
            rb = gameObject.GetComponent<Rigidbody>();
        else
        { 
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            this.gameObject.layer = 8;
        }
    }

    public void Drop()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        //rb kinematic = false; 
    }
}
