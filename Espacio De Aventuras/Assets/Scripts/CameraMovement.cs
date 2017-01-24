
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

using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("")] [SerializeField] float scaleRotationX = 1f;

	[Tooltip("")] [SerializeField] float scaleRotationY = 1f;

	//[Tooltip("")] [SerializeField] float scaleRotationPhoneX = 1f;

	//[Tooltip("")] [SerializeField] float scaleRotationPhoneY = 1f;

	[Tooltip("")] [SerializeField] float positionDamping = 25f;

	[Tooltip("")] [SerializeField] float rotationDamping = 10f;

	// XXX
	bool _allowedRotation = true;

	#endregion

	#region Getters & Setters

	// XXX
	public bool allowedRotation {
		get{
			return _allowedRotation;
		}
		set{
			_allowedRotation = value;
		}
	}

	// XXX
	public Vector3 wantedPosition {
		get;
		set;
	}

	// XXX
	public Quaternion wantedRotation {
		get;
		set;
	}

	#endregion

	// Update is called once per frame
	//AQUI ESTÁN LOS INPUTS PARA MOBILE

	#region Unity Methods

	// Seguimiento de la cámará cuando se encuentra fuera de la nave y el sistema de disparo cuando se encuentre dentro.
	void LateUpdate () {

		if (GameManager._instance.mode == GameManager.ShootingMode.Shooting || GameManager._instance.mode == GameManager.ShootingMode.Returning) {
			//Si llega hasta aqui es que ha disparado
			transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * positionDamping);
			if (allowedRotation)
				transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		} else {

			//En esta linea se dá por hecho que el padre de la camara es el cannon.

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

			#if UNITY_IOS

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

	#endregion

}
