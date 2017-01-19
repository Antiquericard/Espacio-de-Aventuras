using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	[SerializeField] public int lifes = 3;

    public int level;
	string [] levelTexts;
	public bool vic;
	float lerp;

	public enum ShootingMode : byte {Idle, Aiming, Shooting, Returning};
	public ShootingMode mode;

    [SerializeField]
	public Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f, -0.8f,0.47f);
	//public Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f,1f,-1.5f);
    [SerializeField]
	public Vector3 CAMERA_ASTRONAUT_DISTANCE = new Vector3 (0f,1f,-2.5f);
    [SerializeField]
	public Vector3 ASTRONAUT_CANNON_DISTANCE = new Vector3 (0f, 1f, 0f);


	//OBJECTS

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
	[SerializeField]
	GameObject Victory;
	[SerializeField]
	GameObject Lose;

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
			//DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}

	}

	void Start(){
		level = SceneManager.GetActiveScene ().buildIndex;
		//Creamos un astronauta, solo usaremos ese no tenemos que crear mas
		astronaut = Instantiate (astronautPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		astronaut.GetComponent<Astronaut>().cannon = cannon;
		astronaut.SetActive (false);

	}

	void OnGUI() {
		if (mode == ShootingMode.Aiming) {
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, Screen.width/2, 50), emptyPowerBar);
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, powerValue * Screen.width/2, 50), fullPowerBar);
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (mode) {
		case ShootingMode.Idle:
			if (Input.GetMouseButtonDown (0)) {
				mode = ShootingMode.Aiming;
			}
			break;
		case ShootingMode.Aiming:
			//Apuntando
			if (Input.GetMouseButtonUp (0)) {
				Shoot ();
			} else {
				powerValue += powerIncreaseRate * Time.deltaTime;
				if (powerValue > 1) {
					powerValue--;
				}
			}
			break;
		case ShootingMode.Shooting:
			//Disparando

			mainCamera.GetComponent<CameraMovement> ().wantedPosition = astronaut.transform.position + astronautTrueDistance;
			mainCamera.GetComponent<CameraMovement> ().wantedRotation = astronaut.transform.rotation;
			break;
		case ShootingMode.Returning:
			//Volviendo
			mainCamera.GetComponent<CameraMovement> ().wantedPosition = astronaut.transform.position + astronautTrueDistance;
			mainCamera.GetComponent<CameraMovement> ().wantedRotation = astronaut.transform.rotation;
			break;
		}
	}


	public void Shoot(){

		//Ajustamos la potencia: Un disparo completamente cargado tiene 4 veces más potencia que uno sin cargar
		//Convertimos esta progresión 1-100 a una 25-100

		float adjustedPowerValue = ((powerValue - .01f) * .75f / .99f) + .25f;

		astronaut.SetActive (true);
		astronaut.GetComponent<Astronaut> ().Init (adjustedPowerValue * startingVelocity, spaceShip.GetComponent<SpaceShipMovement>().movement);
		powerValue = 0f;

		//Esto rota la distancia. Es importante que el Quaternion tiene que estar en el LADO IZQUIERDO
		astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;

		mode = ShootingMode.Shooting;

		mainCamera.transform.SetParent(null);

		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Clear ();
		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Stop ();

	}

	public void ReturnToIdleMode(){
		mode = ShootingMode.Idle;
		mainCamera.transform.SetParent (cannon.GetChild(0));
		mainCamera.transform.localPosition = CAMERA_CANNON_DISTANCE;
		mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0f,0f));
		cannon.GetComponentInChildren<ParticleSystem> ().Play ();
	}

    public void CompleteLevel() {
		if (vic) {
			Victory.SetActive(true);
			StartCoroutine(LevelCompleted(Victory));
		} else {
			Lose.SetActive(true);
			StartCoroutine(LevelCompleted(Lose));
		}
    }

	public void NextLevel (){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Level " + (level + 1).ToString ());
	}

	IEnumerator LevelCompleted (GameObject can){
		//Guardamos la partida antes que nada
		SaveGameManager.Save(level);

		Color tmp = can.GetComponent<Image>().color;
		Time.timeScale = 0.2f;
		while(!(tmp.a == 0.75f && can.GetComponentInChildren<Text>().fontSize == 100)){
			lerp += Time.unscaledDeltaTime / 2f;
			tmp.a = Mathf.Lerp (0f, 0.75f, lerp);
			can.GetComponent<Image> ().color = tmp;
			can.GetComponentInChildren<Text> ().fontSize = (int) Mathf.Lerp (10f,100f,lerp);
			yield return null;
		}
	}

	IEnumerator DieCoroutine(){
		yield return new WaitForSeconds (.5f);
		astronaut.GetComponent<Rigidbody> ().velocity = new Vector3 ();
		//particles.Play ();
		if (--GameManager._instance.lifes > 0) {
			astronaut.GetComponent<Astronaut> ().StartCoroutine ("ReturnToSpaceShip");
			astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;
		} else {
			vic = false;
			CompleteLevel ();
		}
			
	}
}
