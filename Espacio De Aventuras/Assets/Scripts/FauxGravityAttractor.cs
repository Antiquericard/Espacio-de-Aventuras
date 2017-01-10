using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : GameManager {

	[Tooltip("Gravedad con la cual el planeta atraerá a los objetos.")] [SerializeField] float gravity = -10f;

	[Tooltip("Velocidad a la que el objeto rota para posicionarse de pie al caer en un planeta.")] [SerializeField] float speedRotation = 50f;

	[SerializeField] ForceMode forceMode = ForceMode.Force;

	public ForceMode _forceMode
	{
		get { return forceMode; }
		set { forceMode = value; }
	}

	public void Attract (Transform body) {

		//Guardamos estas variables para coger las normales.
		Vector3 gravityUp = (body.position - transform.position);
		Vector3 bodyUp = body.up;
		Rigidbody rigid = body.GetComponent<Rigidbody> ();

		//Añadimos una fuerza al personaje que haga las veces de atracción hacia el planeta.
		rigid.AddForce(gravityUp.normalized * body.GetComponent<Rigidbody> ().mass * gravity / gravityUp.sqrMagnitude, _forceMode);

		//Rotamos al personaje para que caiga de pie.
		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp.normalized) * body.rotation;

		//Suavizamos la rotación.
		body.rotation = Quaternion.Slerp (body.rotation, targetRotation, speedRotation * Time.deltaTime);

	}
}
