
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

public class SpaceShipMovement : MonoBehaviour {

	#region Setting Attributes

	[Tooltip ("Lista de WayPoints a añadir para establecer la ruta de la spaceShip.")] [SerializeField] Transform[] wayPoints;

	[Tooltip("Velocidad a la cual la spaceShip recorrerá la ruta.")] [SerializeField] float speed = 5f;

	#endregion

	#region Unity Methods

	void Start () {
		if (!(speed == 0f))
			StartCoroutine ("Movement");
	}

	#endregion

	#region Coroutines

	// Se crea una Coroutine para ejecutar el movimiento de un wayPoint a otro cada vez que alcanza el siguiente WayPoint.
	IEnumerator Movement (){

		int index = 0;
		while (true) {
			Vector3 distancia = transform.position - wayPoints [index].position;
			if (distancia.sqrMagnitude == 0) {
				index++;
				index = index % wayPoints.Length;
			}


			/* //Este codigo guardaba la velocidad para poder aplicarla a otros objetos como inercia. Ya no es necesario.
			movement = (wayPoints[index].position - transform.position).normalized * speed;
			*/
			transform.position = Vector3.MoveTowards (transform.position, wayPoints [index].position, speed * Time.deltaTime);
			transform.LookAt (Vector3.zero);

			yield return null;
		}
	}

	#endregion

}
