using UnityEngine;
using System.Collections;

public class Astronaut : GameManager {

	[SerializeField] Transform cannon;

	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = new Vector3 (0f, 0f, 1.5f);
		transform.localEulerAngles = Vector2.zero;
		transform.parent = null;

		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}

	public void returnToSpaceShip (){
		if(  distancia >= MinDist && distancia <= MaxDist  ){
			var targetPos = new Vector3( cannon.x, cannon.position.y, cannon.z);
			transform.LookAt(cannon);
			transform.position += transform.forward*MoveSpeed*Time.deltaTime;
	}

	IEnumerator DieCorutine(){
		yield return new WaitForSeconds (.5f);
		GetComponent<Rigidbody2D> ().isKinematic = true;
		particles.Play ();
		if (life > 0)
			returnToSpaceShip ();
	}
}
