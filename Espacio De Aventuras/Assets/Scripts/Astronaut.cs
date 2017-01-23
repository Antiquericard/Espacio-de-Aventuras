using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Propellers))]
public class Astronaut : MonoBehaviour {

	[Tooltip("Colocar el transform del cañón.")] [SerializeField] public Transform cannon;

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float moveSpeed = 10f;

	float touchTime = 0f;

	Propellers propellers{
		get {
			return GetComponent<Propellers> ();
		}
	}

	void Update () {
		bool control;

		#if UNITY_STANDALONE

			control = Input.GetKeyDown(KeyCode.R);

		#endif

		#if UNITY_ANDROID

		if(Input.GetMouseButton(0)){
			touchTime += Input.GetTouch(0).deltaTime;
		} else {
			touchTime = 0;
		}

		if (touchTime > 1f) {
			touchTime = 0;
			control = true;
		} else {
			control = false;
		}

		#endif

		#if UNITY_IOS

		if(Input.GetMouseButton(0)){
		touchTime += Input.GetTouch(0).deltaTime;
		} else {
		touchTime = 0;
		}

		if (touchTime > 1f) {
		touchTime = 0;
		control = true;
		} else {
		control = false;
		}

		#endif

		if (GameManager._instance.mode == GameManager.ShootingMode.Shooting && control) {
			control = false;
			GameManager._instance.mode = GameManager.ShootingMode.Returning;
			GameManager._instance.StartCoroutine ("DieCoroutine");
		}
	}

	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
		transform.rotation = Quaternion.Euler(cannon.GetChild(0).eulerAngles + new Vector3(180f, 0f,0f));
		transform.parent = null;

		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}
	public IEnumerator ReturnToSpaceShip (){
		this.GetComponent<AudioSource> ().Play ();
		transform.LookAt (cannon);
		Vector3 targetPos = cannon.position + GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
		transform.GetComponent<Rigidbody> ().isKinematic = true;

		while (Vector3.Distance(targetPos, transform.position) > 2.5f) {
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
			yield return null;
		}
		propellers.Refuel ();
		GameManager._instance.ReturnToIdleMode();
		transform.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.SetActive (false);

	}
}
