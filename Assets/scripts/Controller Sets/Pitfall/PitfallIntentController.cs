using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallIntentController : IntentController
{
    public override string establishIntent() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		if(v > 0) {
			if(h < 0)
				return "upLeft";
			
			if(h > 0)
				return "upRight";
			
			return "up";
		}
		
		if(v < 0) {
			if(h < 0)
				return "downLeft";
			
			if(h > 0)
				return "downRight";
			
			return "down";
		}
		
		if(h < 0)
			return "left";
		
		if(h > 0)
			return "right";
		
		return "none";
	}
}
