using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	[SerializeField] public int lifes = 3;

	string [] levelTexts;
	int level = 1;
	/*float sqrMinVelocity;
	BoxCollider bounds;*/

	public bool aimingMode = true;

	Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f,1f,-1.5f);
	Vector3 CAMERA_ASTRONAUT_DISTANCE = new Vector3 (0f,1f,-2.5f);

	//PREFABS

	[SerializeField]
	Texture2D emptyPowerBar;
	[SerializeField]
	Texture2D fullPowerBar;
	[SerializeField]
	Object astronautPrefab;
	[SerializeField]
	Transform cannon;
	[SerializeField]
	Transform spaceShip;
	[SerializeField]
	Camera mainCamera;

	//PARAMETROS

	[SerializeField]
	float startingVelocity = 100f;
	[SerializeField]
	float powerIncreaseRate = 1f;


	//VARIABLES
	GameObject astronaut;
	float powerValue = 0f;
	Vector3 astronautTrueDistance;

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
		if (Input.GetMouseButtonDown (0) && aimingMode) {
			Shoot ();
		}
		if (aimingMode) {
			powerValue += powerIncreaseRate * Time.deltaTime;
			if (powerValue > 1) {
				powerValue--;
			}
		} else {
			mainCamera.GetComponent<CameraMovement> ().wantedPosition = astronaut.transform.position + astronautTrueDistance;
			mainCamera.GetComponent<CameraMovement> ().wantedRotation = astronaut.transform.rotation;
		}
	}


	public void Shoot(){

		//Ajustamos la potencia: Un disparo completamente cargado tiene 4 veces más potencia que uno sin cargar
		//Convertimos esta progresión 1-100 a una 25-100

		float adjustedPowerValue = ((powerValue - .01f) * .75f / .99f) + .25f;

		astronaut = Instantiate (astronautPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		astronaut.GetComponent<Astronaut>().cannon = cannon;
		astronaut.GetComponent<Astronaut> ().Init (adjustedPowerValue * startingVelocity, spaceShip.GetComponent<SpaceShipMovement>().movement);
		powerValue = 0f;

		//Esto rota la distancia. Es importante que el Quaternion tiene que estar en el LADO IZQUIERDO
		astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;

		aimingMode = false;

		mainCamera.transform.SetParent(null);

		Debug.Log (cannon.GetComponentInChildren<ParticleSystem> ());
		cannon.GetComponentInChildren<ParticleSystem> ().Clear ();
		cannon.GetComponentInChildren<ParticleSystem> ().Stop ();

	}

	public void AimingMode(){
		aimingMode = true;
		mainCamera.transform.SetParent (cannon);
		mainCamera.transform.localPosition = CAMERA_CANNON_DISTANCE;
		mainCamera.transform.localRotation = Quaternion.identity;
		cannon.GetComponentInChildren<ParticleSystem> ().Play ();
	}

	//Inicialización de métodos hijos.

	public virtual void returnToSpaceShip (){
		
	}

	IEnumerator DieCorutine(){
		yield return new WaitForSeconds (.5f);
		GetComponent<Rigidbody2D> ().isKinematic = true;
		//particles.Play ();
		if (GameManager._instance.lifes > 0)
			GameManager._instance.returnToSpaceShip ();
	}
}
