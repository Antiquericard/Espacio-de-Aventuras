using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaDeEstados : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField] float speed;

	[SerializeField] Transform[] wayPoints;

	[SerializeField] Vector3 offset = new Vector3 (0f,.5f,0f);

	enum States {Patrol, Follow, Chase}

	void Update () {
		switch (States) {
		case States.Patrol:
			break;
		case States.Follow:
			break;
		case States.Chase:
			break;
		}

	}

	RaycastHit hit;

	int index = 0;

	void Move () {
		if (Vector3.Distance (transform.position, wayPoints [index].position) > Mathf.Epsilon) {
			transform.position = Vector3.MoveTowards (transform.position, wayPoints [index].position, speed * Time.deltaTime);
		} else {
			index = (index + 1) % wayPoints.Length;
		}
	}

	void Follow () {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
			transform.LookAt (hit.point + offset);
		}
	}

	void Chase () {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
			transform.position = Vector3.MoveTowards (transform.position, hit.point + offset, speed * Time.deltaTime);
		}
	}
}
