
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPlayManager : MonoBehaviour{

	#region Setting Attributes

	[Tooltip("Canvas de la escena")] [SerializeField] GameObject canvas;

	[Tooltip("MainCamera de la escena")] [SerializeField] GameObject cam;

	[Tooltip("GameObject a recoger en la escena")] [SerializeField] GameObject deuterio;

	#endregion

	#region Unity Methods

	//Si pulsamos la tecla Esc, entraremos en el menú de pausa.
	protected virtual void Update () {
		if (Input.GetButtonDown("Cancel")){
			PauseOrResume ();
		}
	}

           	#endregion

	#region Private Methods

	// Método para entrar en el menú de pausa.
	public void PauseOrResume () {
		Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
		canvas.SetActive (!canvas.activeSelf);
		cam.GetComponent<CameraMovement>().enabled = (!cam.GetComponent<CameraMovement>().isActiveAndEnabled);
		this.GetComponent<GameManager> ().enabled = (!this.GetComponent<GameManager> ().isActiveAndEnabled);
	}

	// Método para reiniciar el nivel.
	public void ResetLevel () {
		Time.timeScale = 1f;
		SwitchScene._instance.loadAScene (SceneManager.GetActiveScene ().name);
		CancelInvoke ();
	}

	// Método para volver al menú de inicio.
	public void ToMainMenu () {
		Time.timeScale = 1f;
		SwitchScene._instance.loadAScene ("Main Menu");
	}

	// Método para pasar al siguiente nivel.
	public void NextLevel (){
		Time.timeScale = 1f;
		SwitchScene._instance.loadAScene ("Level " + (GameManager._instance.level + 1).ToString ());
	}

	#endregion
}