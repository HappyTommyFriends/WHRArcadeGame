using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WHRIntentController : IntentController
{
    public override string establishIntent() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		float a = Input.GetAxisRaw("Fire1");
		
		if(a > 0)
			return "attack" + directionOnly(h, v);
		
		return directionOnly(h, v);
	}
		
	private string directionOnly(float h, float v) {
		if(Input.GetButtonDown("Jump")) {
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
