using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WHYDinerActionController : ActionController
{
	public override string establishAction(string intent) {
		// if(intent.StartsWith("attack"))
			// intent = intent.Split("attack")[1];
		
		return intent;
	}
}
