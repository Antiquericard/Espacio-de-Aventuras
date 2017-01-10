using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Recoger : GameManager {

	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			Time.timeScale = 0;

			//SceneManager.LoadScene ("Victory");
			//Destroy (this.gameObject);
		}
	}
}
