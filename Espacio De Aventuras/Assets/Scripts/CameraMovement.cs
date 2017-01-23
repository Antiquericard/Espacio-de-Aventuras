﻿using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	float scaleRotationX = 1f;
	[SerializeField]
	float scaleRotationY = 1f;

	[SerializeField]
	float scaleRotationPhoneX = 1f;
	[SerializeField]
	float scaleRotationPhoneY = 1f;

	[SerializeField]
	float positionDamping = 25f;
	[SerializeField]
	float rotationDamping = 10f;

	bool _allowedRotation = true;
	public bool allowedRotation {
		get{
			return _allowedRotation;
		}
		set{
			_allowedRotation = value;
		}
	}

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
	//AQUI ESTÁN LOS INPUTS PARA MOBILE
	void LateUpdate () {

		if (GameManager._instance.mode == GameManager.ShootingMode.Shooting || GameManager._instance.mode == GameManager.ShootingMode.Returning) {
			//Si llega hasta aqui es que ha disparado
			transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * positionDamping);
			if(allowedRotation)
				transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		} else {
			//En esta linea doy por hecho que el padre de la camara es el cannon

			float xValue = 0f;
			float yValue = 0f;

			#if UNITY_STANDALONE
			xValue = scaleRotationX * Input.GetAxis("Mouse X");
			yValue = -scaleRotationY * Input.GetAxis("Mouse Y");
			#endif

			#if UNITY_ANDROID
			if(Input.touchCount > 0){
				Vector2 delta = Input.GetTouch(0).deltaPosition;
				if(delta.x > 1f){
					delta.x = 1f;
				}
				if(delta.y > 1f){
					delta.y = 1f;
				}
				if(delta.x < -1f){
					delta.x = -1f;
				}
				if(delta.y < -1f){
					delta.y = -1f;
				}

				xValue = scaleRotationPhoneX * delta.x;
				yValue = -scaleRotationPhoneY * delta.y;
			}

			#endif

			transform.parent.parent.Rotate (yValue, xValue, 0f);
		}

	}


}
