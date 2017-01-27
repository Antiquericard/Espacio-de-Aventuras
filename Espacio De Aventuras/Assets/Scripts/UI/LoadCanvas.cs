using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCanvas : MonoBehaviour {

	void Start(){
		DontDestroyOnLoad (this.gameObject);
	}
}
