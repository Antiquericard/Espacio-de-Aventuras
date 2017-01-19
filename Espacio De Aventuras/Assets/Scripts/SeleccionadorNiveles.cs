using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SeleccionadorNiveles : MonoBehaviour {

	[Tooltip("Colocar el botón a repetir según el número de niveles.")] [SerializeField] GameObject boton;

	[Tooltip("Colocar el padre de la posicion en la jerarquía del botón.")] [SerializeField] Transform parent;

	private Vector3 startposition = new Vector3 (0f, 130f, 0f);

	[SerializeField] private int totalLevels;

	[SerializeField] GameObject[] buttons;

	[SerializeField] byte escenasNoLevel;

	int levelsCompleted;

	//Aqui cargamos la partida
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
				but.GetComponent<Button> ().onClick.AddListener( () => SwitchScene._instance.loadAScene(scene));
				but.GetComponent<Button>().interactable = true; //En principio no es necesaria esta línea, pero por si acaso..
			} else {
				//Si no, el nivel está bloqueado todavía
				but.GetComponent<Button>().interactable = false;
			}




			//
		}

	}
}
