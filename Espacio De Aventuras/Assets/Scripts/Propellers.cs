
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
/// Ofrece funcionalidad de poder controlar propulsores con combustible al jugador
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Astronaut))]
public class Propellers : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("Fuel máximo para este nivel.")] [SerializeField] float maxFuel = 50f;

	[Tooltip("Potencia máxima.")] [SerializeField] float frontPower = 5f;

	[Tooltip("Potencia mínima.")] [SerializeField] float sidePower = 2f;

	[Tooltip("Ratio de decremento.")] [SerializeField] float decreaseRate = 10f;

	[Tooltip("Máxima velocidad.")] [SerializeField] float maxSpeed = 45f;

	// Cantidad de fuel.
	float fuelAmount;

	// Módulo de la velocidad máxima.
	float sqrMaxSpeed;

	// Rigidbody del astronauta.
	Rigidbody rigid;

	// Sistema de particulas izquierdo.
	ParticleSystem left;

	// Sistema de partículas derecho.
	ParticleSystem right;

	// Sistema de partículas frente.
	ParticleSystem up;

	// Slider del fuel.
	Slider fuelBar;

	// Movimiento frontal.
	float moveFront = 0f;

	// Movimiento lateral.
	float moveSide = 0f;

	#endregion

	#region Unity Methods

	void Awake(){
		//Inicializamos todos los objetos
		rigid = GetComponent<Rigidbody> ();
		left = transform.GetChild(0).FindChild ("leftPropeller").GetComponent<ParticleSystem> ();
		right = transform.GetChild(0).FindChild ("rightPropeller").GetComponent<ParticleSystem> ();
		up = transform.GetChild(0).FindChild ("upPropeller").GetComponent<ParticleSystem> ();
		fuelBar = GameObject.Find ("Fuel").GetComponent<Slider> ();
		fuelAmount = maxFuel;
		sqrMaxSpeed = maxSpeed * maxSpeed;
	}

	void FixedUpdate(){
		//En este código recogemos para dónde se quiere mover el jugador, convertimos el movimiento a los ejes locales
		//Y entonces lo impulsamos en la dirección deseada
		if (moveSide != 0) {
			Vector3 localRight = transform.worldToLocalMatrix.MultiplyVector (transform.right);
			rigid.AddForce (localRight * moveSide * sidePower, ForceMode.Impulse);
		}

		if (moveFront != 0) {
			Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector (transform.forward);
			rigid.AddForce (localForward * moveFront * frontPower, ForceMode.Impulse);
		}
	}

	void Update(){
		
		float horizontal;
		//En escritorio simplemente son los ejes
		#if UNITY_STANDALONE
		horizontal = Input.GetAxis ("Horizontal");
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		//Utilizamos el acelerómetro, pero capamos los valores muy pequeños a 0, para que se pueda quedar sin mover los propulsores fácil
		horizontal = Input.acceleration.x;
		if(horizontal > -0.2f && horizontal < 0.2f){
			horizontal = 0;
		}
		#endif


		//Todo este código define el combustible que tenemos y cómo queda, y además los sistemas
		//de partículas de los propulsores
		if (horizontal != 0 && fuelAmount > 0f && GetComponent<Astronaut>().firing.mode == AstronautFiring.ShootingMode.Shooting) {
			moveSide = horizontal;
			fuelAmount -= decreaseRate * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			if (horizontal < 0) {
				PlayParticleSystem (right);
				StopParticleSystem (left);
			} else {
				PlayParticleSystem (left);
				StopParticleSystem (right);

			}
		} else {
			StopParticleSystem (left);
			StopParticleSystem (right);
		}

		//Capamos la velocidad máxima del astronauta por motivos de seguridad (y por jugabilidad)
		if (rigid.velocity.sqrMagnitude > sqrMaxSpeed) {
			rigid.velocity = rigid.velocity / rigid.velocity.magnitude * maxSpeed;
		}

		float vertical;

		//Ver controles horizontales
		#if UNITY_STANDALONE
		vertical = Input.GetAxis ("Vertical");
		#endif

		#if UNITY_ANDROID
		vertical = Input.acceleration.y;
		if(vertical < 0){
			vertical = 0;
		}
		#endif

		#if UNITY_IOS
		vertical = Input.acceleration.y;
		if(vertical < 0){
		vertical = 0;
		}
		#endif

		//Ver manejo horizontal, pero esta simplificado porque no hay atrás
		if (vertical > 0 && fuelAmount > 0f && GetComponent<Astronaut>().firing.mode == AstronautFiring.ShootingMode.Shooting) {
			moveFront = vertical;
			fuelAmount -= decreaseRate * 2 * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			PlayParticleSystem (up);
		} else {
			StopParticleSystem (up);
		}

	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Activa el sistema de partículas, pero sólo si no estaba ya activo.
	/// </summary>
	/// <param name="parts">El sistema de partículas</param>
	void PlayParticleSystem(ParticleSystem parts){
		if (!parts.isPlaying) {
			parts.Play ();
		}
	}

	/// <summary>
	/// Detiende el sistema de partículas, pero sólo si no estaba ya detenido.
	/// </summary>
	/// <param name="parts">El sistema de partículas</param>
	void StopParticleSystem(ParticleSystem parts){
		if (parts.isPlaying) {
			parts.Stop ();
		}
	}

	/// <summary>
	/// Recarga el combustible
	/// </summary>
	public void Refuel(){
		fuelAmount = maxFuel;
		fuelBar.value = 100;
	}

	#endregion

}
