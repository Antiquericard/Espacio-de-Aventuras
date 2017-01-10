using UnityEngine;
using System.Collections;

public class Astronaut : GameManager {

	[Tooltip("Colocar el transform del cañón.")] [SerializeField] Transform cannon;

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float MoveSpeed = 0.1f;

	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = new Vector3 (0f, 0f, 1.5f);
		transform.localEulerAngles = Vector2.zero;
		transform.parent = null;

		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}

	public override void returnToSpaceShip (){
		if (transform.localPosition.x == 0f && transform.localPosition.y == 0f && transform.localPosition.z == 1.5f) {
			var targetPos = new Vector3 (cannon.position.x, cannon.position.y, cannon.position.z);
			transform.LookAt (cannon);
			transform.position += transform.forward * MoveSpeed * Time.deltaTime;
		}
	}
}
