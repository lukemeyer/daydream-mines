using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gullFlight : MonoBehaviour {

	private Vector3 flightTarget = new Vector3 (0f, 0f, 0f);

	void Start () {
		flightTarget = setNewFlightTarget ();
	}

	void Update () {
		if (Vector3.Distance (transform.localPosition, flightTarget) < .5f) {
			flightTarget = setNewFlightTarget ();
		}

		transform.localPosition = Vector3.MoveTowards (transform.localPosition, flightTarget, Time.deltaTime);
	}

	Vector3 setNewFlightTarget(){
		float newX = Random.Range (-30f, 30f);
		float newZ = Random.Range (-15f, 15f);
		float newY = Random.Range (5f, 10f);

		return new Vector3 (newX, newY, newZ);
	}
}
