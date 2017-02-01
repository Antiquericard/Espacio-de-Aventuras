using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour {

	byte lifes = 3;

	[Tooltip("Fuerza de repulsión al impactar.")] [SerializeField] float repulsion = 100f;

	public void Divide () {
		if (lifes > 0) {
			GameObject izq = Instantiate (gameObject);
			// Mandamos cada enemigo hacia un lado.
			izq.GetComponent<Rigidbody> ().AddForce (new Vector3 (1f, 2f, 0f) * repulsion * Time.deltaTime);
			GetComponent<Rigidbody> ().Sleep ();
			GetComponent<Rigidbody> ().AddForce (new Vector3 (-1f, 2f, 0f) * repulsion * Time.deltaTime);
			// Reescalamos el objeto.

			// Restamos una vida al objeto.
		} else {
			Destroy (gameObject);
		}
	}
}
