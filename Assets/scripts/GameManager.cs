using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public int score = 0;
	public GameObject scoreObject;
	Text scoreText;

	bool scoreFrozen = false;

	void OnEnable() {
		EdgeDetectingTileBuilder.reset();
	}
    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreObject.GetComponent<Text>();
		scoreText.text = "Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void addScore(int amount) {
		if(scoreFrozen)
			return;

		score += amount;
		scoreText.text = "Score: " + score.ToString();
	}

	public void SetScore(int amount) {
		score = amount;
		scoreText.text = "Score: " + score.ToString();
	}

	public void FreezeScore() {
		scoreFrozen = true;
	}

	public void UnfreezeScore() {
		scoreFrozen = false;
	}
}
