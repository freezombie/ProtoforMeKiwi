using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour 
{
	Transform target;
	TextMesh textMesh;
	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
			return;
		transform.LookAt (target.position);
		textMesh.text = Vector3.Distance (target.transform.position, transform.position).ToString();
	}

	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}
}
