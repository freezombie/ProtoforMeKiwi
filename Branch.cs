using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour 
{
	Monkey monkeyOnBranch;
	bool occupied = false; // just for restrict 1 monkey per branch, don't want to think about offsetting positions and shit now.
	Transform[] waypoints = new Transform[5];
	Vector3 debugPointA;
	Vector3 debugPointB;
	Vector3 collisionPoint;
	Vector3 derivedPoint;
	Vector3 b;
	Vector3 c;
	bool doneOnce = false;
	public BananaBranch assignedBananaBranch;
	// Use this for initialization
	void Start () 
	{
		for (int i = 0; transform.childCount > i; i++) 
		{
			//Debug.Log ("adding " + transform.GetChild (i) + "to waypoints on " + this.transform.name);
			waypoints[i] = transform.GetChild(i);
			//Debug.Log(waypoints[i].name + " has sibling index of " + waypoints[i].GetSiblingIndex());
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(!debugPointA.Equals(Vector3.zero) && !debugPointB.Equals(Vector3.zero))
		{
			b = (debugPointB - debugPointA) / (debugPointB - debugPointA).magnitude;
			c = debugPointB - debugPointA;
			Debug.DrawRay(debugPointA,b,Color.red);
			Debug.DrawRay (debugPointA, c, Color.blue); // this is the correct one.
		}

		if (!collisionPoint.Equals(Vector3.zero)) {
			Debug.DrawRay (collisionPoint, Vector3.down, Color.black);
			if (!doneOnce) 
			{
				doneOnce = true;
				derivedPoint = FindPointBetweenWaypoints (c, collisionPoint);
			}
		}

		if (!derivedPoint.Equals(Vector3.zero)) {
			Debug.DrawRay (derivedPoint, Vector3.down, Color.white);
		}
	}

	Vector3 FindPointBetweenWaypoints(Vector3 directionVector, Vector3 collisionPoint)
	{
		//Debug.Log("distance between pointA and collisionpoint: " + Vector3.Distance(debugPointA,collisionPoint));
		float valueWithLowest = 0;
		for (float i = 0; i < 1; i = i + 0.01f) 
		{
			Vector3 testingPoint = debugPointA + (directionVector * i); // all guides say direction vector should be the normalized one, but then again they want it with concrete known distance
			Vector3 valueWithLowestPoint = debugPointA + (directionVector*valueWithLowest);
			if (Vector3.Distance (testingPoint, collisionPoint) < Vector3.Distance (valueWithLowestPoint, collisionPoint)) 
			{				
				valueWithLowest = i;
//				Debug.Log ("Assigned new valuewith Lowest: " + valueWithLowest + " distance was " + Vector3.Distance (directionVector * valueWithLowest,collisionPoint)); 
				//Debug.Log ("value with lowest: " + valueWithLowest);
				//Debug.Log ("Point A: " + debugPointA + " derivedPoint: " + (valueWithLowest * Vector3.Normalize((directionVector * valueWithLowest) - debugPointA) + debugPointA));
			} 
//			else 
//			{
//				Debug.Log("didn't assign new value with Lowest, existing distance is " + Vector3.Distance(directionVector * valueWithLowest,collisionPoint) + " tried point distance was " + Vector3.Distance(directionVector * i,collisionPoint)); 
//			}
		}
		doneOnce = true;
		//Debug.Log("value with lowest: " + valueWithLowest + " derivedpoint: " + (debugPointA + (directionVector*valueWithLowest)));
		Debug.DrawRay (debugPointA, (directionVector * valueWithLowest), Color.cyan, 60f);
		return (debugPointA + (directionVector*valueWithLowest));
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.collider.gameObject.name == "Monkey" && !occupied) 
		{
			occupied = true;
			monkeyOnBranch = col.collider.gameObject.GetComponent<Monkey>();
			Transform[] nearestWaypoints = FindNearestWaypoints (col.collider.transform.position); //monkeyOnBranch.Climb (monkeyOnBranch.transform.position, FindNearestWaypoints(monkeyOnBranch.transform.position).position.z);
			debugPointA = nearestWaypoints[0].position;
			debugPointB = nearestWaypoints [1].position;
			collisionPoint = col.contacts[0].point;
			monkeyOnBranch.Climb (FindPointBetweenWaypoints ((nearestWaypoints [1].position - nearestWaypoints [0].position), collisionPoint),this);
		}
	}

	public Transform[] FindNearestWaypoints(Vector3 pos)
	{
		Transform[] results = new Transform[2];
		foreach (Transform trans in waypoints) 
		{
			if (results [0] == null) 
			{
				results [0] = trans;
			} 
			else if (results [1] == null) 
			{
				results [1] = trans;
			} 
			else if (Vector3.Distance (pos,trans.position) < Vector3.Distance (pos,results [0].position)) 
			{
				if (Vector3.Distance (pos, results [0].position) < Vector3.Distance(pos,results [1].position)) 
				{
					results [1] = results [0];
				}
				results [0] = trans;
			}
			else if (Vector3.Distance (pos,trans.position) < Vector3.Distance (pos,results [1].position)) 
			{
				if (Vector3.Distance (pos, results [1].position) < Vector3.Distance (pos, results [0].position)) 
				{
					results [0] = results [1];
				}
				results [1] = trans;
			}
			/*if(results[0] != null && results[1]!=null)
				Debug.Log ("results[0]("+results[0].name+") - pos distance: " + Vector3.Distance (pos,results [0].position) + 
					" results[1]("+results[1].name+") - pos distance: " + Vector3.Distance (pos,results [1].position) 
				+ " " + trans.name + " - pos distance: " + Vector3.Distance (pos,trans.position));*/
		}
		for (int i = 0; i < results.Length; i++) {
			//Debug.Log ("results[" + i + "] has " + results [i].name);
		}
		return results;
	}

	public int AmountOfWaypointsBeforeGatherPoint(Vector3 pos)
	{
		Transform[] nearestWaypoints = FindNearestWaypoints (pos);
		if (nearestWaypoints [0].GetSiblingIndex () == 2 || nearestWaypoints [1].GetSiblingIndex () == 2) {
			return 0;
		} 
		else
			return 1; // At least I think that just 1 is the only other option; 
	}

	public Transform GetNextWaypoint(Vector3 pos) // there might be a problem here. the one where we are supposed to go next
	{											  // might not get found by findneareswaypoints when we are on a
		Transform[] nearestWaypoints = FindNearestWaypoints (pos); // waypoint
		int index;

		if (Mathf.Abs (nearestWaypoints [0].GetSiblingIndex () - 2) < Mathf.Abs (nearestWaypoints [1].GetSiblingIndex () - 2))
			index = 0;
		else
			index = 1;
		return nearestWaypoints [index].transform;

	}

	public TextMesh GetGatherText()
	{
		return transform.GetComponentInChildren<TextMesh> ();
	}
}
