﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLimits : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if (GameManager._instance.mode != GameManager.ShootingMode.Returning) {
			GameManager._instance.mode = GameManager.ShootingMode.Returning;
			GameManager._instance.StartCoroutine ("DieCoroutine");
		}

	}
}
