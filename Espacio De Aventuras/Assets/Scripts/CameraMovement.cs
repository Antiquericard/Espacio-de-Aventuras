using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	float scaleRotationX = 0.2f;
	[SerializeField]
	float scaleRotationY = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate (scaleRotationY * Input.GetAxis("Mouse Y"),scaleRotationX * Input.GetAxis("Mouse X"), 0f);
	}
}
