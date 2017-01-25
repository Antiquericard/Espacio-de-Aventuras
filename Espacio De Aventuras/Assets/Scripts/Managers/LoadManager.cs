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

	[Tooltip("Fondo de carga.")] [SerializeField] Image im;

	[Tooltip("Porcentage cargado.")] [SerializeField] Text percentageText;

	[Tooltip("Slider de carga.")] [SerializeField] GameObject loadImage;

	// Nombre de la escena a la que ir.
	string name;

	#endregion

	#region Private Methods

	// Regresco de la interfaz de usuario.
	void RefreshUI (float percentage) {
		percentageText.text = percentage.ToString ("##0 %");
		im.fillAmount = percentage;
	}

	#endregion

	#region Coroutines

	// Coroutine para cargar la siguiente escena.
	IEnumerator Load () {

		AsyncOperation loadProcess = SceneManager.LoadSceneAsync( name);
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
		loadProcess.allowSceneActivation = true;
	}

	#endregion

	#region Public Methods

	// Método para cargar la escena nameScene.
	public void loadAScene (string nameScene) {
		loadImage.SetActive(true);
		name = nameScene;
		StartCoroutine ("Load");
		//SceneManager.LoadScene (nameScene);
	}

	#endregion

}