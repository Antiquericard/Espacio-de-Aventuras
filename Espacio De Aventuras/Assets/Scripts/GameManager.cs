using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	string [] levelTexts;
	int level = 1;
	/*float sqrMinVelocity;
	BoxCollider bounds;*/

	bool aimingMode = true;

	Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f,1f,-1.5f);

	[SerializeField]
	Texture2D emptyPowerBar;
	[SerializeField]
	Texture2D fullPowerBar;
	[SerializeField]
	Object astronaut;
	[SerializeField]
	Transform cannon;
	[SerializeField]
	Transform spaceShip;
	[SerializeField]
	Camera mainCamera;


	[SerializeField]
	float startingVelocity = 50f;
	[SerializeField]
	float powerIncreaseRate = 1f;
	float powerValue = 0f;

	void Awake(){
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this);
		}
	}

	void OnGUI() {
		if (aimingMode) {
			
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, Screen.width/2, 50), emptyPowerBar);
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, powerValue * Screen.width/2, 50), fullPowerBar);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Shoot ();
		}
		if (aimingMode) {
			powerValue += powerIncreaseRate * Time.deltaTime;
			if (powerValue > 1) {
				powerValue --;
			}
		}
	}


	public void Shoot(){

		GameObject lanzamiento = Instantiate (astronaut, Vector3.zero, Quaternion.identity) as GameObject;
		lanzamiento.GetComponent<Astronaut> ().Init (powerValue * startingVelocity, cannon, spaceShip.GetComponent<SpaceShipMovement>().movement);
		powerValue = 0f;

		aimingMode = false;

		mainCamera.transform.SetParent(lanzamiento.transform);
	}

	public void AimingMode(){
		aimingMode = true;
		mainCamera.transform.SetParent (cannon);
		mainCamera.transform.localPosition = CAMERA_CANNON_DISTANCE;
		mainCamera.transform.localRotation = Quaternion.identity;
	}
}
