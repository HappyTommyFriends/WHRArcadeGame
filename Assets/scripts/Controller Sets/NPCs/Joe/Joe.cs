using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joe : BasicController
{
	public SceneController sceneController;
	public int thirstTrigger = 10;
	public int gulpsPerDrink = 3;
	public float drinkDelay = 4f;
	public float drinkDuration = 2f;
	public float blinkDelay = 3f;
	public float blinkDuration = 0.2f;
	public string[] messages;
	public float turnDelay = 5f;
	
	public bool orderingDrink = false;
	public bool returningCup = false;
	
	private int thirst = 5;
	private float thirstTick = 1f;
	private int drinkAmount = 0;
	private bool cup = false;
	private bool turned = false;
	private int messageIndex = 0;
	private bool turnForced = false;
	
	protected override void takeAction(string action) {
		// Debug.Log("Joe.takeAction " + action);
		lastAction = action;
		
		switch(action) {
			case "free":
				// Do your thug thizz, Joe.
				break;
			case "idle":
				idle();
				break;
		}
		
		if(movementFrozen) {
			horizontalSpeed = 0;
		}
	}

	protected override void inheritedStart() {
		Invoke("tickThirst", thirstTick);
		Invoke("blink", blinkDelay);
		Invoke("toggleTurn", turnDelay);
	}
	
	protected void toggleTurn() {
		if(turned) {
			faceForward();
		} else {
			turn();
		}
		Invoke("toggleTurn", turnDelay);
	}
	
	protected void blink() {
		animator.SetInteger("blinks", 1);
		Invoke("stopBlinking", blinkDuration);
	}
	
	protected void stopBlinking() {
		animator.SetInteger("blinks", 0);
		Invoke("blink", blinkDelay);
	}
	
	protected void tickThirst() {
		// Debug.Log("Joe.drinkAmount: " + drinkAmount);
		// Debug.Log("Joe.thirst: " + thirst);
		// Debug.Log("Joe.tickThirst()");
		if(drinkAmount <= 0) {
			thirst++;
			if(!orderingDrink && thirst >= thirstTrigger)
				orderDrink();
		}
		Invoke("tickThirst", thirstTick);
	}
	
	protected void orderDrink() {
		// Debug.Log("Joe.orderDrink()");
		forceTurn();
		animator.SetInteger("animationState", 1);
		Invoke("turnOnOrderingDrink", 0.9f);
	}
	
	protected void forceTurn() {
		turnForced = true;
		turn();
	}
	
	protected void turnOnOrderingDrink() {
		orderingDrink = true;
	}
	
	protected void turn() {
		// Debug.Log("Joe.turn()");
		turned = true;
		animator.SetBool("isTurned", true);
	}
	
	protected void faceForward() {
		if(turnForced)
			return;
		
		// Debug.Log("Joe.turn()");
		turned = false;
		animator.SetBool("isTurned", false);
	}
	
	public void receiveDrink() {
		// Debug.Log("Joe.receiveDrink()");
		orderingDrink = false;
		setCup(true);
		drinkAmount = gulpsPerDrink;
		drink();
		turnForced = false;
	}
	
	protected void drink() {
		// Debug.Log("Joe.drink()");
		thirst = -1;
		drinkAmount--;
		animator.SetBool("drinking", true);
		Invoke("stopDrinking", drinkDuration);
	}
	
	protected void stopDrinking() {
		// Debug.Log("Joe.stopDrinking()");
		animator.SetBool("drinking", false);
		if(drinkAmount > 0) {
			Invoke("drink", drinkDelay);
		} else {
			returnCup();
		}
	}
	
	protected void setCup(bool c) {
		// Debug.Log("Joe.setCup()");
		cup = c;
		animator.SetBool("hasCup", c);
	}
	
	protected void returnCup() {
		// Debug.Log("Joe.returnCup()");
		returningCup = true;
	}
	
	public void cupReturned() {
		returningCup = false;
		cup = false;
		faceForward();
		animator.SetInteger("animationState", 0);
		animator.SetBool("hasCup", false);
		animator.Play("Joe_Turned_Idle");
	}
	
	private void OnTriggerEnter2D(Collider2D other)
    {
		if(other.name == "Player")
			giveNextSpeech();
    }
	
	private void OnTriggerExit2D(Collider2D other)
    {
		Debug.Log("OnTriggerExit2D");
		if(other.name == "Player")
			stopTalking();
    }
	
	private void giveNextSpeech() {
		talk();
		GetComponent<Speaker>().speak(nextMessage());
	}
	
	private string nextMessage() {
		string message = messages[messageIndex];
		messageIndex++;
		if(messageIndex >= messages.Length) {
			messageIndex = 0;
			sceneController.clearForNext = true;
		}
		
		return message;
	}
	
	private void talk() {
		animator.SetBool("isTalking", true);
	}
	
	private void stopTalking() {
		animator.SetBool("isTalking", false);
		GetComponent<Speaker>().clear();
	}
}
