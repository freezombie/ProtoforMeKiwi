using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP : MonoBehaviour
{
    HingeJoint hj;
    float min=0;
    float max=0; 
	// Use this for initialization
	void Start ()
    {
        hj = GetComponent<HingeJoint>();
        min = hj.angle;
        max = hj.angle;
        Debug.Log("Startup angle set to " + hj.angle);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(hj.angle>max)
        {
            max = hj.angle;
            Debug.Log("Max set to: " + max);
        }
        if(hj.angle<min)
        {
            min = hj.angle;
            Debug.Log("Min set to: " + min);
        }
	}
}
