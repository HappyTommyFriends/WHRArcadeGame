using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDisplay : MonoBehaviour
{
	public TextDisplayer textDisplay;
	public string prefix;
	public string unit;
	public bool showUnit = false;
	
	public void SetInteger(int amount) {
		Set(amount.ToString());
	}
	
	void Set(string variable) {
		textDisplay.Display(prefix + variable);
	}
}
