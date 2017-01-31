
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

	[Tooltip("Velocidad a la que retorna el personaje a la nave.")] [SerializeField] float moveSpeed = 10f;

	public AstronautFiring firing{
		get{
			return cannon.GetComponent<AstronautFiring> ();
		}
	}


	// Tiempo en el cual se mantiene pulsada la pantalla.
	#pragma warning disable 0414
	float touchTime = 0f;
	#pragma warning restore 0414

	public Transform cannon;
	public bool returned = false;

	#endregion

	#region Setters & Getters

	Propellers propellers{
		get {
			return GetComponent<Propellers> ();
		}
	}

	#endregion

	#region Unity Methods


	// Cuando se mantiene pulsada la pantalla mas de touchTime se activa el retorno a la nave.
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

		if (firing.mode == AstronautFiring.ShootingMode.Shooting && control) {
			control = false;
			firing.LaunchFail ();
		}
	}

	#endregion

	#region Public Methods

	// Método para comenzar el primer disparo.
	public void Init(float speed/*, Vector3 shipSpeed*/){
		returned = false;
		transform.parent = cannon;
		transform.localPosition = firing.ASTRONAUT_CANNON_DISTANCE;
		Vector3 rotation = -cannon.eulerAngles; //la rotación correcta parece ser la inversa del propio cañon
		transform.localRotation = Quaternion.Euler(rotation);
		transform.parent = null;

		GameManager.instance.markPosition = transform.position;

		//GetComponent<Rigidbody> ().AddForce (shipSpeed, ForceMode.VelocityChange);
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.Impulse);

		Camera.main.GetComponent<CameraMovement> ().UpdateFOV (speed * speed);
	}

	#endregion

	#region Coroutines

	// Coroutine para volver a la nave.
	public IEnumerator ReturnToSpaceShip (){
		this.GetComponent<AudioSource> ().Play ();
		transform.LookAt (cannon);
		//Vector3 targetPos = cannon.position + firing.ASTRONAUT_CANNON_DISTANCE;
		transform.GetComponent<Rigidbody> ().isKinematic = true;

		while (!returned) {
			transform.LookAt (cannon); //por si acaso hay algun problema con los planetas
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
			Camera.main.GetComponent<CameraMovement> ().UpdateFOV (moveSpeed * moveSpeed);
			yield return null;
		}
		propellers.Refuel ();
		firing.ReturnToIdleMode();
		transform.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.SetActive (false);
	}

	#endregion

}
