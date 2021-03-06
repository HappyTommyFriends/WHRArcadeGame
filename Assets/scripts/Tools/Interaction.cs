using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
	public string type;
	public GameObject source;
	public Dictionary<string, float> floats;
	
	public Interaction(string type, GameObject source) {
		this.type = type;
		this.source = source;
		floats = new Dictionary<string, float>();
	}
	
	public static Interaction basicAttack(GameObject source, float damage) {
		Interaction interaction = new Interaction("attack", source);
		interaction.floats.Add("damage", 1f);
		return interaction;
	}
}
