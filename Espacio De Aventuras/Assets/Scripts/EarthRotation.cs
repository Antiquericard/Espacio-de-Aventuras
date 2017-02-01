using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotation : MonoBehaviour {

	[Tooltip("Velocidad a la que rota la Tierra.")] [SerializeField] float rotateVelocity = 5f;

	void LateUpdate () {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(.1f,1f,.1f) * rotateVelocity * Time.deltaTime);
	}
}
