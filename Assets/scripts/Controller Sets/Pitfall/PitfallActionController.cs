using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallActionController : ActionController
{
	public override string establishAction(string intent) {
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
