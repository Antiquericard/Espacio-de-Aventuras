
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

public class FauxGravityAttractor : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("Gravedad con la cual el planeta atraerá a los objetos.")] [SerializeField] float gravity = -10f;

	[Tooltip("Velocidad a la que el objeto rota para posicionarse de pie al caer en un planeta.")] [SerializeField] float speedRotation = 50f;

	[Tooltip("Modo de fuerza a aplicar.")] [SerializeField] ForceMode forceMode = ForceMode.Force;

	#endregion

	#region Setters & Getters

	public ForceMode _forceMode
	{
		get { return forceMode; }
		set { forceMode = value; }
	}

	#endregion

	#region Public Methods

	// Método de atracción de un objeto.
	public void Attract (Transform body) {

		//Guardamos estas variables para coger las normales.
		Vector3 gravityUp = (body.position - transform.position);
		Vector3 bodyUp = body.up;
		Rigidbody rigid = body.GetComponent<Rigidbody> ();

		//Añadimos una fuerza al personaje que haga las veces de atracción hacia el planeta.
		rigid.AddForce(gravityUp.normalized * body.GetComponent<Rigidbody> ().mass * gravity / gravityUp.sqrMagnitude, _forceMode);

		//Rotamos al personaje para que caiga de pie.
		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp.normalized) * body.rotation;

		//Suavizamos la rotación.
		body.rotation = Quaternion.Slerp (body.rotation, targetRotation, speedRotation * Time.deltaTime);

	}

	#endregion

}
