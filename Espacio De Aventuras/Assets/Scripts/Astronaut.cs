using UnityEngine;
using System.Collections;

public class Astronaut : MonoBehaviour {

	public void Init(float speed, Transform cannon){
		transform.parent = cannon;
		transform.localPosition = new Vector3 (0f, 0f, 1.5f);
		transform.localEulerAngles = Vector2.zero;
		transform.parent = null;
		//GetComponent<Rigidbody> ().velocity = cannon.GetComponent<Rigidbody> ().velocity;
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
		//GetComponent<Rigidbody> ().velocity = speed * transform.forward;
	}

}
