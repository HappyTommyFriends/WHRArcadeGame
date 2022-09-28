using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresController : MonoBehaviour
{
	public GameObject highScoresHolder;
	public SceneController sceneController;
	public SLAnimator slAnimator;
	public float animationDuration = 1.7f;
	public Alphabet alphabet;
	public float startX = -1f;
	public float startY = 1f;
	public float spacing = 0.14f;
	public int scorePixelWidth = 200;
	public float titleY = -1.1f;
	
	protected static string[] names = { "Silas", "Doomn", "DrSci", "edd!e", "Jon", "Geetz", "PStu", "tomas", "SL" };
	protected static int[] scores = { 1000, 500, 500, 500, 500, 500, 500, 500, 500 };
	
    // Start is called before the first frame update
    void Start()
    {
		buildScoresDisplay();
        slAnimator.Animate(highScoresHolder, highScoresHolder.transform.position, Vector3.zero, animationDuration);
    }
	
	void buildScoresDisplay() {
		GameObject highScoresWord = alphabet.WordObject(" High Scores ");
		highScoresWord.transform.parent = highScoresHolder.transform;
		highScoresWord.transform.localPosition = new Vector3(-0.24f, titleY, 0);
		float currentY = startY;
		for(int i = 0; i < 9; i++) {
			GameObject displayObject = alphabet.CenteredWordObject(" " + names[i] + " . . . . . . . . . . " + scores[i].ToString(), scorePixelWidth);
			displayObject.transform.parent = highScoresHolder.transform;
			displayObject.transform.localPosition = new Vector3(startX, currentY, 0);
			currentY -= spacing;
		}
	}

    // Update is called once per frame
    void Update()
    {
        if(selectButtonPressed())
			sceneController.transitionTo("Menu");
    }
	
	bool selectButtonPressed() {
		if(Input.GetButtonDown("Jump"))
			return true;
		
		return Input.GetAxisRaw("Fire1") > 0;
	}
}
