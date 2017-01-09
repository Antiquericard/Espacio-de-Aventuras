using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLimits : MonoBehaviour {

	void OnTriggerExit(Collider other){
		SceneManager.LoadScene ("Lose");
		//GameManager._instance.AimingMode ();
		//Destroy (other.gameObject);
	}
}
