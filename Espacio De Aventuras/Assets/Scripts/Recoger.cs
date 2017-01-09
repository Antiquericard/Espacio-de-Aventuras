using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Recoger : MonoBehaviour {

	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			SceneManager.LoadScene ("Victory");
			//Destroy (this.gameObject);
		}
	}
}
