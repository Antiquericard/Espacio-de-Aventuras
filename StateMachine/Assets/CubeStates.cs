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
		Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);
		if (Input.GetMouseButtonDown(1)) {
			ToPatrol ();
		} else if (index == wayPoints.Length - 1) {
			index = 0;
			ToFollow ();
		} else if (hit.collider == null) {
			ToChase ();
		}


		/*switch (currentState) {
		case States.Patrol:
			Move ();
			break;
		case States.Follow:
			Follow ();
			break;
		case States.Chase:
			Chase ();
			break;
		default:
			break;
		}*/

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
		if(hit.collider != null){
			transform.LookAt (hit.point + offset);
		}


	}

	void Chase (){
		if(hit.collider != null){
			transform.position = Vector3.MoveTowards (transform.position, hit.point + offset, speed * Time.deltaTime);
			transform.LookAt (hit.point + offset);
		}
	}

	#endregion

	#region Transitions
	void ToPatrol(){
		if (currentState == States.Patrol) {
			return;
		}
		else if (currentState == States.Follow)
			updateCurrentState -= Follow;
		else if (currentState == States.Chase)
			updateCurrentState -= Chase;
			

		currentState = States.Patrol;
		meshR.material.color = Color.green;
		updateCurrentState += Move;
	}

	void ToFollow(){
		if (currentState == States.Follow) {
			return;
		}
		else if (currentState == States.Patrol)
			updateCurrentState -= ToPatrol;
		else if (currentState == States.Chase)
			updateCurrentState -= Chase;
		
		currentState = (States) 1;
		meshR.material.color = new Color (1f, 1f, 0f, 1f);
		updateCurrentState += Follow;
	}

	void ToChase(){
		if (currentState == States.Chase) {
			return;
		}
		else if (currentState == States.Follow)
			updateCurrentState -= Follow;
		else if (currentState == States.Patrol)
			updateCurrentState -= Move;
		
		currentState = States.Chase;
		meshR.material.color = chaseColor;
		updateCurrentState += Chase;
	}
	#endregion

}
