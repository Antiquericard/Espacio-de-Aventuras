
/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya 2017
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

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------

	[SerializeField] GameObject canvas;

	[SerializeField] GameObject cam;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	
	protected virtual void Start () {
		
	}
	
	protected virtual void Update () {
		if (Input.GetButtonDown("Cancel")){
			PauseOrResume ();
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// 
	/// </summary>

	public void PauseOrResume () {
		Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
		canvas.SetActive (!canvas.activeSelf);
		cam.GetComponent<CameraMovement>().enabled = (!cam.GetComponent<CameraMovement>().isActiveAndEnabled);
		this.GetComponent<GameManager> ().enabled = (!this.GetComponent<GameManager> ().isActiveAndEnabled);
	}

	public void ResetLevel () {
		if (Time.timeScale == 0){
			Time.timeScale = 1;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		CancelInvoke ();
	}

}