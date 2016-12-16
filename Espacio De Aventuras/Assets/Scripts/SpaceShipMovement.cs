using UnityEngine;
using System.Collections;

public class SpaceShipMovement : MonoBehaviour {

	[Tooltip ("Lista de WayPoints a añadir para establecer la ruta de la spaceShip.")] [SerializeField] Transform[] wayPoints;

	[Tooltip("Velocidad a la cual la spaceShip recorrerá la ruta.")] [SerializeField] float speed = 0.2f;

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
			transform.position = Vector3.MoveTowards (transform.position, wayPoints [index].position, speed);
			transform.LookAt (Vector3.zero);

			yield return null;
		}
	}
}
