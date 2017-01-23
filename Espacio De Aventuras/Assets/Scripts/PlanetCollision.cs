using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.collider.CompareTag ("Player")) {
			col.transform.tag = "PlayerLose"; //asi no chocara con planetas ni activara gravedad en su vuelta
			col.transform.gameObject.layer = 7; //evita que pueda chocar con planetas
			GameManager._instance.StartCoroutine ("DieCoroutine");
		}
	}
}
