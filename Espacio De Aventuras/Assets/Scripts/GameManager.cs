using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	string [] levelTexts;
	int level = 1;
	/*float sqrMinVelocity;
	BoxCollider bounds;*/

	//0: Modo de apuntar cámara 1: Modo de potencia 2: Modo disparado
	public static short mode = 0;

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
	float startingVelocity = 50f;
	[SerializeField]
	float powerIncreaseRate = 1f;
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

			GameObject lanzamiento = Instantiate (astronaut, Vector3.zero, Quaternion.identity) as GameObject;
			lanzamiento.GetComponent<Astronaut> ().Init (powerValue * startingVelocity, cannon, spaceShip.GetComponent<SpaceShipMovement>().movement);
			powerValue = 0f;

			//Deshabilitado para probar el tiro bien
			//mode = 2;
		} else if (mode == 0) {
			mode = 1;
		}
	}
}
