﻿using UnityEngine;
using System.Collections;

public class SpaceShipMovement : GameManager {

	[Tooltip ("Lista de WayPoints a añadir para establecer la ruta de la spaceShip.")] [SerializeField] Transform[] wayPoints;

	[Tooltip("Velocidad a la cual la spaceShip recorrerá la ruta.")] [SerializeField] float speed = 5f;


	public Vector3 movement = new Vector3();

	void Start () {
		StartCoroutine ("Movement");
	}

	// Creo un Coroutine para ejecutar el movimiento de un wayPoint a otro cada vez que alcanza el siguiente WayPoint.
	IEnumerator Movement (){

		int index = 0;
		while (true) {
			Vector3 distancia = transform.position - wayPoints [index].position;
			if (distancia.sqrMagnitude == 0) {
				index++;
				index = index % wayPoints.Length;
			}

			//Codigo nuevo para saber desde fuera la direccion en la que se esta moviendo en cada momento
			//Esta velocidad esta en unidades/frame
			movement = (wayPoints[index].position - transform.position).normalized * speed;

			transform.position = Vector3.MoveTowards (transform.position, wayPoints [index].position, speed * Time.deltaTime);
			transform.LookAt (Vector3.zero);

			yield return null;
		}
	}
}
