using UnityEngine;
using System.Collections;

public class BachelorController : MonoBehaviour {

	NavMeshAgent agent;

    RoomController currentRoom;

	void Start() {

		currentRoom = GameObject.Find("northRoom").GetComponent<RoomController>();
		agent = GetComponent<NavMeshAgent>();
	}

    public void setDestination(RoomController o) {
		
		agent.SetDestination(o.transform.position);
        currentRoom = o;
	}

    public RoomController getCurrentRoom() {

		return currentRoom;
	}
}
