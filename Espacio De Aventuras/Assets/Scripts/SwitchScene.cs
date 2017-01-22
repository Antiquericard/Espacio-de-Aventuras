using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene: MonoBehaviour {

	public static SwitchScene _instance;

	[SerializeField] Image im;
	[SerializeField] Text percentageText;
	[SerializeField] GameObject loadImage;
	string name;

	void Awake(){
		if (_instance == null) {
			_instance = this;
		} else {
			Destroy (this.gameObject);
		}

	}

	void RefreshUI (float percentage) {
		percentageText.text = percentage.ToString ("##0 %");
		im.fillAmount = percentage;
	}

	IEnumerator Load () {

		AsyncOperation loadProcess = SceneManager.LoadSceneAsync( name);
		loadProcess.allowSceneActivation = false;

		float timer = 0f;

		while (timer <= 1f) {
			timer += Time.deltaTime;
			RefreshUI (.25f);
			yield return null;
		}
		while (timer - Time.deltaTime <= 1.5f) {
			timer += Time.deltaTime;
			RefreshUI (.5f);
			yield return null;
		}
		while (loadProcess.progress < .9f || timer - Time.deltaTime <= 2f) {
			timer += Time.deltaTime;
			RefreshUI (loadProcess.progress);
			yield return null;
		}
		loadProcess.allowSceneActivation = true;
	}

	public void loadAScene (string nameScene) {
		loadImage.SetActive(true);
		name = nameScene;
		StartCoroutine ("Load");
		//SceneManager.LoadScene (nameScene);
	}
}
