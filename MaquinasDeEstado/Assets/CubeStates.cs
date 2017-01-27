using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStates : MonoBehaviour {

	private delegate void UpdateState ();
	private UpdateState updateCurrentState;

	void Start () {
		updateCurrentState += Move;
	}

	[SerializeField]
	Transform[] wayPoints;
	[SerializeField]
	float speed;
	int index = 0;
	Vector3 offset = new Vector3 (0f,.5f,0f);
	RaycastHit hit;
	[SerializeField]
	Color chaseColor;

	enum States {Patrol, Follow, Chase};
	States currentState = States.Patrol;

	MeshRenderer meshR{
		get{
			return GetComponent<MeshRenderer> ();
		}
	}

	void Update () {
		
		if (Input.GetButtonDown("Jump")) {
			Debug.Log ("ToPatrol");
			ToPatrol ();
		} 
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Debug.Log ("ToFollow");
			ToFollow ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Debug.Log ("ToChase");
			ToChase ();
		}
		updateCurrentState ();
	}

	#region States

	void Move (){
		if (Vector3.Distance (transform.position, wayPoints [index].position) > Mathf.Epsilon) {
			transform.position = Vector3.MoveTowards (transform.position, wayPoints [index].position, speed * Time.deltaTime);
			transform.LookAt (wayPoints [index].position);
		} else {
			index = (index + 1) % wayPoints.Length;
		}
	}

	void Follow (){
		if(Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
			transform.LookAt (hit.point + offset);
	}

	void Chase (){
		if(Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)){
			transform.position = Vector3.MoveTowards (transform.position, hit.point + offset, speed * Time.deltaTime);
			transform.LookAt (hit.point + offset);
		}
	}

	#endregion

	#region Transitions
	void ToPatrol(){
		if (currentState == States.Patrol)
			return;
		else if (currentState == States.Follow)
			updateCurrentState -= Follow;
		else
			updateCurrentState -= Chase;

		currentState = States.Patrol;
		meshR.material.color = Color.green;
		updateCurrentState += Move;
	}

	void ToFollow(){
		if (currentState == States.Follow)
			return;
		else if (currentState == States.Patrol)
			updateCurrentState -= Move;
		else
			updateCurrentState -= Chase;
		
		currentState = (States) 1;
		meshR.material.color = Color.yellow;
		updateCurrentState += Follow;
	}

	void ToChase(){
		if (currentState == States.Chase)
			return;
		else if (currentState == States.Follow)
			updateCurrentState -= Follow;
		else
			updateCurrentState -= Move;
		
		currentState = States.Chase;
		meshR.material.color = Color.red;
		updateCurrentState += Chase;
	}
	#endregion

}
