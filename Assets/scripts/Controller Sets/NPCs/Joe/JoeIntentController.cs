using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoeIntentController : IntentController
{
	bool talking = false;
	
    public override string establishIntent() {
		if(talking)
			return "talk";
		
		return "free";
	}
}
