using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	float scaleRotationX = 0.5f;
	[SerializeField]
	float scaleRotationY = 0.5f;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//En esta linea doy por hecho que el padre de la camara es el cannon
		transform.parent.Rotate (-scaleRotationY * Input.GetAxis("Mouse Y"),-scaleRotationX * Input.GetAxis("Mouse X"), 0f);
	}
}
