using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	float scaleRotationX = 1f;
	[SerializeField]
	float scaleRotationY = 1f;
	[SerializeField]
	float positionDamping = 25f;
	[SerializeField]
	float rotationDamping = 10f;

	public Vector3 wantedPosition {
		get;
		set;
	}

	public Quaternion wantedRotation {
		get;
		set;
	}

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!GameManager._instance.aimingMode) {
			//Si llega hasta aqui es que ha disparado
			transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * positionDamping);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		} else {
			//En esta linea doy por hecho que el padre de la camara es el cannon
			transform.parent.Rotate (-scaleRotationY * Input.GetAxis("Mouse Y"),-scaleRotationX * -Input.GetAxis("Mouse X"), 0f);
		}

	}


}
