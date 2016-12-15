using UnityEngine;
using System.Collections;

public class Recoger : MonoBehaviour {

	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			Destroy (this.gameObject);
		}
	}
}
