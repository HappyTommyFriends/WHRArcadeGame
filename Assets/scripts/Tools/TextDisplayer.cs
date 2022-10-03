using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayer : MonoBehaviour
{
	public enum Alignment {Left, Center, Right, Justify};
	public string text;
	public Alignment align = Alignment.Center;
	
    public virtual void Display(string message) {
		Debug.Log("TextDisplayer using virtual Display method");
	}
}
