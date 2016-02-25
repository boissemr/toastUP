using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {

	public BachelorController bachelor;

    List<RoomController> adjacentRooms;

	void Start() {

		GetComponent<MeshRenderer>().enabled = false;

        adjacentRooms = new List<RoomController>();
	}

	void OnTriggerEnter(Collider c) {

		if(c.gameObject.CompareTag("room")) {
            adjacentRooms.Add(c.gameObject.GetComponent<RoomController>());
		}
	}

	void OnMouseDown() {

		bachelor.setDestination(this);

        foreach(RoomController o in adjacentRooms) {
            Debug.Log(o.gameObject.name);
		}
	}

	void OnMouseOver() {

        foreach(RoomController o in adjacentRooms) {
			Debug.DrawLine(transform.position, o.transform.position);
		}
	}

	public RoomController[] getAdjacentRooms(bool includeSelf) {

		if(includeSelf) {
			adjacentRooms.Add(this);
			RoomController[] returnMe = adjacentRooms.ToArray();
			adjacentRooms.Remove(this);
			return returnMe;
		}

		return adjacentRooms.ToArray();
	}
}
