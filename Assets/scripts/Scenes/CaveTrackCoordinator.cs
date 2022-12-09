using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveTrackCoordinator : MonoBehaviour
{
	public static CaveTrackCoordinator instance;

	public WHRPlayerController player;
	public AudioClip mainTrack;
	public AudioClip caveTrack;
	public AudioClip bossIntro;
	public AudioClip bossTrack;

	public float caveCutoff;
	public float bossCutoff;
	public float bossIntroDuration = 3.0f;

	public Scorpion scorpion;

	public AudioSource source;
	int state = 0;

	private IEnumerator coroutine;

  // Start is called before the first frame update
  void Start()
  {
		instance = this;
		source = GetComponent<AudioSource>();
    playTrack(mainTrack);
  }

  // Update is called once per frame
  void Update() {
		if(state == 4)
			return;

		// Debug.Log("State: " + state);

		if(state == 0) {
			if(player.transform.position.y < caveCutoff) {
				playTrack(caveTrack);
				state = 1;
			}
			return;
		}

		if(state == 1 && player.transform.position.x > bossCutoff) {
			state = 2;
			Time.timeScale = 0;
			playTrack(bossIntro);
			coroutine = BossTime(bossIntroDuration);
      StartCoroutine(coroutine);
			return;
		}

		if(state == 3 && scorpion.isDead()) {
			playTrack(caveTrack);
			state = 4;
		}
  }

	private IEnumerator BossTime(float waitTime) {
		yield return new WaitForSecondsRealtime(waitTime);
		Debug.Log("BossTime");
		Time.timeScale = 1f;
			Debug.Log("BossTime post timeScale");
		playTrack(bossTrack);
		state = 3;
		// This doesn't belong here but cutting some corners in crunch time.
		scorpion.activate();
			Debug.Log("BossTime end");
	}

	void playTrack(AudioClip clip) {
		source.clip = clip;
		source.Play();
	}
}
