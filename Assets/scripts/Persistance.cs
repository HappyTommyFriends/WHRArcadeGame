using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Persistance
{
	public static int score;
	
	static string replayToAdd;
	static Dictionary<string, bool> replays = new Dictionary<string, bool>();
	
	private static System.Timers.Timer aTimer;
	
    public static void store(string scene) {
		Debug.Log("Persistance.store()...");
		replayToAdd = scene;
		delayedAddReplay(scene);
		if(scene == "Diner")
			return;
		
		GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		Debug.Log(gameManager);
		Debug.Log(gameManager.score);
		score = gameManager.score;
	}

	private static void delayedAddReplay(string scene) {
		Debug.Log("Persistance.delayedAddReplay()...");
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(1900);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += _delayedAddReplay;
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
	}
	
	private static void _delayedAddReplay(object source, ElapsedEventArgs e) {
		Debug.Log("Persistance._delayedAddReplay()...");
		if(!replays.ContainsKey(replayToAdd))
			replays.Add(replayToAdd, true);
		
		replays[replayToAdd] = true;
	}
	
	public static void load(string scene) {
		Debug.Log("Persistance.load()...");
		Debug.Log(score);
	}
	
	public static bool isReload(string scene) {
		if(!replays.ContainsKey(scene))
			return false;
		
		bool returnValue = replays[scene];
		replays[scene] = false;
		return returnValue;
	}
}
