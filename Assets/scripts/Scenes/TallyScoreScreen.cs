using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyScoreScreen : MonoBehaviour
{
	public WHRPlayerController player;
	public TextDisplayer titleText;
	public VariableDisplay baseScoreText;
	public VariableDisplay healthBonusText;
	public VariableDisplay timeBonusText;
	public VariableDisplay totalScoreText;
	public float maxTallyDuration = 3f;
	public float betweenScoresWait = 0.5f;
	
	public AudioClip tallyNoise;
	public float minimumTallyNoiseDelay = 0.25f;
	
	int baseScore;
	int healthBonus;
	int timeBonus;
	int totalScore;
	int totalBaseScore;
	int totalHealthBonus;
	int totalTimeBonus;
	int totalTotalScore;
	
	int[] baseScoreAnimationAmounts;
	float[] baseScoreAnimationTimes;
	bool animatingBaseScore = false;
	int[] healthBonusAnimationAmounts;
	float[] healthBonusAnimationTimes;
	bool animatingHealthBonus = false;
	int[] timeBonusAnimationAmounts;
	float[] timeBonusAnimationTimes;
	bool animatingTimeBonus = false;
	int[] totalScoreAnimationAmounts;
	float[] totalScoreAnimationTimes;
	bool animatingTotalScore = false;
	
	AudioSource audioSource;
	float tallyNoiseLastPlayed = 0;
	
	private SceneController sceneController;
	
    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		SetupAnimations();
        // titleText.gameObject.SetActive(false);
		baseScoreText.gameObject.SetActive(false);
		healthBonusText.gameObject.SetActive(false);
		timeBonusText.gameObject.SetActive(false);
		totalScoreText.gameObject.SetActive(false);
		// Invoke("Go", 1f);
    }
	
	void SetupAnimations() {
		baseScoreAnimationAmounts = new int[2];
		baseScoreAnimationTimes = new float[2];
		healthBonusAnimationAmounts = new int[2];
		healthBonusAnimationTimes = new float[2];
		timeBonusAnimationAmounts = new int[2];
		timeBonusAnimationTimes = new float[2];
		totalScoreAnimationAmounts = new int[2];
		totalScoreAnimationTimes = new float[2];
	}

    // Update is called once per frame
    void Update()
    {
		if(animatingBaseScore) {
			float delta = (Time.time - baseScoreAnimationTimes[0]) / baseScoreAnimationTimes[1];
			if(delta >= 1f) {
				delta = 1f;
				animatingBaseScore = false;
			} else {
				PlayTallyNoise();
			}
			baseScore = baseScoreAnimationAmounts[0] + (int) ((float) (baseScoreAnimationAmounts[1] - baseScoreAnimationAmounts[0]) * delta);
			baseScoreText.SetInteger(baseScore);
		}
		if(animatingTotalScore) {
			float delta = (Time.time - totalScoreAnimationTimes[0]) / totalScoreAnimationTimes[1];
			if(delta >= 1f) {
				delta = 1f;
				animatingTotalScore = false;
			} else {
				PlayTallyNoise();
			}
			totalScore = totalScoreAnimationAmounts[0] + (int) ((float) (totalScoreAnimationAmounts[1] - totalScoreAnimationAmounts[0]) * delta);
			totalScoreText.SetInteger(totalScore);
		}
		if(animatingHealthBonus) {
			float delta = (Time.time - healthBonusAnimationTimes[0]) / healthBonusAnimationTimes[1];
			if(delta >= 1f) {
				delta = 1f;
				animatingHealthBonus = false;
			} else {
				PlayTallyNoise();
			}
			healthBonus = healthBonusAnimationAmounts[0] + (int) ((float) (healthBonusAnimationAmounts[1] - healthBonusAnimationAmounts[0]) * delta);
			healthBonusText.SetInteger(healthBonus);
		}
		if(animatingTimeBonus) {
			float delta = (Time.time - timeBonusAnimationTimes[0]) / timeBonusAnimationTimes[1];
			if(delta >= 1f) {
				delta = 1f;
				animatingTimeBonus = false;
			} else {
				PlayTallyNoise();
			}
			timeBonus = timeBonusAnimationAmounts[0] + (int) ((float) (timeBonusAnimationAmounts[1] - timeBonusAnimationAmounts[0]) * delta);
			timeBonusText.SetInteger(timeBonus);
		}
    }
	
	public void PlayTallyNoise() {
		if(tallyNoiseLastPlayed + minimumTallyNoiseDelay > Time.time)
			return;
		
		tallyNoiseLastPlayed = Time.time;
		audioSource.PlayOneShot(tallyNoise);
	}
	
	public void SetSceneManager(SceneController controller) {
		sceneController = controller;
	}
	
	public void Go() {
		// audioSource.PlayOneShot(tallyNoise);
		titleText.gameObject.SetActive(true);
		Invoke("TallyBaseScore", betweenScoresWait);
	}
	
	void TallyBaseScore() {
		baseScoreText.gameObject.SetActive(true);
		SetBase(0);
		AddBase(sceneController.gameManager.score);
		// AddBase(500);
	}
	
	void SetBase(int score) {
		baseScore = score;
		totalBaseScore = score;
		baseScoreText.SetInteger(score);
	}
	
	void AddBase(int amount) {
		float wait = betweenScoresWait;
		if(amount > 0) {
			totalBaseScore = totalBaseScore + amount;
			float tallyDuration = maxTallyDuration;
			if(totalBaseScore - baseScore <= 100)
				tallyDuration = 1f;
			if(totalBaseScore - baseScore <= 300)
				tallyDuration = 2f;
			wait += tallyDuration;
			
			baseScoreAnimationAmounts[0] = baseScore;
			baseScoreAnimationAmounts[1] = totalBaseScore;
			baseScoreAnimationTimes[0] = Time.time;
			baseScoreAnimationTimes[1] = tallyDuration;
			animatingBaseScore = true;
		}
		Invoke("GoHealthBonus", wait);
	}
	
	void GoHealthBonus() {
		Debug.Log("GoHealthBonus");
		// audioSource.PlayOneShot(tallyNoise);
		healthBonusText.gameObject.SetActive(true);
		Invoke("TallyHealthBonus", betweenScoresWait);
	}
	
	void TallyHealthBonus() {
		healthBonusText.gameObject.SetActive(true);
		SetHealthBonus(0);
		// AddBase(Persistance.score);
		AddHealthBonus(HealthBonusAmount());
	}
	
	int HealthBonusAmount() {
		int health = (int) player.hp;
		int amount = 25;
		int bonus = 0;
		while(health > 0) {
			bonus += amount;
			if(health % 2 == 1)
				amount += 25;
			health--;
		}
		
		return bonus;
	}
	
	void SetHealthBonus(int bonus) {
		healthBonus = bonus;
		totalHealthBonus = bonus;
		healthBonusText.SetInteger(bonus);
	}
	
	void AddHealthBonus(int amount) {
		Debug.Log("AddHealthBonus: " + amount);
		float wait = betweenScoresWait;
		if(amount > 0) {
			totalHealthBonus = totalHealthBonus + amount;
			float tallyDuration = maxTallyDuration;
			if(totalHealthBonus - healthBonus <= 100)
				tallyDuration = 1f;
			if(totalHealthBonus - healthBonus <= 300)
				tallyDuration = 2f;
			wait += tallyDuration;
			
			healthBonusAnimationAmounts[0] = healthBonus;
			healthBonusAnimationAmounts[1] = totalHealthBonus;
			healthBonusAnimationTimes[0] = Time.time;
			healthBonusAnimationTimes[1] = tallyDuration;
			animatingHealthBonus = true;
		}
		Invoke("GoTimeBonus", wait);
	}
	
	void GoTimeBonus() {
		Debug.Log("GoTimeBonus");
		// audioSource.PlayOneShot(tallyNoise);
		timeBonusText.gameObject.SetActive(true);
		Invoke("TallyTimeBonus", betweenScoresWait);
	}
	
	void TallyTimeBonus() {
		timeBonusText.gameObject.SetActive(true);
		SetTimeBonus(0);
		// AddBase(Persistance.score);
		AddTimeBonus(TimeBonusAmount());
	}
	
	int TimeBonusAmount() {
		int time = (int) player.time;
		if(time < 1)
			return 0;
		
		return time * 5;
		// int amount = 1;
		// int bonus = 0;
		// while(time > 0) {
			// bonus += amount;
			// if(time % 30 == 0)
				// amount += 1;
			// time--;
		// }
		
		// return bonus;
	}
	
	void SetTimeBonus(int bonus) {
		timeBonus = bonus;
		totalTimeBonus = bonus;
		timeBonusText.SetInteger(bonus);
	}
	
	void AddTimeBonus(int amount) {
		Debug.Log("AddTimeBonus: " + amount);
		float wait = betweenScoresWait;
		if(amount > 0) {
			totalTimeBonus = totalTimeBonus + amount;
			float tallyDuration = maxTallyDuration;
			if(totalTimeBonus - timeBonus <= 100)
				tallyDuration = 1f;
			if(totalTimeBonus - timeBonus <= 300)
				tallyDuration = 2f;
			wait += tallyDuration;
			
			timeBonusAnimationAmounts[0] = timeBonus;
			timeBonusAnimationAmounts[1] = totalTimeBonus;
			timeBonusAnimationTimes[0] = Time.time;
			timeBonusAnimationTimes[1] = tallyDuration;
			animatingTimeBonus = true;
		}
		Invoke("TallyTotalScore", wait);
	}
	
	void TallyTotalScore() {
		totalScoreText.gameObject.SetActive(true);
		SetTotal(totalBaseScore);
		// AddTotal(Persistance.score);
		AddTotal(totalHealthBonus + totalTimeBonus);
	}
	
	void SetTotal(int score) {
		totalScore = score;
		totalTotalScore = score;
		totalScoreText.SetInteger(score);
	}
	
	void AddTotal(int amount) {
		float wait = betweenScoresWait;
		if(amount > 0) {
			totalTotalScore = totalTotalScore + amount;
			float tallyDuration = maxTallyDuration;
			if(totalTotalScore - totalScore <= 100)
				tallyDuration = 1f;
			if(totalTotalScore - totalScore <= 300)
				tallyDuration = 2f;
			wait += tallyDuration;
			
			totalScoreAnimationAmounts[0] = totalScore;
			totalScoreAnimationAmounts[1] = totalTotalScore;
			totalScoreAnimationTimes[0] = Time.time;
			totalScoreAnimationTimes[1] = tallyDuration;
			animatingTotalScore = true;
		}
		Invoke("Complete", wait);
	}
	
	public void Complete() {
		sceneController.gameManager.SetScore(totalTotalScore);
		sceneController.FadeOut1();
		gameObject.SetActive(false);
	}
	
	public void SetActive(bool active) {
		gameObject.SetActive(active);
	}
}
