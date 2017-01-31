using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button),typeof(AudioSource))]
public class MenuButton : MonoBehaviour {

	public void PlaySound(){
		if (this.GetComponent<Button> ().IsInteractable ()) {
			this.GetComponent<AudioSource> ().Play ();
		}
	}
}
