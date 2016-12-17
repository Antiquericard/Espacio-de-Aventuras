using UnityEngine;
using System.Collections;

public class Astronaut : MonoBehaviour {

	public void Init(float speed, Transform cannon, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = new Vector3 (0f, 0f, 1.5f);
		transform.localEulerAngles = Vector2.zero;
		transform.parent = null;
		Debug.Log (shipSpeed);
		//GetComponent<Rigidbody> ().velocity = shipSpeed;
		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}

}
