using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : PlayerController
{
    public float walkSpeed = 2;
	public float height = 0.32f;
	protected AudioSource audioSource;
	
	protected override void takeAction(string action) {
		Debug.Log("BasicController.takeAction " + action);
		lastAction = action;
		
		switch(action) {
			case "idle":
				horizontalSpeed = 0;
				idle();
				break;
		}
		
		if(movementFrozen) {
			horizontalSpeed = 0;
		}
	}
	
	protected virtual void idle() {
		animator.SetInteger("animationState", 0);
	}
}
