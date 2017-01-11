using UnityEngine;
using System.Collections;

public class Astronaut : MonoBehaviour {

	[Tooltip("Colocar el transform del cañón.")] [SerializeField] public Transform cannon;

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float moveSpeed = 10f;

	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
		transform.localEulerAngles = Vector2.zero;
		transform.parent = null;

		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}

	public IEnumerator ReturnToSpaceShip (){
		transform.LookAt (cannon);
		Vector3 targetPos = cannon.position + GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
		transform.GetComponent<Rigidbody> ().isKinematic = true;

		while (Vector3.Distance(targetPos, transform.position) > 2.5f) {
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
			yield return null;
		}
		//transform.GetComponent<Rigidbody> ().isKinematic = false;
		GameManager._instance.AimingMode();
		Destroy (this.gameObject);
		//TODO Preparar una pool!

	}
}
