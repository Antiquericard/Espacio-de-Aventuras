
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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SeleccionadorNiveles : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("Colocar el botón a repetir según el número de niveles.")] [SerializeField] GameObject boton;

	[Tooltip("Colocar el padre de la posicion en la jerarquía del botón.")] [SerializeField] Transform parent;

	[Tooltip("Total de niveles.")] [SerializeField] private int totalLevels;

	[Tooltip("Array de botones de niveles.")] [SerializeField] GameObject[] buttons;

	[Tooltip("Escenas que no son niveles.")] [SerializeField] byte escenasNoLevel;

	// Posición de inicio.
	private Vector3 startposition = new Vector3 (0f, 130f, 0f);

	// Niveles completados.
	int levelsCompleted;

	#endregion

	#region Unity Methods

	// Se carga la partida
	void Awake(){
		levelsCompleted = SaveGameManager.Load ();
	}

	void Start () {

		totalLevels = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;

		buttons = new GameObject [totalLevels];

		for (int i = 0; i < totalLevels; i++){
			
			GameObject but = (GameObject) Instantiate(boton, parent);
			but.transform.localPosition = new Vector3(startposition.x,startposition.y - 50 * i, startposition.z);
			but.GetComponentInChildren<Text> ().text = (i+escenasNoLevel).ToString ();
			buttons [i] = but;

			if (levelsCompleted >= i) {
				//Si es asi, el nivel fue completado
				string scene = "Level " + (i+escenasNoLevel).ToString ();
				but.GetComponent<Button> ().onClick.AddListener( () => this.gameObject.SetActive(false));
				but.GetComponent<Button> ().onClick.AddListener( () => LoadManager.instance.loadAScene(scene));
				but.GetComponent<Button>().interactable = true; //En principio no es necesaria esta línea, pero por si acaso..
			} else {
				//Si no, el nivel está bloqueado todavía
				but.GetComponent<Button>().interactable = false;
			}
		}
	}

	#endregion

}
