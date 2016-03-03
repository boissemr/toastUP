using UnityEngine;
using System.Collections;

public class Toaster : MonoBehaviour {

	public float	popUpForce,
					spawnSpacing;
	public Vector3	spawnPosition;
	public GameObject toastPrefab;

	public GameObject makeToast(int index) {

		Quaternion tempRotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
		Vector3 tempPosition = transform.position + spawnPosition + (Vector3.forward * index * spawnSpacing) + (Vector3.back * spawnSpacing / 2);

		// make toast
		GameObject toast = (GameObject)Instantiate(toastPrefab, tempPosition, tempRotation);

		// pop up
		toast.GetComponent<Rigidbody>().AddRelativeForce(tempRotation * Vector3.up * popUpForce);

		// return to player
		return toast;
	}
}
