using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

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
	float	currentCamLerp;
	DepthOfField dof;

	void Start() {

		// initializations
		timer = secondsToWait;
		screenLight.intensity = defaultLightIntensity;
		screen.sprite = sprites[0];
		cam.transform.position = camWaypoints[0].position;
		cam.transform.rotation = camWaypoints[0].rotation;
		currentCamWaypoint = 1;
		currentCamLerp = camLerp;
		dof = cam.GetComponent<DepthOfField>();
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

		// camera movement
		if(Vector3.Distance(cam.transform.position, camWaypoints[currentCamWaypoint].position) > 0.5) {

			// move camera along waypoints
			cam.transform.position = Vector3.Lerp(cam.transform.position, camWaypoints[currentCamWaypoint].position, currentCamLerp * Time.deltaTime);
			cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camWaypoints[currentCamWaypoint].rotation, currentCamLerp * Time.deltaTime);
			dof.focalTransform = camWaypoints[currentCamWaypoint].GetComponentInChildren<FocusOn>().transform;
			dof.focalSize = Mathf.Lerp(dof.focalSize, Vector3.Distance(dof.focalTransform.position, cam.transform.position) / 100, currentCamLerp * Time.deltaTime);

			// skip camera movement with any input
			if(Input.anyKeyDown) {
				currentCamLerp *= 10;
			}
		} else {

			// reset lerp speed
			currentCamLerp = camLerp;
		}
	}

	// returns a value between (n-v) and (n+v)
	float floatVariation(float n, float v) {
		return n - v + (v * Random.value * 2);
	}

	// button handlers
	void startButtonPressed() {
		Debug.Log("start button pressed, moving to map select");
		currentCamWaypoint = 2;
	}
	void configureButtonPressed() {
		Debug.Log("config button pressed");
	}
	void exitButtonPressed() {
		Debug.Log("exit button pressed");

		// quit from editor
		// this prevents the game from making a build for some reason
		//UnityEditor.EditorApplication.isPlaying = false;

		// quit from build
		Application.Quit();
	}
	void mapSelected() {
		var map = 0;
		Debug.Log("map #" + map + " selected");
		currentCamWaypoint = 3;
	}
	void characterSelected() {
		var character = 0;
		Debug.Log("character #" + character + " selected");
		Application.LoadLevel("map0");
	}
	void backButtonPressed() {
		currentCamWaypoint -= 1;
	}
}
