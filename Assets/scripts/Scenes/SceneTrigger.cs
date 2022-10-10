using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
	public GameObject player;
	public SceneController sceneController;
	public string parameter;
	
	private void OnTriggerEnter2D(Collider2D other)
    {
		if(other.name == player.name)
			triggerScene();
    }
	
	private void triggerScene() {
		Debug.Log("triggerScene");
		sceneController.transitionTo(parameter);
	}
}