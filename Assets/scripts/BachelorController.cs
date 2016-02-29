using UnityEngine;
using System.Collections;

public class BachelorController : MonoBehaviour {

	// parameters
	public bool			AI;
	public float		cursorReach,
						cursorLerp;

	// variables
	NavMeshAgent		agent;
	RoomController		currentRoom,
						destinationRoom,
						AIDestinationRoom;
	CursorController	cursor;

	void Start() {

		// initalize compoents
		currentRoom = GameObject.Find("northRoom").GetComponent<RoomController>();
		AIDestinationRoom = currentRoom;
		agent = GetComponent<NavMeshAgent>();
		cursor = GetComponentInChildren<CursorController>();
	}

	void Update() {

		if(AI) {
			// AI controlled
			cursor.transform.position = Vector3.Lerp(cursor.transform.position, AIDestinationRoom.transform.position, cursorLerp * Time.deltaTime);
		} else {
			// human player controlled
			cursor.transform.localPosition = Vector3.Lerp(cursor.transform.localPosition, new Vector3(Input.GetAxis("Horizontal") * cursorReach, 1f, Input.GetAxis("Vertical") * cursorReach), cursorLerp * Time.deltaTime);
		}

		// lock rotation
		transform.rotation = Quaternion.identity;
	}

	// set an AI player's destination (this is the thinky part!)
	public void setAIDestination() {

		// right now we just pick a random room
		RoomController[] rooms = getCurrentRoom().getAdjacentRooms(true);

		// set the destination to the room picked above
		AIDestinationRoom = rooms[Random.Range(0, rooms.Length)].GetComponent<RoomController>();
	}

	// go to this player's destination
	public void go() {

		// set destinationRoom to room that is nearest to cursor
		destinationRoom = GameObject.Find("god").GetComponent<God>().getNearestRoom(getCurrentRoom().getAdjacentRooms(true), cursor.transform.position);

		// move agent
		agent.SetDestination(destinationRoom.transform.position);
		currentRoom = destinationRoom;
	}

	// return this palyer's current room
    public RoomController getCurrentRoom() {
		return currentRoom;
	}
}
