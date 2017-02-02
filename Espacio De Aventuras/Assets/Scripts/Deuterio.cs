
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
using UnityEngine.UI;

public class Deuterio : MonoBehaviour {

	#region Unity Methods

	// Cuando el Player colicione, activará su animación además de su audio, y se iniciará el menú de victoria.
	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag ("Player")) {
			hit.GetComponent<Animator> ().SetBool ("Recoge", true);
			hit.GetComponent<Animator> ().SetBool ("Recoge", false);
			this.GetComponent<AudioSource> ().Play ();
			GameManager.instance.RemoveDeuterium (gameObject);
		}
	}

	#endregion

	public void SetArrowAlpha(float percentage){
		Image image = this.transform.FindChild ("MarkerArrow").GetChild (0).GetComponent<Image> ();
		Color tmpColor = new Color (image.color.r, image.color.g, image.color.b, percentage);
		image.color = tmpColor;
	}
}
