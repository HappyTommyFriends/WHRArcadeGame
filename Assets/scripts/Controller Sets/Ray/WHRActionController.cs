using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WHRActionController : ActionController
{
	public float openingAnimationDuration = 4.0f;
	protected bool frozen = false;

	void Start() {
		Freeze();
		Invoke("Unfreeze", openingAnimationDuration);
	}

	void Freeze() {
		frozen = true;
	}

	void Unfreeze() {
		frozen = false;
	}

	public override string establishAction(string intent) {
		if(frozen)
			return "idle";

		switch(intent) {
			case "none":
				return "idle";
			case "right":
				return "walkRight";
			case "left":
				return "walkLeft";
			default:
				return intent;
		}
	}
}
