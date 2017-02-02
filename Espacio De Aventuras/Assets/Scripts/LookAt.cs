using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	[Tooltip("Cámara a la cual tiene que mirar este GameObject.")] [SerializeField] GameObject cam;

	void lateUpdate () {
		transform.LookAt (cam.transform);
	}
}
