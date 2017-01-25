using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextFadeOut : MonoBehaviour {

	float fadeSpeed = 1f;

	void Start () {
		StartCoroutine ("FadeOut");
	}
	

	IEnumerator FadeOut () {
		yield return new WaitForSecondsRealtime (5f);
		while (this.GetComponent<Text> ().color.a >= 0.15f) {
			this.GetComponent<Text> ().color = Color.Lerp (this.GetComponent<Text> ().color, Color.clear, fadeSpeed * Time.deltaTime);
			yield return null;
		}
		Destroy (gameObject);
	}
}
