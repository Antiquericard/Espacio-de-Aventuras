using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	string [] levelTexts;
	int level = 1;
	float sqrMinVelocity;
	BoxCollider bounds;

	//0: Modo de apuntar cámara 1: Modo de potencia 2: Modo disparado
	public static short mode = 0;

	[SerializeField]
	Texture2D emptyPowerBar;
	[SerializeField]
	Texture2D fullPowerBar;

	[SerializeField]
	float startingVelocity = 50f;
	[SerializeField]
	Object astronaut;
	[SerializeField]
	Transform cannon;

	[SerializeField]
	float powerIncreaseRate = 1f;
	[SerializeField]
	float powerValue = 0f;

	void OnGUI() {
		if (mode == 1) {
			
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, Screen.width/2, 50), emptyPowerBar);
			GUI.DrawTexture(new Rect(Screen.width/4, Screen.height - 100, powerValue * Screen.width/2, 50), fullPowerBar);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Shoot ();
		}
		if (mode == 1) {
			powerValue += powerIncreaseRate * Time.deltaTime;
			if (powerValue > 1) {
				powerValue --;
			}
		}
	}


	public void Shoot(){
		if (mode == 1) {

			//Fijamos el punto donde crearemos al astronauta
			Vector3 spawnPoint = cannon.FindChild ("SpawnPoint").position;
			GameObject lanzamiento = Instantiate (astronaut, spawnPoint, cannon.rotation) as GameObject;
			float velocity = startingVelocity * powerValue;

			//Esto son vectores básicos. A partir de las posiciones sacamos un vector de direccion
			//lanzamiento.GetComponent<Rigidbody> ().velocity = cannon.GetComponent<Rigidbody> ().velocity;
			Vector3 launchDirection = (spawnPoint - cannon.position).normalized;
			//Y le damos velocidad
			lanzamiento.GetComponent<Rigidbody> ().velocity += startingVelocity * powerValue * launchDirection /*lanzamiento.transform.forward*/;

			//Para el proximo tiro lo dejamos a 0
			powerValue = 0f;

			//Deshabilitado para probar el tiro bien
			//mode = 2;
		} else if (mode == 0) {
			mode = 1;
		}
	}
}
