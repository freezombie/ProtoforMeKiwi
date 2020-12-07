using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour 
{
	enum State {InCage,Climbing,Moving,Gathering, PickedUp, Idle};
	State state = State.InCage;
	public float climbTime = 1f;
	public float moveTime = 1f;
	public float gatherTime = 1f;
	float buffer = 0f;
	Vector3 startPos;
	Vector3 endPos;
	Vector3 branchEndPoint; // this is so we don't have to get the vector3 again from branch where the middle of the branch is, even if it's a bit ugly.
	Quaternion startRot;
	Quaternion endRot;
	Branch branch;
	BananaTree bananaTree;
	List<MoveCommand> moveCommands = new List<MoveCommand>();
	TextMesh gatherText;
	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		switch (state) 
		{
		case State.Climbing:
			buffer += Time.deltaTime;
			transform.position = Vector3.Lerp (startPos, endPos, buffer / climbTime);
			//Debug.Log (Vector3.Distance (transform.position, endPos));
			if (buffer >= climbTime) 
			{
				state = State.Moving;
				moveCommands.Add(new MoveCommand(transform.position,branchEndPoint,transform.rotation,transform.rotation,0.2f));
				int amountofWaypointsToGo = branch.AmountOfWaypointsBeforeGatherPoint (transform.position);
				Vector3 pointToProcess = transform.position;
				Quaternion rotationToProcess = transform.rotation;
				while (amountofWaypointsToGo >= 0) 
				{
					amountofWaypointsToGo--;
					Transform nextWaypoint = branch.GetNextWaypoint (pointToProcess);
					moveCommands.Add(new MoveCommand(pointToProcess,nextWaypoint.position,rotationToProcess,nextWaypoint.rotation,moveTime));
					pointToProcess = nextWaypoint.position;
					rotationToProcess = nextWaypoint.rotation;
				}
				buffer = 0;
			}
			break;
		case State.Moving:
			/*Debug.Log("MoveCommands for " + transform.name + " on " + branch.transform.name);
			int i=1;
			foreach(MoveCommand mc in moveCommands)
			{
				Debug.Log(i + ": StartPosition" + mc.startPosition + " endPosition " + mc.endPosition + " startRotation " + mc.startRotation + " endRotation " + mc.endRotation + " moveTime " + mc.moveTime); 
				i++;
			}*/
			buffer += Time.deltaTime;
			transform.position = Vector3.Lerp (moveCommands [0].startPosition, moveCommands [0].endPosition, buffer / moveCommands [0].moveTime);
			transform.rotation = Quaternion.Lerp (moveCommands [0].startRotation, moveCommands [0].endRotation, buffer / moveCommands [0].moveTime);
			if (buffer >= moveCommands [0].moveTime) 
			{
				buffer = 0;
				moveCommands.RemoveAt (0);
				if (moveCommands.Count == 0) 
				{					
					state = State.Gathering;
					gatherText = branch.GetGatherText ();
				}
			}
			break;
		case State.Gathering:
			buffer += Time.deltaTime;
			gatherText.text = (gatherTime - buffer).ToString ("0.00");
			if (buffer >= gatherTime) 
			{
				buffer = 0;
				if (branch.assignedBananaBranch != null && branch.assignedBananaBranch.GetAmountOfBananas() > 0)
					bananaTree.DropABanana (branch.assignedBananaBranch);
				else 
				{
					state = State.Idle;
				}
			}
			break;
		default:
			break;
		}
	}

	public void Climb(Vector3 endPoint, Branch branch)
	{
		this.branch = branch;
		bananaTree = branch.transform.parent.GetComponentInChildren<BananaTree> ();
		state = State.Climbing;
		transform.rotation = Quaternion.Euler(-90f,0,0);
		transform.LookAt (endPoint);
		startPos = transform.position;
		endPos = new Vector3 (startPos.x, endPoint.y,startPos.z );
		branchEndPoint = endPoint;
		buffer = 0f;
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<Collider> ().isTrigger = true;
	}
}

public class MoveCommand
{
	public Vector3 startPosition;
	public Vector3 endPosition;
	public Quaternion startRotation;
	public Quaternion endRotation;
	public float moveTime;

	public MoveCommand(Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation, float moveTime)
	{
		this.startPosition = startPosition;
		this.endPosition = endPosition;
		this.startRotation = startRotation;
		this.endRotation = endRotation;
		this.moveTime = moveTime;
	}
}
