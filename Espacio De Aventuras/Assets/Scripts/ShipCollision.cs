using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollision : MonoBehaviour {

	#region Unity Methods

	void OnTriggerEnter(Collider other){

		if (other.transform.CompareTag ("PlayerLose")) {
			Astronaut astronaut = other.transform.GetComponent<Astronaut> ();
			if (astronaut.firing.mode == AstronautFiring.ShootingMode.Returning) {
				astronaut.returned = true;
			}
		}

	}

	#endregion

}
