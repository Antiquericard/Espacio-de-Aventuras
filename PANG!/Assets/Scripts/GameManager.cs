using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private GameManager _instance;

	public GameManager instance {
		get {
			return _instance;
		}
		set{
			_instance = instance;
		}
	
	}

	// Vamos a necesitar un método Shoot

	public void Shoot () {

	}

}
