
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

public class LevelLimits : MonoBehaviour {

	#region Unity Methods

	void Start () {
		GameManager.instance.ReloadVariables (3);
	}

	// Al sobrepasar el límite, retornará a la nave.
	void OnTriggerExit(Collider other){
		if (other.CompareTag("Player") && other.GetComponent<Astronaut>().firing.mode != AstronautFiring.ShootingMode.Returning) {
			other.GetComponent<Astronaut> ().firing.LaunchFail ();
		}
	}

	#endregion

}
