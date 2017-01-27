using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour {

	#region Unity Methods

	void OnCollisionEnter(Collision col){
		if (col.transform.CompareTag("Player") && col.transform.GetComponent<Astronaut>().firing.mode != AstronautFiring.ShootingMode.Returning) {
			col.transform.GetComponent<Astronaut> ().firing.LaunchFail ();
		}
	}

	#endregion
}
