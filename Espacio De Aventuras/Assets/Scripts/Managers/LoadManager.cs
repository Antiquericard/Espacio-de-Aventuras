
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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager: Singleton<LoadManager> {

	#region Setting Attributes

	[Tooltip("Canvas con una imagen de carga, texto y un slider bien colocados")][SerializeField] Object canvasPrefab;

	GameObject canvas;
	Image loadImage;
	Text percentageText;
	GameObject loadSlider;

	// Nombre de la escena a la que ir.
	string sceneName;

	#endregion

	#region Unity Methods
	void Awake(){
		canvas = Instantiate (canvasPrefab) as GameObject;
		loadSlider = canvas.transform.GetChild (0).gameObject;
		loadImage = loadSlider.transform.GetChild (0).GetComponent<Image> ();
		percentageText = loadImage.transform.GetChild (0).GetComponent<Text> ();
		canvas.SetActive (false);
	}
	#endregion

	#region Private Methods

	// Regresco de la interfaz de usuario.
	void RefreshUI (float percentage) {
		percentageText.text = percentage.ToString ("##0 %");
		loadImage.fillAmount = percentage;
	}

	#endregion

	#region Coroutines

	// Coroutine para cargar la siguiente escena.
	IEnumerator Load () {

		AsyncOperation loadProcess = SceneManager.LoadSceneAsync(sceneName);
		loadProcess.allowSceneActivation = false;

		float timer = 0f;

		while (timer <= 1f) {
			timer += Time.deltaTime;
			RefreshUI (.25f);
			yield return null;
		}
		while (timer - Time.deltaTime <= 1.5f) {
			timer += Time.deltaTime;
			RefreshUI (.5f);
			yield return null;
		}
		while (loadProcess.progress < .9f || timer - Time.deltaTime <= 2f) {
			timer += Time.deltaTime;
			RefreshUI (loadProcess.progress);
			yield return null;
		}
		//GameManager.instance.ReloadVariables ();
		loadProcess.allowSceneActivation = true;
		canvas.SetActive (false);
	}

	#endregion

	#region Public Methods

	// Método para cargar la escena nameScene.
	public void loadAScene (string nameScene) {
		loadSlider.SetActive (true);
		canvas.SetActive (true);
		sceneName = nameScene;
		StartCoroutine ("Load");
	}

	#endregion

}
