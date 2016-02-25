using UnityEngine;
using System.Collections;

public class TitleScreenController : MonoBehaviour {

	// parameters
	public Sprite[] sprites;
	public float	secondsToWait,
					secondsToFlash,
					flashTimeVariance,
					defaultLightIntensity,
					lightIntensityVariance;
	public SpriteRenderer screen;
	public Light	screenLight;
	public Camera	cam;
	public Transform[] camWaypoints;
	public float	camLerp;

	// variables
	float	timer;
	int		currentCamWaypoint;

	void Start() {

		// initializations
		timer = secondsToWait;
		screenLight.intensity = defaultLightIntensity;
		screen.sprite = sprites[0];
		currentCamWaypoint = 0;
	}

	void Update() {

		// count down for screen flashing
		timer -= Time.deltaTime;

		// flash a new sprite after time
		if(timer <= 0) {

			// random sprite
			screen.sprite = sprites[Random.Range(0, sprites.Length)];

			// change light and timer according to whether or not the sprite is [0] (the non-glitchy image)
			if(screen.sprite == sprites[0]) {
				screenLight.intensity = defaultLightIntensity;
				timer += floatVariation(secondsToWait, flashTimeVariance);
			} else {
				screenLight.intensity = floatVariation(defaultLightIntensity, lightIntensityVariance);
				timer += floatVariation(secondsToFlash, flashTimeVariance);
			}
		}

		// intro camera movement
		if(currentCamWaypoint != camWaypoints.Length) {

			// move camera along waypoints
			cam.transform.position = Vector3.Lerp(cam.transform.position, camWaypoints[currentCamWaypoint].position, camLerp);
			cam.transform.LookAt(Vector3.zero);

			// advance waypoints when destination is reached
			if(Vector3.Distance(cam.transform.position, camWaypoints[currentCamWaypoint].position) < 0.5) {
				currentCamWaypoint++;
			}

			// skip intro camera movement with any input
			if(Input.anyKeyDown) {
				camLerp = .7f;
			}
		}
	}

	// returns a value between (n-v) and (n+v)
	float floatVariation(float n, float v) {
		return n - v + (v * Random.value * 2);
	}

	// button handlers
	void startButtonPressed() {
		Debug.Log("start button pressed");
	}
	void configureButtonPressed() {
		Debug.Log("config button pressed");
	}
	void exitButtonPressed() {
		Debug.Log("exit button pressed");

		// quit from editor
		UnityEditor.EditorApplication.isPlaying = false;

		// quit from build
		Application.Quit();
	}
}
