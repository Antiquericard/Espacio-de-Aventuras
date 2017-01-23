using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLimits : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if (other.CompareTag("Player") && GameManager._instance.mode != GameManager.ShootingMode.Returning) {
			other.tag = "PlayerLose"; //asi no chocara con planetas ni activara gravedad en su vuelta
			other.gameObject.layer = 7; //evita que pueda chocar con planetas
			GameManager._instance.mode = GameManager.ShootingMode.Returning;
			GameManager._instance.StartCoroutine ("DieCoroutine");
		}
	}

}
