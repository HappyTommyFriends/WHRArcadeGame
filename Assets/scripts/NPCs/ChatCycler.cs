using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCycler : MonoBehaviour
{
	public GameObject speaker;
	public GameObject player;
	public string[] messages;
	
	private int messageIndex = 0;
	
	private void giveNextSpeech() {
		Debug.Log("giveNextSpeech");
		speaker.GetComponent<Speaker>().speak(nextMessage());
	}
	
	private string nextMessage() {
		string message = messages[messageIndex];
		messageIndex++;
		if(messageIndex >= messages.Length)
			messageIndex = 0;
		
		return message;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
    {
		if(other.name == player.name)
			giveNextSpeech();
    }
	
	private void OnTriggerExit2D(Collider2D other)
    {
		if(other.name == player.name)
			speaker.GetComponent<Speaker>().clear();
    }
}
