using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	string [] levelTexts;
	int level = 1;
	float sqrMinVelocity;
	BoxCollider bounds;

	//0: Modo de apuntar cámara 1: Modo de potencia 2: Modo disparado
	public static short mode = 0;

	[SerializeField]
	float startingVelocity = 50f;
	[SerializeField]
	Object astronaut;
	[SerializeField]
	Transform cannon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Shoot ();
		}
	}


	public void Shoot(){
		Debug.Log (mode);
		if (mode == 1) {
			GameObject lanzamiento = Instantiate (astronaut, cannon.position, cannon.rotation) as GameObject;
			lanzamiento.GetComponent<Rigidbody> ().velocity = startingVelocity * -lanzamiento.transform.forward;

			mode = 2;
		} else if (mode == 0) {
			mode = 1;
		}
	}
}
