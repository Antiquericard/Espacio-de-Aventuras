using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

	// Hacer un pull de objetos

	[Tooltip("Velocidad a la cual se desplaza la bala.")] [SerializeField] int velocidad = 2;

	void Update () {
		transform.position += Vector3.up * Time.deltaTime * velocidad;
		transform.localScale += Vector3.up * Time.deltaTime * 2 * velocidad;
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Enemy")) {
			other.GetComponent<Enemigo> ().Divide ();
			Destroy (gameObject);
		}
		if (other.CompareTag ("Floor")) {
			Destroy (gameObject);
		}
	}

}
