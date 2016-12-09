using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchScene: MonoBehaviour {

	public void loadAScene (string nameScene) {
		SceneManager.LoadScene (nameScene);
	}
}
