using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager: MonoBehaviour {

	[SerializeField] GameObject Asteroiditos;

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) {
			GameObject ast = Instantiate (Asteroiditos);
			ast.transform.localPosition = this.transform.localPosition;
			ast.AddComponent<Orbitacion> ();
			ast.GetComponent<Orbitacion> ().speed = this.GetComponent<Orbitacion> ().speed;
			Destroy (this.gameObject);
		}
	}
}
