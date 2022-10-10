using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveTrackCoordinator : MonoBehaviour
{
	public WHRPlayerController player;
	public AudioClip mainTrack;
	public AudioClip caveTrack;
	public AudioClip bossTrack;
	
	public float caveCutoff;
	public float bossCutoff;
	
	public Scorpion scorpion;
	
	AudioSource source;
	int state = 0;
	
    // Start is called before the first frame update
    void Start()
    {
		source = GetComponent<AudioSource>();
        playTrack(mainTrack);
    }

    // Update is called once per frame
    void Update()
    {
		if(state == 3)
			return;
		
		if(state == 0) {
			if(player.transform.position.y < caveCutoff) {
				playTrack(caveTrack);
				state = 1;
			}
			return;
		}
			
		if(state == 1 && player.transform.position.x > bossCutoff) {
			playTrack(bossTrack);
			state = 2;
			// This doesn't belong here but cutting some corners in crunch time.
			scorpion.activate();
			return;
		}
		
		if(state == 2 && scorpion.isDead()) {
			playTrack(caveTrack);
			state = 3;
		}
    }
	
	void playTrack(AudioClip clip) {
		source.clip = clip;
		source.Play();
	}
}