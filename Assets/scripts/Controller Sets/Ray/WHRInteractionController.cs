using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WHRInteractionController : InteractionController
{
	public WHRPlayerController player;
	
    public override void interact(Interaction interaction) {
		// Debug.Log("PlayerInteractionController.interact: " + interaction.type);
		switch(interaction.type) {
			case "attack":
				attack(interaction);
				break;
		}
	}
	
	void attack(Interaction interaction) {
		player.damage(1);
		Vector3 direction = player.transform.position - interaction.source.transform.position;
		// player.rigidBody.AddForce(direction.normalized * 2000f);
	}
}
