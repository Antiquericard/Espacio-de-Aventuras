
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

/// <summary>
/// Este script mueve la cámara selectivamente dependiendo del modo de disparo.
/// </summary>
public class CameraMovement : MonoBehaviour {

	#region Setting Attributes

	AstronautFiring firing;

	#pragma warning disable 0414

	[Tooltip("")] [SerializeField] float scaleRotationX = 1f;
	[Tooltip("")] [SerializeField] float scaleRotationY = 1f;
	[Tooltip("")] [SerializeField] float scaleRotationPhoneX = 1f;
	[Tooltip("")] [SerializeField] float scaleRotationPhoneY = 1f;

	#pragma warning restore 0414

	[Tooltip("")] [SerializeField] float positionDamping = 25f;

	[Tooltip("")] [SerializeField] float rotationDamping = 10f;

	bool _allowedRotation = true;

	#endregion

	#region Getters & Setters

	// Variable que permite habilitar o deshabilitar la rotación de la cámara
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

	#region Unity Methods

	void Start(){
		firing = transform.parent.parent.GetComponent<AstronautFiring> ();
	}

	// Seguimiento de la cámará cuando se encuentra fuera de la nave y el sistema de disparo cuando se encuentre dentro.
	void LateUpdate () {

		if (firing.mode == AstronautFiring.ShootingMode.Shooting || firing.mode == AstronautFiring.ShootingMode.Returning) {
			//Si llega hasta aqui es que ha disparado
			transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * positionDamping);
			if (allowedRotation)
				transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		} else {

			float xValue = 0f;
			float yValue = 0f;

			// Para la versión de escritorio simplemente cogemos la entrada de ratón
			#if UNITY_STANDALONE
			yValue = scaleRotationX * Input.GetAxis("Mouse X");
			xValue = -scaleRotationY * Input.GetAxis("Mouse Y");
			#endif

			// Para la versión de móvil cogemos el movimiento táctil, pero lo capeamos un poco para que la velocidad alta no importe tanto
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

			//Igual que para android
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

			//Aquí aplicamos el movimiento. Como se puede ver, se da por hecho que el cañon es el abuelo de la camara
			transform.parent.parent.Rotate (xValue, 0f, yValue);
		}

	}

	#endregion

}
