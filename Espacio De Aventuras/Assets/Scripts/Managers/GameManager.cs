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

public class GameManager : Singleton<GameManager> {

	#region Setting Attributes
	// Nivel de la escena.
    public int level;

	//Variable de suavizado.
	float lerp;

	[Tooltip("Interfaz de victoria.")] [SerializeField] GameObject victory;

	[Tooltip("Interfaz de derrota.")] [SerializeField] GameObject lose;

	//Parámetros
	[Tooltip("Vidas totales para el nivel.")] [SerializeField] public int lifes = 3;


	#endregion

	#region Unity Methods

	//
	void Start(){
		//Cogemos el nivel
		DontDestroyOnLoad(gameObject);
		level = SceneManager.GetActiveScene ().buildIndex;
	}
		
	#endregion

	#region Public Methods

	/// <summary>
	/// Este método es necesario para recargar todas las variables de cada Manager al empezar un nivel
	/// </summary>
	public void ReloadVariables(){
		this.GetComponent<UIPlayManager>().ReloadVariables ();
		this.victory = GameObject.Find ("Victory");
		this.lose = GameObject.Find ("Lose");
		lifes = 3;
	}

	/// <summary>
	/// Método para completar el nivel.
	/// </summary>
	/// <param name="vic">Si ganó o perdió el nivel</param>
	public void CompleteLevel(bool vic) {
		if (vic) {
			victory.SetActive(true);
			StartCoroutine(LevelCompleted(victory));
		} else {
			lose.SetActive(true);
			StartCoroutine(LevelCompleted(lose));
		}
    }
		
	/// <summary>
	/// Método para comenzar el siguiente nivel.
	/// </summary>
	public void NextLevel (){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Level " + (level + 1).ToString ());
	}

	#endregion

	#region Coroutines

	/// <summary>
	/// Coroutine para completar el nivel.
	/// </summary>
	/// <param name="can">El objeto canvas de la escena.</param>
	/// <returns>>The coroutine</returns>
	IEnumerator LevelCompleted (GameObject can){
		//Guardamos la partida antes que nada
		SaveGameManager.Save(level);

		Color tmp = can.GetComponent<Image>().color;
		Time.timeScale = 0.2f;
		while(!(tmp.a == 0.75f && can.GetComponentInChildren<Text>().fontSize == 100)){
			lerp += Time.unscaledDeltaTime / 2f;
			tmp.a = Mathf.Lerp (0f, 0.75f, lerp);
			can.GetComponent<Image> ().color = tmp;
			can.GetComponentInChildren<Text> ().fontSize = (int) Mathf.Lerp (10f,100f,lerp);
			yield return null;
		}
	}


	#endregion

}
