using UnityEngine;
using System.Collections;

public class LevelLimits : MonoBehaviour {

	void OnTriggerExit(Collider other){
		GameManager._instance.AimingMode ();
	}
}
