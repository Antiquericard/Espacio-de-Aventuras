using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeleccionadorNiveles : MonoBehaviour {

	[Tooltip("Colocar el botón a repetir según el número de niveles.")] [SerializeField] GameObject boton;

	[Tooltip("Colocar el padre de la posicion en la jerarquía del botón.")] [SerializeField] Transform parent;

	Vector3 posicion = new Vector3 (0f, 130f, 0f);

	void Start () {
		GameObject but = (GameObject) Instantiate(boton, parent);
		but.transform.localPosition = posicion;
	}
}
