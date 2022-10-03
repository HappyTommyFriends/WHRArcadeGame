using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayerAlphabet : TextDisplayer
{
	public Alphabet alphabet;
	public int pixelWidth = 200;
	
	GameObject messageObject;
	
	void Start() {
		Redraw();
	}
	
    public override void Display(string message) {
		Debug.Log("TextDisplayerAlphabet.Display");
		text = message;
		Redraw();
	}
	
	void Redraw() {
		if(messageObject != null)
			Destroy(messageObject);
		messageObject = GetMessage();
		messageObject.transform.parent = transform;
		messageObject.transform.localPosition = Vector3.zero;
	}
	
	GameObject GetMessage() {
		switch(align) {
			case Alignment.Center:
				return alphabet.CenteredWordObject(text, pixelWidth);
			case Alignment.Justify:
				return alphabet.JustifiedWordObject(text, pixelWidth);
			case Alignment.Left:
				return alphabet.AlignedLeftWordObject(text, pixelWidth);
		}
		
		return null;
	}
}
