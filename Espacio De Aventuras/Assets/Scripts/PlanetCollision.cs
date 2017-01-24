﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.collider.CompareTag ("Player") && GameManager._instance.mode != GameManager.ShootingMode.Returning) {
			GameManager._instance.LaunchFail ();
		}
	}
}
