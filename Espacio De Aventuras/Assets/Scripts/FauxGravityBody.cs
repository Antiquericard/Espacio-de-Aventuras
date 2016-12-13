using UnityEngine;
using System.Collections;

public class FauxGravityBody : MonoBehaviour {

	public FauxGravityAttractor attractor;

	private Transform myTransform;

	void Start () {
		myTransform = transform;
	}

	void Update () {
		//Función de atracción del objeto de este script hacia otros objetos.
		attractor.Attract (myTransform);
	}
}
