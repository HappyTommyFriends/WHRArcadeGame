using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
	public virtual string establishAction(string intent) {
		switch(intent) {
			case "none":
				return "idle";
			default:
				return intent;
		}
	}
}
