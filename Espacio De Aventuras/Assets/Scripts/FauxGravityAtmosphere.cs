﻿using UnityEngine;
using System.Collections;

public class FauxGravityAtmosphere : MonoBehaviour {

	[Tooltip("Introducir el componente del attractor.")] public FauxGravityAttractor attractor;

	void OnTriggerStay (Collider other) {
		//Función de atracción del objeto de este script hacia otros objetos.
		attractor.Attract (other.transform);
	}
}