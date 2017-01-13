using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Recoger : MonoBehaviour {

	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			GameManager._instance.vic = true;
            GameManager._instance.CompleteLevel();
		}
	}
}
