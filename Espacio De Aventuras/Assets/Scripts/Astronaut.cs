using UnityEngine;
using System.Collections;

public class Astronaut : MonoBehaviour {

	[Tooltip("Colocar el transform del cañón.")] [SerializeField] public Transform cannon;

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float moveSpeed = 10f;

	void Update () {
		if (GameManager._instance.mode != GameManager.ShootingMode.Returning && Input.GetKeyDown (KeyCode.R)) {
			GameManager._instance.mode = GameManager.ShootingMode.Returning;
			GameManager._instance.StartCoroutine ("DieCoroutine");
		}
	}

	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
        transform.rotation = Quaternion.Euler(cannon.localRotation.eulerAngles + new Vector3(90f, 0f,0f));
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
		GameManager._instance.AimingMode();
		transform.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.SetActive (false);

	}
}
