﻿
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

	[Tooltip("Canvas con una imagen de carga, texto y un slider")][SerializeField] GameObject loadSlider;

	[Tooltip("Objeto interfaz de usuario de los menús")] [SerializeField] GameObject canvasUI;


	Image loadImage;
	Text percentageText;

	// Nombre de la escena a la que ir.
	string sceneName;

	#endregion

	#region Unity Methods
	void Awake(){
		
		loadImage = loadSlider.transform.GetChild (0).GetComponent<Image> ();
		percentageText = loadImage.transform.GetChild (0).GetComponent<Text> ();
		loadSlider.SetActive (false);
		DontDestroyOnLoad (canvasUI);
	}
	#endregion

	#region Private Methods

	/// <summary>
	/// Refresco de la interfaz de usuario de carga del nivel
	/// .
	/// </summary>
	/// <param name="percentage">Porcentaje de carga de la siguiente escena.</param>
	void RefreshUI (float percentage) {
		percentageText.text = percentage.ToString ("##0 %");
		loadImage.fillAmount = percentage;
	}

	#endregion

	#region Coroutines

	// Coroutine para cargar la siguiente escena.
	IEnumerator Load () {

		// Están puestas las esperas queriendo que se muestre la barra de carga, sino ni se aprecia.

		AsyncOperation loadProcess = SceneManager.LoadSceneAsync(sceneName);
		loadProcess.allowSceneActivation = false;

		float timer = 0f;

		while (timer <= 1f) {
			timer += Time.unscaledDeltaTime;
			RefreshUI (.25f);
			yield return null;
		}
		while (timer - Time.unscaledDeltaTime <= 1.5f) {
			timer += Time.unscaledDeltaTime;
			RefreshUI (.5f);
			yield return null;
		}
		while (loadProcess.progress < .9f || timer - Time.unscaledDeltaTime <= 2f) {
			timer += Time.unscaledDeltaTime;
			RefreshUI (loadProcess.progress);
			yield return null;
		}
		loadProcess.allowSceneActivation = true;
		loadSlider.SetActive (false);
		GameManager.instance.victory.SetActive (false);
		GameManager.instance.lose.SetActive (false);
		UIPlayManager.instance.pause.SetActive (false);
		if (Time.timeScale < 1f) {
			Time.timeScale = 1f;
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Método para cargar la escena nameScene.
	/// </summary>
	/// <param name="nameScene">Nombre de la escena</param>
	public void loadAScene (string nameScene) {
		loadSlider.SetActive (true);
		loadSlider.SetActive (true);
		sceneName = nameScene;
		StartCoroutine ("Load");
	}

	#endregion

}