using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Propellers : MonoBehaviour {

	[SerializeField]
	float fuelAmount = 50f;
	[SerializeField]
	float power = 10f;
	[SerializeField]
	float decreaseRate = 1f;

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
	}

	void Update(){
		float horizontal = Input.GetAxis ("Horizontal");
		if (horizontal != 0 && fuelAmount > 0f) {
			rigid.AddForce (transform.right * horizontal * power, ForceMode.Impulse);
			fuelAmount -= decreaseRate * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			if (horizontal < 0) {
				left.Play ();
				right.Stop ();
			} else {
				right.Play ();
				left.Stop ();
			}
		}

		float vertical = Input.GetAxis ("Vertical");
		if (vertical > 0 && fuelAmount > 0f) {
			rigid.AddForce (transform.up * horizontal * power, ForceMode.Impulse);
			fuelAmount -= decreaseRate * Time.deltaTime;
			fuelBar.value = fuelAmount / 5;
			up.Play ();
		} else {
			up.Stop ();
		}
	}


}
