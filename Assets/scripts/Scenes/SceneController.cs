using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour
{
	public static string[] scenes = new string[10];
	public static int sceneIndex = 0;
	
	public GameManager gameManager;
	public PlayerController player;
	public GameObject playerReentryMarker;
	public string lastQueuedSceneParameter;
	public float fadeIterationDelay = 0.4f;
	public bool clearForNext = false;
	public string previousSceneParameter;
	public string scene;
	public bool checkForReplay = true;
	public GameObject fadeOverlay;
	public bool tallyScore = true;
	public TallyScoreScreen tallyScreen;
	public Scorpion scorpion;
	
	bool preventTransition = true;
	
	public void Start() {
		Debug.Log("SceneController.Start()...");
		if(fadeOverlay != null) {
			fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,1f);
			fadeOverlay.transform.localPosition = new Vector3(0, 1f, 1f);
			Invoke("fadeIn2", fadeIterationDelay);
		}
		Invoke("allowTransition", 1.1f);
		if(gameManager == null)
			return;
		
		gameManager.UnfreezeScore();
	}
	
	public void allowTransition() {
		// Debug.Log("SceneController.allowTransition()...");	
		preventTransition = false;
	}
	
	public void Update() {
		if(checkForReplay) {
			checkForStartup();
		}
	}
	
	public void checkForStartup() {
		if(Persistance.isReload(scene))
			replayTasks();
	}
	
	protected void replayTasks() {
		Debug.Log("SceneController.replayTasks()...");
		Invoke("allowTransition", 1.1f);
		// Persistance.load(scene);
		setScore(Persistance.score);
		tallyScore = false;
		if(scene == "Desert 1")
			gameManager.FreezeScore();
		if(playerReentryMarker != null)
			player.transform.position = new Vector3(playerReentryMarker.transform.position.x, playerReentryMarker.transform.position.y, player.transform.position.z);
	}
	
	protected void setScore(int score) {
		if(gameManager == null)
			return;
		
		gameManager.score = 0;
		gameManager.addScore(score);
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
		Debug.Log(preventTransition + ": " + preventTransition);
		if(preventTransition)
			return;
		
		lastQueuedSceneParameter = sceneParameter;
		startTransition();
	}
	
	public void startTransition() {
		Debug.Log("SceneController.startTransition()...");
		if(player != null)
			player.freezeMovement();
		if(tallyScore) {
			tallyScoreScreen();
			return;
		}
		FadeOut1();
	}
	
	public void FadeOut1() {
		if(fadeOverlay != null) {
			fadeOverlay.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0.25f);
			Invoke("fadeOut2", fadeIterationDelay);
		} else {
			Invoke("changeToLastQueuedScene", fadeIterationDelay);
		}
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
	
	public void tallyScoreScreen() {
		Debug.Log("SceneController.tallyScoreScreen...");
		if(scorpion != null)
			scorpion.death();
		
		tallyScreen.SetActive(true);
		tallyScreen.SetSceneManager(this);
		tallyScreen.Go();
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
		preventTransition = true;
		Persistance.store(scene);
		switch(parameter) {
			case "Menu":
				SceneManager.LoadScene(0);
				break;
			case "High Scores":
				SceneManager.LoadScene(1);
				break;
			case "Credits":
				SceneManager.LoadScene(2);
				break;
			case "Desert 1":
				SceneManager.LoadScene(3);
				break;
			case "Diner":
				SceneManager.LoadScene(4);
				break;
			case "Desert 2":
				SceneManager.LoadScene(5);
				break;
			case "Win":
				SceneManager.LoadScene(6);
				break;
		}
	}
}
