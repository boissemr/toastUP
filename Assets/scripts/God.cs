using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class God : MonoBehaviour {

	// parameters
	public float		beatsPerMinute;
	public int			phaseLength,
						beatsPerAction;
	public Text			UI_phase;
	public Image		progressBar;
	public AudioClip[]	jams;

	// varialbes
	int					phase;
	float				beats,
						seconds,
						secondsPerBeat;
	Vector3				progressBarScale;
	AudioClip			track;
	AudioSource			source;

	void Start() {

		// components
		source = GetComponent<AudioSource>();

		// initialize
		phase = 0;
		beats = 0;
		seconds = 0;
		track = null;
		progressBarScale = progressBar.transform.localScale;

		// calculate seconds/beat from beats/minute
		secondsPerBeat = 1 / (beatsPerMinute / 60);
	}

	void Update() {

		// increase real time counter
		seconds += Time.deltaTime;

		while(seconds > secondsPerBeat) {

			// increase beat counter
			seconds -= secondsPerBeat;
			beats += 1;

			// take action every x beats
			if(beats % beatsPerAction == 0) {
				takeAction();
			}

			// increase phase counter and change song
			if(phase == 0 || beats == phaseLength * 32) {
				phase += 1;
				beats = 0;
				source.Stop();
				track = jams[Random.Range(0, jams.Length)];
				source.PlayOneShot(track);
			}

			// update UI
			UI_phase.text = track.name;
			progressBar.transform.localScale = new Vector3(progressBarScale.x * (beats / (phaseLength * 32)), progressBarScale.y, progressBarScale.z);
		}
	}

	void takeAction() {
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("player")) {

			BachelorController b = o.GetComponentInChildren<BachelorController>();

			if(b.AI) {
				b.setAIDestination();
			}

			b.go();
		}
	}

	public RoomController getNearestRoom(RoomController[] rooms, Vector3 position) {
		RoomController nearestRoom = rooms[0];
		for(int i = 1; i < rooms.Length; i++) {
			if(Vector3.Distance(rooms[i].transform.position, position) < Vector3.Distance(nearestRoom.transform.position, position)) {
				nearestRoom = rooms[i];
			}
		}
		return nearestRoom;
	}
}
