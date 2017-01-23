using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Recoger : MonoBehaviour {

	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			hit.GetComponent<Animator> ().SetBool ("Recoge", true);
			hit.GetComponent<Animator> ().SetBool ("Recoge", false);
			this.GetComponent<AudioSource> ().Play ();
			GameManager._instance.vic = true;
            GameManager._instance.CompleteLevel();
		}
	}
}
