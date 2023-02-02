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
	public int scorePixelWidth = 50;
	public int namePixelWidth = 40;
	public int centerSpacing = 110;
	public float titleY = -1.1f;

	protected ScrollInput scrollInput;
	protected int editingIndex;
	protected static string[] names = { "Silas", "Doomn", "DrSci", "edd!e", "Geetz", "Jon", "PStu", "SL", "tomas" };
	protected static int[] scores = { 3000, 2000, 2000, 2000, 2000, 2000, 2000, 1000, 1000 };
	protected static float[] positions = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	protected bool enteringScore;

  // Start is called before the first frame update
  void Start() {
		buildScoresDisplay();
		AddHighScore(Persistance.score);
		Persistance.score = 0;
		slAnimator.Animate(highScoresHolder, highScoresHolder.transform.position, Vector3.zero, animationDuration);
	}

	void buildScoresDisplay() {
		clearChildren();
		GameObject highScoresWord = alphabet.WordObject(" High Scores ");
		highScoresWord.transform.parent = highScoresHolder.transform;
		highScoresWord.transform.localPosition = new Vector3(-0.24f, titleY, 0);
		float currentY = startY;
		for(int i = 0; i < scores.Length; i++) {
			positions[i] = currentY;
			GameObject displayObject = alphabet.AlignedLeftWordObject(" " + names[i], namePixelWidth);
			displayObject.transform.parent = highScoresHolder.transform;
			displayObject.transform.localPosition = new Vector3(startX, currentY, 0);
			displayObject = alphabet.CenteredWordObject(" . . . . . . . . . . . . . . . . . . . . . . . . . . ", centerSpacing);
			displayObject.transform.parent = highScoresHolder.transform;
			float cx = startX + ((float) namePixelWidth) / 100;
			displayObject.transform.localPosition = new Vector3(cx, currentY, 0);
			displayObject = alphabet.AlignedRightWordObject(scores[i].ToString(), scorePixelWidth);
			displayObject.transform.parent = highScoresHolder.transform;
			cx += ((float) centerSpacing) / 100;
			displayObject.transform.localPosition = new Vector3(cx, currentY, 0);
			currentY -= spacing;
		}
	}

	void clearChildren() {
		foreach(Transform child in highScoresHolder.transform)
		{
			Destroy(child.gameObject);
		}
	}

  // Update is called once per frame
	void Update()
	{
		if(enteringScore)
			return;

		if(selectButtonPressed())
			sceneController.transitionTo("Menu");
	}

	bool selectButtonPressed() {
		if(Input.GetButtonDown("Jump"))
			return true;

		return Input.GetAxisRaw("Fire1") > 0;
	}

	public bool isHighScore(int score) {
		return scores[scores.Length - 1] < score;
	}

	public bool AddHighScore(int score) {
		if(!isHighScore(score))
			return false;

		enteringScore = true;
		editingIndex = highScoreIndex(score);
		for(int scoreIndex = scores.Length - 2; scoreIndex >= editingIndex; scoreIndex--) {
			scores[scoreIndex + 1] = scores[scoreIndex];
			names[scoreIndex + 1] = names[scoreIndex];
		}

		scores[editingIndex] = score;
		names[editingIndex] = "";
		buildScoresDisplay();
		// ScrollInput scrollInput = new ScrollInput(alphabet, 5);
		scrollInput = highScoresHolder.gameObject.AddComponent<ScrollInput>();
		scrollInput.Build(alphabet, 5);
		HighScoresControllerScrollInputReceiver receiver = new HighScoresControllerScrollInputReceiver(this);
		scrollInput.setReceiver(receiver);
		scrollInput.go.transform.parent = highScoresHolder.transform;
		scrollInput.go.transform.localPosition = new Vector3(startX + 0.03f, positions[editingIndex], -0.5f);
		return true;
	}

	public void EntryComplete(string message) {
		// Debug.Log("EntryComplete");
		// Debug.Log(message);
		names[editingIndex] = scrollInput.text;
		Destroy(scrollInput);
		buildScoresDisplay();
		Invoke("FinishGame", 1f);
	}
	
	public void FinishGame() {
		sceneController.transitionTo("Credits");
		Invoke("ReleaseEnteringFlag", 2f);
	}
	
	public void ReleaseEnteringFlag() {
		enteringScore = false;
	}

	public int highScoreIndex(int score) {
		int index = scores.Length - 1;
		while(score > scores[index] && index >= 0) {
			index--;
			if(index == -1)
				return 0;
		}

		return index + 1;
	}
}
