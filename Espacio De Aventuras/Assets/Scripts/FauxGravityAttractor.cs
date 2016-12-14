using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : MonoBehaviour {

	[Tooltip("Gravedad con la cual el planeta atraerá a los objetos.")] [SerializeField] float gravity = -10f;

	[Tooltip("Velocidad a la que el objeto rota para posicionarse de pie al caer en un planeta.")] [SerializeField] float speedRotation = 50f;

	public void Attract (Transform body) {

		//Guardamos estas variables para coger las normales.
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 bodyUp = body.up;

		//Añadimos una fuerza al personaje que haga las veces de atracción hacia el planeta.
		body.GetComponent<Rigidbody> ().AddForce (gravityUp * gravity);

		//Rotamos al personaje para que caiga de pie.
		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * body.rotation;

		//Suavizamos la rotación.
		body.rotation = Quaternion.Slerp (body.rotation, targetRotation, speedRotation * Time.deltaTime);
	}
}
