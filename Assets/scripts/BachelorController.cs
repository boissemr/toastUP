using UnityEngine;
using System.Collections;

public class BachelorController : MonoBehaviour {

	// parameters
	public bool AI;
	public float	cursorReach,
					cursorLerp;

	// variables
	NavMeshAgent agent;
    RoomController currentRoom;
	CursorController cursor;

	void Start() {

		// initalize compoents
		currentRoom = GameObject.Find("northRoom").GetComponent<RoomController>();
		agent = GetComponent<NavMeshAgent>();
		try { 
			cursor = GetComponentInChildren<CursorController>();
			Debug.Log(cursor.name);
		} catch {
			Debug.Log(transform.parent.gameObject.name + " cursor get component failed");
		}
	}

	void Update() {

		if(AI) {
			
			// AI controlled
			cursor.transform.localPosition = Vector3.Lerp(cursor.transform.localPosition, new Vector3(0, 1f, 0), cursorLerp * Time.deltaTime);
		} else {
			
			// human player controlled
			cursor.transform.localPosition = Vector3.Lerp(cursor.transform.localPosition, new Vector3(Input.GetAxis("Horizontal") * cursorReach, 1f, Input.GetAxis("Vertical") * cursorReach), cursorLerp * Time.deltaTime);
		}
	}

	// set this player's destination
	public void setDestination(RoomController o) {
		agent.SetDestination(o.transform.position);
        currentRoom = o;
	}

	// return this palyer's current room
    public RoomController getCurrentRoom() {
		return currentRoom;
	}
}
