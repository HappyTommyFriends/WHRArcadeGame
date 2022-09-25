using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public PlayerController player;
	public GameObject fadeOverlay;
	public string lastQueuedSceneParameter;
	public float fadeIterationDelay = 0.4f;
	public bool clearForNext = false;
	public string previousSceneParameter;
	
	public void Start() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,1f);
		fadeOverlay.transform.localPosition = new Vector3(0, 1f, 1f);
		Invoke("fadeIn2", fadeIterationDelay);
	}
	
	public void fadeIn2() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.75f);
		Invoke("fadeIn3", fadeIterationDelay);
	}
	
	public void fadeIn3() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.5f);
		Invoke("fadeIn4", fadeIterationDelay);
	}
	
	public void fadeIn4() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.25f);
		Invoke("fadeIn5", fadeIterationDelay);
	}
	
	public void fadeIn5() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0f);
	}
	
	public void transitionTo(string sceneParameter) {
		Debug.Log("SceneController.transitionTo: " + sceneParameter);
		lastQueuedSceneParameter = sceneParameter;
		startTransition();
	}
	
	public void startTransition() {
		Debug.Log("SceneController.startTransition()...");
		player.freezeMovement();
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.25f);
		Invoke("fadeOut2", fadeIterationDelay);
	}
	
	public void fadeOut2() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.5f);
		Invoke("fadeOut3", fadeIterationDelay);
	}
	
	public void fadeOut3() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.75f);
		Invoke("fadeOut4", fadeIterationDelay);
	}
	
	public void fadeOut4() {
		fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,1f);
		changeToLastQueuedScene();
	}
	
	public void changeToLastQueuedScene() {
		if(!clearForNext) {
			changeToSceneParameter(previousSceneParameter);
			return;
		}
		changeToSceneParameter(lastQueuedSceneParameter);
	}
	
	public void changeToSceneParameter(string parameter) {
		Debug.Log("Changing to scene according to parameter " + parameter);
		switch(parameter) {
			case "Desert 1":
				SceneManager.LoadScene(0);
				break;
			case "Diner":
				SceneManager.LoadScene(1);
				break;
			case "Desert 2":
				SceneManager.LoadScene(2);
				break;
		}
	}
}
