using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollision : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (GameManager._instance.mode == GameManager.ShootingMode.Returning && other.transform.CompareTag("PlayerLose")) {
			other.transform.GetComponent<Astronaut> ().returned = true;
		}
	}
}
