
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

public class CanvasConvert: MonoBehaviour {

	#region Setting Attributes

	[Tooltip("GameObjet objetivo.")] [SerializeField] GameObject target;

	[Tooltip("Elemento gráfico a mostrar.")] [SerializeField] RectTransform UIElement;

	[Tooltip("MainCamera de la escena.")] [SerializeField] Camera camera;

	[Tooltip("Canvas de la escena.")] [SerializeField] Canvas canvas;

	[Tooltip("Elevación extra para el elemento gráfico.")] [SerializeField] float offsetY;

	// RectTransform base.
	RectTransform CanvasRect;

	#endregion

	#region Unity Methods

	// Inicializamos el RectTransform.
	void Awake() {
		//first you need the RectTransform component of your canvas
		CanvasRect = canvas.GetComponent<RectTransform> ();
	}

	// Se actualiza le ubicación en la interfaz del objetivo.
	void LateUpdate(){
		
		Vector2 ViewportPosition = camera.WorldToViewportPoint (target.transform.position);
		Vector2 TargetScreenPosition = new Vector2 (
			                                   ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
			                                   ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)) + offsetY);

		UIElement.anchoredPosition = TargetScreenPosition;
	}

	#endregion

}
