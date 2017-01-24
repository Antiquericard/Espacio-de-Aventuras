using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.collider.CompareTag ("Player") && GameManager.instance.mode != GameManager.ShootingMode.Returning) {
			GameManager.instance.LaunchFail ();
		}
	}
}
