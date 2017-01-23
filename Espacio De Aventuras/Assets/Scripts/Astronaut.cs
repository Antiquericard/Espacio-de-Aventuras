﻿
/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya & Nicolás Robayo Moreno 2017
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Propellers))]

public class Astronaut : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("Colocar el transform del cañón.")] [SerializeField] public Transform cannon;

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float moveSpeed = 10f;

	// Retardo del touch para retornar.
	float touchTime = 0f;

	// Inicialización del getter de los propulsores.
	Propellers propellers{
		get {
			return GetComponent<Propellers> ();
		}
	}

	#endregion

	#region Unity Methods

	// Cuando pulsamos la pantalla más de touchTime, haremos un retorno a la nave.
	void Update () {
		bool control;
		//Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);
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

	#endregion

	#region Public Methods

	// Método para aplicar la inercia al astronauta.
	public void Init(float speed, Vector3 shipSpeed){
		transform.parent = cannon;
		transform.localPosition = GameManager._instance.ASTRONAUT_CANNON_DISTANCE;
		transform.rotation = Quaternion.Euler(cannon.GetChild(0).eulerAngles + new Vector3(180f, 0f,0f));
		transform.parent = null;

		GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);
	}

	#endregion

	#region Coroutines

	// Coroutine para volver a la nave.
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

	#endregion

}
