using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Propellers : MonoBehaviour {

	[SerializeField]
	float maxFuel = 50f;
	float fuelAmount;
	[SerializeField]
	float frontPower = 5f;
	[SerializeField]
	float sidePower = 2f;
	[SerializeField]
	float decreaseRate = 10f;

	Rigidbody rigid;

	ParticleSystem left;
	ParticleSystem right;
	ParticleSystem up;

	Slider fuelBar;

	float moveFront = 0f;
	float moveSide = 0f;

	void Awake(){
		rigid = GetComponent<Rigidbody> ();
		left = transform.GetChild(0).FindChild ("leftPropeller").GetComponent<ParticleSystem> ();
		right = transform.GetChild(0).FindChild ("rightPropeller").GetComponent<ParticleSystem> ();
		up = transform.GetChild(0).FindChild ("upPropeller").GetComponent<ParticleSystem> ();
		fuelBar = GameObject.Find ("Fuel").GetComponent<Slider> ();
		fuelAmount = maxFuel;
	}

	void FixedUpdate(){
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
		#if UNITY_STANDALONE
		horizontal = Input.GetAxis ("Horizontal");
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		horizontal = Input.acceleration.x;
		#endif


		if (horizontal != 0 && fuelAmount > 0f && GameManager._instance.mode == GameManager.ShootingMode.Shooting) {
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

		float vertical;
		#if UNITY_STANDALONE
		horizontal = Input.GetAxis ("Vertical");
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		vertical = Input.acceleration.y;
		if(vertical < 0){
			vertical = 0;
		}
		#endif

		if (vertical > 0 && fuelAmount > 0f && GameManager._instance.mode == GameManager.ShootingMode.Shooting) {
			moveFront = vertical;
			fuelAmount -= decreaseRate * 2 * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			PlayParticleSystem (up);
		} else {
			StopParticleSystem (up);
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
