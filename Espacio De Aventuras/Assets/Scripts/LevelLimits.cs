using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLimits : MonoBehaviour {

	void OnTriggerExit(Collider other){
		GameManager._instance.StartCoroutine ("DieCoroutine");
	}
}
