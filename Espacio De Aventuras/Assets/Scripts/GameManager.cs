/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya & Nicolás Robayo Moreno 2017
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	#region Setting Attributes

	// GameObject astronauta.
	GameObject astronaut;

	// Valor de potencia.
	float powerValue = 0f;

	// Distancia real del astronauta.
	Vector3 astronautTrueDistance;

	// Nivel de la escena.
    public int level;

	// 
	string [] levelTexts;

	// ¿Victoria?
	public bool vic;

	//Variable de suavizado.
	float lerp;

	// Enumerado de los modos de disparo.
	public enum ShootingMode : byte {Idle, Aiming, Shooting, Returning};

	// Modo de disparo.
	public ShootingMode mode;

	[Tooltip("")] [SerializeField] public Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f, -0.8f,0.47f);

	[Tooltip("")] [SerializeField] public Vector3 CAMERA_ASTRONAUT_DISTANCE = new Vector3 (0f,1f,-2.5f);

	[Tooltip("")] [SerializeField] public Vector3 ASTRONAUT_CANNON_DISTANCE = new Vector3 (0f, 1f, 0f);

	//OBJECTS

	[Tooltip("Barra vacía.")] [SerializeField] Texture2D emptyPowerBar;

	[Tooltip("Barra llena.")] [SerializeField] Texture2D fullPowerBar;

	[Tooltip("Prefab del astronauta.")] [SerializeField] Object astronautPrefab;

	[Tooltip("BaseCannon de la escena.")] [SerializeField] Transform cannon;

	[Tooltip("Nave espacial de la escena.")] [SerializeField] Transform spaceShip;

	[Tooltip("MainCamera de la escena.")] [SerializeField] Camera mainCamera;

	[Tooltip("Interfaz de victoria.")] [SerializeField] GameObject Victory;

	[Tooltip("Interfaz de derrota.")] [SerializeField] GameObject Lose;

	//PARAMETROS

	[Tooltip("Vidas totales para el nivel.")] [SerializeField] public int lifes = 3;

	[Tooltip("Velocidad inicial del astronauta al dispararse.")] [SerializeField] float startingVelocity = 100f;

	[Tooltip("Potencia inicial del astronauta al dispararse.")] [SerializeField] float powerIncreaseRate = 1f;

	#endregion

	#region Unity Methods

	//
	void Awake(){
		if (_instance == null) {
			_instance = this;
			//DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}

	}

	//
	void Start(){
		level = SceneManager.GetActiveScene ().buildIndex;
		//Creamos un astronauta, solo usaremos ese no tenemos que crear mas
		astronaut = Instantiate (astronautPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		astronaut.GetComponent<Astronaut>().cannon = cannon;
		astronaut.SetActive (false);

	}

	//
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
				cannon.GetComponent<AudioSource> ().Play ();
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

	// Representación gráfica de la potencia.
	void OnGUI() {
		if (mode == ShootingMode.Aiming) {
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, Screen.width/2, 50), emptyPowerBar);
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, powerValue * Screen.width/2, 50), fullPowerBar);
		}
	}

	#endregion

	#region Public Methods

	// Método para realizar el disparo.
	public void Shoot(){

		//Ajustamos la potencia: Un disparo completamente cargado tiene 4 veces más potencia que uno sin cargar
		//Convertimos esta progresión 1-100 a una 25-100

		float adjustedPowerValue = ((powerValue - .01f) * .75f / .99f) + .25f;

		astronaut.SetActive (true);
		astronaut.GetComponent<Astronaut> ().Init (adjustedPowerValue * startingVelocity, /*spaceShip.GetComponent<SpaceShipMovement>().movement*/ Vector3.zero);
		powerValue = 0f;

		//Esto rota la distancia. Es importante que el Quaternion tiene que estar en el LADO IZQUIERDO
		astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;

		mode = ShootingMode.Shooting;

		mainCamera.transform.SetParent(null);

		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Clear ();
		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Stop ();

	}

	// Método para volver al estado inicial de disparo.
	public void ReturnToIdleMode(){
		mode = ShootingMode.Idle;
		mainCamera.transform.SetParent (cannon.GetChild(0));
		mainCamera.transform.localPosition = CAMERA_CANNON_DISTANCE;
		mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0f,0f));
		cannon.GetComponentInChildren<ParticleSystem> ().Play ();
	}

	// Método para completar el nivel.
    public void CompleteLevel() {
		if (vic) {
			Victory.SetActive(true);
			StartCoroutine(LevelCompleted(Victory));
		} else {
			Lose.SetActive(true);
			StartCoroutine(LevelCompleted(Lose));
		}
    }
		
	// Método para comenzar el siguiente nivel.
	public void NextLevel (){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Level " + (level + 1).ToString ());
	}

	#endregion

	#region Coroutines

	// Coroutine para completar el nivel.
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

	// Coroutine al perder una vida, si se pierden todas comienza pantalla de derrota.
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

	#endregion

}
