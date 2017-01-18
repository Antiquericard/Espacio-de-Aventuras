using UnityEngine;
using System.Collections;

public class FauxGravityAtmosphere : MonoBehaviour {

	[Tooltip("Introducir el componente del attractor.")] public FauxGravityAttractor attractor;

	void OnTriggerStay (Collider other) {
		//Función de atracción del objeto de este script hacia otros objetos.
		if (other.CompareTag ("Player")) {
			Camera.main.GetComponent<CameraMovement> ().allowedRotation = false;
			Debug.Log ("Desactivada la rotación");
		}
			
		attractor.Attract (other.transform);
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			Debug.Log ("Activada la rotación");
			Camera.main.GetComponent<CameraMovement> ().allowedRotation = true;
		}
			
	}
}
