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

	void Start () {

		totalLevels = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 3;

		buttons = new GameObject [totalLevels];

		for (int i = 0; i < totalLevels; i++){
			
			GameObject but = (GameObject) Instantiate(boton, parent);
			but.transform.localPosition = new Vector3(startposition.x,startposition.y - 50 * i, startposition.z);
			string scene = "Level " + (i + 1).ToString ();
			but.GetComponent<Button> ().onClick.AddListener( () => SceneManager.LoadScene(scene) );
			buttons [i] = but;

		}

	}
}
