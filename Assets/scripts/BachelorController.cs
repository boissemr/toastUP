using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BachelorController : MonoBehaviour {

	// parameters
	public bool			AI;
	public float		cursorReach,
						cursorLerp,
						toastSpawnSpacing,
						toasterForce;
	public GameObject	toastPrefab;
	public Vector3		toastSpawnPosition;
	public int			toasterCapacity;

	// variables
	NavMeshAgent		agent;
	RoomController		currentRoom,
						destinationRoom,
						AIDestinationRoom;
	CursorController	cursor;
	GameObject			toaster;
	Text				info;
	int					bread,
						toast,
						love;
	List<GameObject>	toastObjects;

	void Start() {

		// initalize compoents
		currentRoom = GameObject.Find("hall").GetComponent<RoomController>();
		AIDestinationRoom = currentRoom;
		agent = GetComponent<NavMeshAgent>();
		cursor = GetComponentInChildren<CursorController>();
		info = GetComponentInChildren<Text>();
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("toaster"))
			if(o.transform.IsChildOf(transform.parent))
				toaster = o;
		bread = 0;
		toast = 0;
		love = 0;
		toastObjects = new List<GameObject>();
	}

	void Update() {

		// AI cursor
		if(AI) {
			cursor.transform.position = Vector3.Lerp(cursor.transform.position, AIDestinationRoom.transform.position + Vector3.up, cursorLerp * Time.deltaTime);
		// human cursor
		} else {
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

		// increment bread if in bakery
		if(currentRoom.isBakery) {
			bread += currentRoom.loafSize;
		}

		// increment toast if in room
		if(currentRoom.isRoom && currentRoom.whoseRoom == this) {
			for(int i = 0; i < toasterCapacity; i++) {
				if(bread > 0) {

					// adjust values
					bread--;
					toast++;

					toastObjects.Add(toaster.GetComponent<Toaster>().makeToast(i));

					/*
					// instantiate toast object
					Quaternion tempRotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
					Vector3 tempPosition = toaster.transform.position + toastSpawnPosition + (Vector3.forward * i * toastSpawnSpacing) + (Vector3.back * toastSpawnSpacing / 2);
					toastObjects.Add((GameObject)Instantiate(toastPrefab, tempPosition, tempRotation));

					// apply force on toast so it pops up
					toastObjects[toastObjects.Count - 1].GetComponent<Rigidbody>().AddRelativeForce(tempRotation * Vector3.up * toasterForce);
					*/
				}
			}
		}

		// update info
		updateInfo();

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

	public void updateInfo() {
		info.text = bread + " / " + toast + " / " + love;
	}
}
