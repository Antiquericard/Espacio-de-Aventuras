using UnityEngine;
using System.Collections;

public class SpaceShipMovement : MonoBehaviour {

	[SerializeField] Transform[] wayPoints;

	[SerializeField] float speed = 0.2f;

	void Start () {
		//StartCoroutine ("Movement");
	}

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

		/*for (int i = 0; i < wayPoints.Length; i++) {
			transform.position = Vector3.MoveTowards (transform.position, wayPoints[i].position, speed);
			transform.LookAt (Vector3.zero);
		}
		yield return null;
		*/
		/*int index = 0;
		while (true) {
			Vector3 distancia = transform.position - wayPoints[index].position;
			if (distancia.sqrMagnitude < Mathf.Epsilon) {
				index = Random.Range (0, wayPoints.Length);
			}
			transform.position = Vector3.MoveTowards (transform.position, wayPoints[index].position, speed);
			transform.LookAt (Vector3.zero);
			yield return null;
		}*/
	}
}
