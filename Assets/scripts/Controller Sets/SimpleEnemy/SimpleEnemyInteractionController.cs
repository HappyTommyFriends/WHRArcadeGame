using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyInteractionController : InteractionController
{
	// public PitfallPlayerController player;
	
    public override void interact(Interaction interaction) {
		Debug.Log("SimpleEnemyInteractionController.interact: " + interaction.type);
		switch(interaction.type) {
			case "attack":
				attack(interaction);
				break;
		}
	}
	
	void attack(Interaction interaction) {
		// Debug.Log("SimpleEnemyInteractionController.attack");
		// Debug.Log(this);
		// Debug.Log(this.GetComponent<PlatformEnemyController>());
		this.GetComponent<PlatformEnemyController>().damage(interaction.floats["damage"]);
		// Vector3 direction = player.transform.position - interaction.source.transform.position;!
		// player.rigidBody.AddForce(direction.normalized * 2000f);
	}
}
