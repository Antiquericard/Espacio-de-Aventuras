﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Propellers : MonoBehaviour {

	[SerializeField]
	float maxFuel = 50f;
	[SerializeField]
	float fuelAmount;
	[SerializeField]
	float power = 2f;
	[SerializeField]
	float decreaseRate = 10f;

	Rigidbody rigid;

	ParticleSystem left;
	ParticleSystem right;
	ParticleSystem up;

	Slider fuelBar;

	void Awake(){
		rigid = GetComponent<Rigidbody> ();
		left = transform.FindChild ("leftPropeller").GetComponent<ParticleSystem> ();
		right = transform.FindChild ("rightPropeller").GetComponent<ParticleSystem> ();
		up = transform.FindChild ("upPropeller").GetComponent<ParticleSystem> ();
		fuelBar = GameObject.Find ("Fuel").GetComponent<Slider> ();
		fuelAmount = maxFuel;
	}

	void Update(){
		float horizontal = Input.GetAxis ("Horizontal");
		if (horizontal != 0 && fuelAmount > 0f && GameManager._instance.mode == GameManager.ShootingMode.Shooting) {
			Vector3 localRight = transform.worldToLocalMatrix.MultiplyVector (transform.right);
			rigid.AddForce (localRight * horizontal * power, ForceMode.Impulse);
			fuelAmount -= decreaseRate * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			if (horizontal < 0) {
				PlayParticleSystem (right);
				StopParticleSystem (left);
			} else {
				PlayParticleSystem (left);
				StopParticleSystem (right);

			}
		}

		float vertical = Input.GetAxis ("Vertical");
		if (vertical > 0 && fuelAmount > 0f && GameManager._instance.mode == GameManager.ShootingMode.Shooting) {
			Vector3 localUp = transform.worldToLocalMatrix.MultiplyVector (transform.up);
			rigid.AddForce (localUp * horizontal * power, ForceMode.Impulse);
			fuelAmount -= decreaseRate * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			PlayParticleSystem (up);
		} else {
			StopParticleSystem (up);
		}

		if (vertical <= 0 && horizontal == 0) {
			StopParticleSystem (up);
			StopParticleSystem (left);
			StopParticleSystem (right);
		}
	}

	void PlayParticleSystem(ParticleSystem parts){
		if (!parts.isPlaying) {
			parts.Play ();
		}
	}

	void StopParticleSystem(ParticleSystem parts){
		if (parts.isPlaying) {
			parts.Stop ();
		}
	}

	public void Refuel(){
		fuelAmount = maxFuel;
		fuelBar.value = 100;
	}

}
