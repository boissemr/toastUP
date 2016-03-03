using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {

	public bool isBakery;
	public bool isRoom;
	public BachelorController whoseRoom;
	public int loafSize;

    List<RoomController> adjacentRooms;

	void Start() {

		// hide the room Collider when game starts
		GetComponent<MeshRenderer>().enabled = false;

		// initialize
        adjacentRooms = new List<RoomController>();
	}

	void OnTriggerEnter(Collider c) {

		// find adjacentRooms rooms
		if(c.gameObject.CompareTag("room")) {
			adjacentRooms.Add(c.gameObject.GetComponent<RoomController>());
		}
	}

	public RoomController[] getAdjacentRooms(bool includeSelf) {

		// return adjacent rooms and this room
		if(includeSelf) {
			adjacentRooms.Add(this);
			RoomController[] returnMe = adjacentRooms.ToArray();
			adjacentRooms.Remove(this);
			return returnMe;
		}

		// returnMe adjacent rooms
		return adjacentRooms.ToArray();
	}
}
