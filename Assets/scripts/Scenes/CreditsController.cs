using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
	public GameObject creditsHolder;
	public SceneController sceneController;
	public SLAnimator slAnimator;
	public float animationDuration = 1.7f;
	public Alphabet alphabet;
	public float startX = -1f;
	public float startY = 1f;
	public float spacing = 0.14f;
	public int scorePixelWidth = 200;
	public float titleY = -1.1f;
	public PlatformEnemyController rat;
	
	protected static string[] credits = { "Silas Hart", "Doomn", "DrSci", "edd!e", "Jon", "Geetz", "PStu", "tomas", "SL" };
	
    void Start() {
		buildCreditsDisplay();
        slAnimator.Animate(creditsHolder, creditsHolder.transform.position, Vector3.zero, animationDuration);
		invokeRatAnimations();
    }
	
	void invokeRatAnimations() {
		Invoke("RatChill", 3f);
		Invoke("RatLeft", 4f);
		Invoke("RatChill", 6f);
		Invoke("RatRight", 8f);
		Invoke("RatChill", 9f);
		Invoke("RatLeft", 12f);
		Invoke("RatPattern1", 17f);
	}
	
	void RatChill() {
		rat.idle();
	}
	
	void RatRight() {
		rat.goRight();
	}
	
	void RatLeft() {
		rat.goLeft();
	}
	
	void RatPattern1() {
		RatChill();
		Invoke("RatPattern1b", 3f);
	}
	
	void RatPattern1b() {
		RatRight();
		Invoke("RatPattern1c", 2f);
	}
	
	void RatPattern1c() {
		RatChill();
		Invoke("RatPattern1d", 3f);
	}
	
	void RatPattern1d() {
		RatLeft();
		Invoke("RatPattern1", 2f);
	}
	
	void buildCreditsDisplay() {
		// GameObject creditsWord = alphabet.CenteredWordObject("Credits", scorePixelWidth);
		// creditsWord.transform.parent = creditsHolder.transform;
		// creditsWord.transform.localPosition = new Vector3(startX, titleY, 0);
		// float currentY = startY;
		// for(int i = 0; i < credits.Length; i++) {
			// GameObject displayObject = alphabet.CenteredWordObject(" " + credits[i] + " ", scorePixelWidth);
			// displayObject.transform.parent = creditsHolder.transform;
			// displayObject.transform.localPosition = new Vector3(startX, currentY, 0);
			// currentY -= spacing;
		// }
		GameObject creditsWord = alphabet.CenteredWordObject("By Silas Hart", scorePixelWidth);
		creditsWord.transform.parent = creditsHolder.transform;
		creditsWord.transform.localPosition = new Vector3(startX, titleY, 0);
	}

    void Update() {
        if(selectButtonPressed())
			sceneController.transitionTo("Menu");
    }
	
	bool selectButtonPressed() {
		if(Input.GetButtonDown("Jump"))
			return true;
		
		return Input.GetAxisRaw("Fire1") > 0;
	}
}
