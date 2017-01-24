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

	private static GameManager _instance;
	public static GameManager instance{
		get{
			return _instance;
		}
	}

	#region Setting Attributes

	// GameObject astronauta.
	[SerializeField]
	GameObject astronaut;

	// Valor de potencia.
	float powerValue = 0f;

	// Distancia real del astronauta.
	Vector3 astronautTrueDistance;

	// Nivel de la escena.
    public int level;

	// Subtitulos de cada nivel, level es el indice
	// TODO ESTO NO DEBERIA ESTAR AQUI DEBERIAMOS GUARDAR LOS TEXTOS DE LOS NIVELES APARTE
	public string [] levelTexts;

	//Variable de suavizado.
	float lerp;

	// Enumerado de los modos de disparo.
	public enum ShootingMode : byte {Idle, Aiming, Shooting, Returning};

	// Modo de disparo.
	public ShootingMode mode;

	//Constantes de posicionamiento de objetos
	[Tooltip("")] [SerializeField] public Vector3 CAMERA_CANNON_DISTANCE = new Vector3 (0f, -0.8f,0.47f);

	[Tooltip("")] [SerializeField] public Vector3 CAMERA_ASTRONAUT_DISTANCE = new Vector3 (0f,1f,-2.5f);

	[Tooltip("")] [SerializeField] public Vector3 ASTRONAUT_CANNON_DISTANCE = new Vector3 (0f, 1f, .5f);


	//Objetos requeridos de introducir
	[Tooltip("Barra vacía.")] [SerializeField] Texture2D emptyPowerBar;

	[Tooltip("Barra llena.")] [SerializeField] Texture2D fullPowerBar;

	[Tooltip("Prefab del astronauta.")] [SerializeField] Object astronautPrefab;

	[Tooltip("BaseCannon de la escena.")] [SerializeField] Transform cannon;

	[Tooltip("Nave espacial de la escena.")] [SerializeField] Transform spaceShip;

	[Tooltip("MainCamera de la escena.")] [SerializeField] Camera mainCamera;

	[Tooltip("Interfaz de victoria.")] [SerializeField] GameObject Victory;

	[Tooltip("Interfaz de derrota.")] [SerializeField] GameObject Lose;


	//Parámetros
	[Tooltip("Vidas totales para el nivel.")] [SerializeField] public int lifes = 3;

	[Tooltip("Velocidad inicial del astronauta al dispararse.")] [SerializeField] float startingVelocity = 100f;

	[Tooltip("Potencia inicial del astronauta al dispararse.")] [SerializeField] float powerIncreaseRate = 1f;

	#endregion

	#region Unity Methods

	//
	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}

	}

	//
	void Start(){
		//Cogemos el nivel
		level = SceneManager.GetActiveScene ().buildIndex;
		//TODO coger el texto

		//Creamos un astronauta, solo usaremos ese no tenemos que crear mas
		astronaut = Instantiate (astronautPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		astronaut.GetComponent<Astronaut>().cannon = cannon;
		astronaut.SetActive (false);

	}

	//
	void Update () {
		switch (mode) {
		case ShootingMode.Idle:
			//Si está esperando...
			if (Input.GetMouseButtonDown (0)) {
				mode = ShootingMode.Aiming;
			}
			break;
		case ShootingMode.Aiming:
			//Apuntando...
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
			//Ya ha disparado!
			mainCamera.GetComponent<CameraMovement> ().wantedPosition = astronaut.transform.position + astronautTrueDistance;
			mainCamera.GetComponent<CameraMovement> ().wantedRotation = astronaut.transform.rotation;
			break;
		case ShootingMode.Returning:
			//Volviendo a la nave
			mainCamera.GetComponent<CameraMovement> ().wantedPosition = astronaut.transform.position + astronautTrueDistance;
			mainCamera.GetComponent<CameraMovement> ().wantedRotation = astronaut.transform.rotation;
			break;
		}
	}
		
	/// <summary>
	/// Representación gráfica de la potencia.
	/// </summary>
	void OnGUI() {
		if (mode == ShootingMode.Aiming) {
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, Screen.width/2, 50), emptyPowerBar);
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, powerValue * Screen.width/2, 50), fullPowerBar);
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Método para realizar el disparo.
	/// </summary>
	public void Shoot(){

		//Ajustamos la potencia: Un disparo completamente cargado tiene 4 veces más potencia que uno sin cargar
		//Convertimos esta progresión 1-100 a una 25-100

		float adjustedPowerValue = ((powerValue - .01f) * .75f / .99f) + .25f;
		astronaut.SetActive (true);
		astronaut.tag = "Player"; //El tag se lo quitamos en el tiro anterior para evitar a los planetas
		astronaut.layer = 0; //Asi podra chocar con los planetas de nuevo
		astronaut.GetComponent<Astronaut> ().Init (adjustedPowerValue * startingVelocity, Vector3.zero);
		powerValue = 0f;

		//Esto rota la distancia. Es importante que el Quaternion tiene que estar en el LADO IZQUIERDO del Vector3 para poder rotarlo
		astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;

		mode = ShootingMode.Shooting;

		//Ponemos la camara arriba del todo de la jerarquia
		mainCamera.transform.SetParent(null);

		//Desactivamos la ayuda para disparar
		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Clear ();
		cannon.GetChild(0).GetComponentInChildren<ParticleSystem> ().Stop ();

	}

	/// <summary>
	/// Se ejecuta cuando el astronauta pierde una vida. Le devuelve a la nave.
	/// </summary>
	public void LaunchFail(){
		astronaut.tag = "PlayerLose"; //asi no chocara con planetas ni activara gravedad en su vuelta
		astronaut.layer = 7; //evita que pueda chocar con planetas
		mode = ShootingMode.Returning;
		//En el caso de los planetas, hay que volver a activar la rotacion de la camara
		Camera.main.GetComponent<CameraMovement> ().allowedRotation = true; 
		StartCoroutine ("DieCoroutine");
	}


	/// <summary>
	/// Método para volver al estado inicial de disparo.
	/// </summary>
	public void ReturnToIdleMode(){
		mode = ShootingMode.Idle;
		mainCamera.transform.SetParent (cannon.GetChild(0));
		mainCamera.transform.localPosition = CAMERA_CANNON_DISTANCE;
		mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0f,0f));
		cannon.GetComponentInChildren<ParticleSystem> ().Play ();
	}

	/// <summary>
	/// Método para completar el nivel.
	/// </summary>
	/// <param name="vic">Si ganó o perdió el nivel</param>
	public void CompleteLevel(bool vic) {
		if (vic) {
			Victory.SetActive(true);
			StartCoroutine(LevelCompleted(Victory));
		} else {
			Lose.SetActive(true);
			StartCoroutine(LevelCompleted(Lose));
		}
    }
		
	/// <summary>
	/// Método para comenzar el siguiente nivel.
	/// </summary>
	public void NextLevel (){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Level " + (level + 1).ToString ());
	}

	#endregion

	#region Coroutines

	/// <summary>
	/// Coroutine para completar el nivel.
	/// </summary>
	/// <param name="can">El objeto canvas de la escena.</param>
	/// <returns>>The coroutine</returns>
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
		
	/// <summary>
	/// Coroutine al perder una vida, si se pierden todas comienza pantalla de derrota.
	/// </summary>
	/// <returns>The coroutine.</returns>
	IEnumerator DieCoroutine(){
		yield return new WaitForSeconds (.5f);
		astronaut.GetComponent<Rigidbody> ().velocity = new Vector3 ();
		//particles.Play ();
		if (--GameManager.instance.lifes > 0) {
			astronaut.GetComponent<Astronaut> ().StartCoroutine ("ReturnToSpaceShip");
			astronautTrueDistance =  astronaut.transform.rotation * CAMERA_ASTRONAUT_DISTANCE;
		} else {
			CompleteLevel (false);
		}
	}

	#endregion

}
