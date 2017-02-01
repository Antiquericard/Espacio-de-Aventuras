using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl : MonoBehaviour {

	[Tooltip("Velocidad a la que se desplaza el player.")] [SerializeField] float Speed = 10f;

	[Tooltip("Velocidad a la que se salta el player.")] [SerializeField] float Jump = 1000f;

	[Tooltip("Variable para controlar si se encuentra tocando algún suelo.")] [SerializeField] bool OnGround = false;

	[Tooltip("Prefab de la bala.")] [SerializeField] GameObject bala;

	[Tooltip("Offset de donde tiene que aparecer la bala.")] [SerializeField] float offset = 1f;

	// Este script está destinado al movimiento del personaje.

	//Aquí controlaremos los Inputs del personaje.

	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)){
			Shoot();
		} 

		if (Input.GetKey(KeyCode.A)){
			transform.position += Vector3.right*Speed*Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.D)){
			transform.position += Vector3.left*Speed*Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.W) && OnGround) {
			this.GetComponent<Rigidbody> ().AddForce (Vector3.up * Time.deltaTime * Jump);
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.collider.CompareTag ("Floor") && collision != null) {
			OnGround = true;
		}
	}

	void OnCollisionExit (Collision collision) {
		if (collision.collider.CompareTag ("Floor") && collision != null) {
			OnGround = false;
		}
	}

	void Shoot () {
		GameObject ba = Instantiate (bala);
		ba.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0.2f);
	}



}
