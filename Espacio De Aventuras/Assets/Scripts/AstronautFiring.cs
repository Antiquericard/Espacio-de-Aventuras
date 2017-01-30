
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este script debe estar asociado al Base Cannon. Proporciona funcionalidad completa de disparo, incluidos los controles, e información
/// a otros componentes de la escena sobre el estado del disparo
/// </summary>
public class AstronautFiring : MonoBehaviour {

	// Este script debe ir en el base cannon de la escena

	#region Setting Attributes

	[Tooltip("Introducir prefab del astronauta.")] [SerializeField] GameObject astronaut;

	// Valor de potencia.
	float powerValue = 0f;

	// Distancia real del astronauta.
	Vector3 astronautTrueDistance;

	// Enumerado de los modos de disparo.
	public enum ShootingMode : byte {Idle, Aiming, Shooting, Returning};

	// Modo de disparo.
	public ShootingMode mode;

	CameraMovement cameraMovement;

	// Constantes de posicionamiento de objetos
	[Tooltip("Distancia local de la cámara.")] [SerializeField] public Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (-2f, 0f, 3.39f);

	[Tooltip("Distancia de la cámara al astronauta.")] [SerializeField] public Vector3 CAMERA_ASTRONAUT_DISTANCE = new Vector3 (0f, 1f, -2.5f);

	[Tooltip("Distancia del astronauta al cannon.")] [SerializeField] public Vector3 ASTRONAUT_CANNON_DISTANCE = new Vector3 (0f, 1f, -3f);

	[Tooltip("Rotación por defecto de la cámara.")] [SerializeField] public Vector3 CAMERA_ROTATE = new Vector3(-180f, -80f, -90f);


	//Objetos requeridos de introducir

	[Tooltip("Barra de potencia de disparo.")] [SerializeField] Image PowerBar;

	[Tooltip("Barra padre de potencia de disparo.")] [SerializeField] GameObject powBar;

	[Tooltip("Prefab del astronauta.")] [SerializeField] Object astronautPrefab;

	[Tooltip("Velocidad inicial del astronauta al dispararse.")] [SerializeField] float startingVelocity = 100f;

	[Tooltip("Potencia inicial del astronauta al dispararse.")] [SerializeField] float powerIncreaseRate = 1f;

	#endregion

	#region Unity Methods

	void Awake(){
		// Creamos un astronauta, solo usaremos ese no tenemos que crear mas
		astronaut = Instantiate (astronautPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		astronaut.GetComponent<Astronaut>().cannon = transform;
		astronaut.SetActive (false);

		cameraMovement = Camera.main.GetComponent<CameraMovement> ();
	}

	void Update () {
		switch (mode) {
		case ShootingMode.Idle:
			// Si está esperando...
			if (Input.GetMouseButtonDown (0)) {
				mode = ShootingMode.Aiming;
			}
			break;
		case ShootingMode.Aiming:
			// Apuntando...
			if (Input.GetMouseButtonUp (0)) {
				powBar.SetActive (false);
				GetComponent<AudioSource> ().Play ();
				Shoot ();
			} else {
				if (!powBar.activeSelf) {
					powBar.SetActive (true);
				}
				powerValue += powerIncreaseRate * Time.deltaTime;
				if (powerValue > 1) {
					powerValue--;
				}
				PowerBar.fillAmount = powerValue;
			}
			break;
		case ShootingMode.Shooting:
			
			// Ya ha disparado!
			cameraMovement.wantedPosition = astronaut.transform.position + astronautTrueDistance;
			cameraMovement.wantedRotation = astronaut.transform.rotation;
			break;
		case ShootingMode.Returning:
			// Volviendo a la nave
			cameraMovement.wantedPosition = astronaut.transform.position + astronautTrueDistance;
			cameraMovement.wantedRotation = astronaut.transform.rotation;
			break;
		}

	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Método para realizar el disparo.
	/// </summary>
	public void Shoot(){

		// Ajustamos la potencia: Un disparo completamente cargado tiene 4 veces más potencia que uno sin cargar
		// Convertimos esta progresión 1-100 a una 25-100

		float adjustedPowerValue = ((powerValue - .01f) * .75f / .99f) + .25f;
		astronaut.SetActive (true);
		astronaut.tag = "Player"; //El tag se lo quitamos en el tiro anterior para evitar a los planetas
		astronaut.layer = 0; //Asi podra chocar con los planetas de nuevo
		astronaut.GetComponent<Astronaut> ().Init (adjustedPowerValue * startingVelocity);
		powerValue = 0f;

		// Esto rota la distancia. Es importante que el Quaternion esté en el LADO IZQUIERDO del Vector3 para poder rotarlo
		astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;

		mode = ShootingMode.Shooting;

		// Ponemos la cámara arriba del todo de la jerarquia
		cameraMovement.transform.SetParent(null);

		// Desactivamos la ayuda para disparar
		transform.GetChild(0).GetComponentInChildren<ParticleSystem> ().Clear ();
		transform.GetChild(0).GetComponentInChildren<ParticleSystem> ().Stop ();

	}

	/// <summary>
	/// Método para volver al estado inicial de disparo.
	/// </summary>
	public void ReturnToIdleMode(){
		mode = ShootingMode.Idle;
		cameraMovement.UpdateFOV (0);
		cameraMovement.transform.SetParent (transform.GetChild(0));
		cameraMovement.transform.localPosition = CAMERA_CANNON_DISTANCE;
		cameraMovement.transform.localRotation = Quaternion.Euler(CAMERA_ROTATE);
		GetComponentInChildren<ParticleSystem> ().Play ();
	}

	/// <summary>
	/// Se ejecuta cuando el astronauta pierde una vida. Le devuelve a la nave.
	/// </summary>
	public void LaunchFail(){
		astronaut.tag = "PlayerLose"; // Así no chocará con planetas ni activará la gravedad en su vuelta
		astronaut.layer = 7; // Evita que pueda chocar con planetas
		mode = ShootingMode.Returning;
		// En el caso de los planetas, hay que volver a activar la rotación de la cámara
		Camera.main.GetComponent<CameraMovement> ().allowedRotation = true; 
		StartCoroutine ("DieCoroutine");
	}

	#endregion

	#region Coroutines

	/// <summary>
	/// Coroutine al perder una vida, si se pierden todas comienza pantalla de derrota.
	/// </summary>
	/// <returns>The coroutine.</returns>
	IEnumerator DieCoroutine(){
		yield return new WaitForSeconds (.5f);
		astronaut.GetComponent<Rigidbody> ().velocity = new Vector3 ();
		//particles.Play ();
		if (--GameManager.instance.lifes > 0) {
			astronaut.GetComponent<Astronaut> ().StartCoroutine ("ReturnToSpaceShip");
			astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;
		} else {
			GameManager.instance.CompleteLevel (false);
		}
	}

	#endregion

}
