using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	string [] levelTexts;
	int level = 1;
	float sqrMinVelocity;
	BoxCollider bounds;

	//0: Modo de apuntar cámara 1: Modo de potencia 2: Modo disparado
	public static short mode = 0;

	[SerializeField]
	float startingVelocity = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
