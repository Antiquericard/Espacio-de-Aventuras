
/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya & Nicolas Robayo 2017
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

public class Recoger : MonoBehaviour {

	#region Unity Methods

	// Cuando el Player colicione, activará su animación además de su audio, y se iniciará el menú de victoria.
	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			hit.GetComponent<Animator> ().SetBool ("Recoge", true);
			hit.GetComponent<Animator> ().SetBool ("Recoge", false);
			this.GetComponent<AudioSource> ().Play ();
            GameManager.instance.CompleteLevel(true);
		}
	}

	#endregion
}
