using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Speaker : MonoBehaviour
{
	public GameObject alphabetObject;
	public GameObject topLeft;
	public GameObject top;
	public GameObject topOne;
	public GameObject topRight;
	public GameObject left;
	public GameObject right;
	public GameObject bottomLeft;
	public GameObject bottom;
	public GameObject bottomOne;
	public GameObject bottomRight;
	public float speakerHeight = 0.32f;
	public float bottomLeftWidth = 0.25f;
	public float bottomLeftHeight = 0.14f;
	
	public float letterWidth = 0.08f;
	
	
	Alphabet alphabet;
	
	void speak(string message) {
		float pieceWidth = 0.08f;
		float pieceHeight = 0.08f;
		
		int lineCount = 1;
		int totalWidth = alphabet.width(message);
		int targetWidth = 100;
		Debug.Log("totalWidth: " + totalWidth);
		int estimatedLines = (int) Math.Ceiling(Math.Sqrt(totalWidth) / 10);
		List<string> lines = alphabet.split(message, (totalWidth + 16) / estimatedLines);
		lineCount = lines.Count;
		int maxWidth = (int) (100 * (bottomLeftWidth - pieceWidth));
		foreach(string line in lines) {
			Debug.Log("line: " + line);
			int lineWidth = alphabet.width(line);
			if(lineWidth > maxWidth)
				maxWidth = lineWidth;
		}
		Debug.Log("maxWidth: " + maxWidth);
		
		float startX = transform.position.x;
		float startY = transform.position.y + 0.01f * lines.Count * alphabet.letterHeight + pieceHeight * 2 + speakerHeight + 0.02f;
		
		float currentX = startX;
		float currentY = startY;
		
		GameObject piece = Instantiate(topLeft, Vector3.zero, Quaternion.identity);
		piece.transform.parent = transform;
		piece.transform.position = new Vector3(currentX, currentY, 0);
		currentX += pieceWidth / 2;
		while(currentX < startX - pieceWidth / 2 + 0.01f * maxWidth) {
			currentX += pieceWidth / 2;
			piece = Instantiate(top, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += pieceWidth / 2;
		}
		if(currentX + 0.01f < startX + pieceWidth / 2 + 0.01f * maxWidth) {
			while(currentX + 0.01f < startX + pieceWidth / 2 + 0.01f * maxWidth) {
				currentX += 0.005f;
				piece = Instantiate(topOne, Vector3.zero, Quaternion.identity);
				piece.transform.parent = transform;
				piece.transform.position = new Vector3(currentX, currentY, 0);
				currentX += 0.005f;
			}
		}
		currentX += pieceWidth / 2;
		piece = Instantiate(topRight, Vector3.zero, Quaternion.identity);
		piece.transform.parent = transform;
		piece.transform.position = new Vector3(currentX, currentY, 0);
		currentY -= (pieceHeight + 0.01f * alphabet.letterHeight) / 2;
		
		foreach(string line in lines) {
			currentX = startX;
			piece = Instantiate(left, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += pieceWidth / 2;
			
			string output = alphabet.alignCenter(line, maxWidth);
			Debug.Log(output);
			// Put the letters up there
			foreach(char letter in output) {
				int characterWidth = alphabet.letterWidth(letter.ToString());
				Debug.Log("characterWidth(" + letter + "): " + characterWidth);
				currentX += 0.005f * characterWidth;
				// GameObject letterObject = alphabet.getLetter(letter);
				// letterObject.transform.parent = transform;
				// letterObject.transform.position = new Vector3(currentX, currentY, 0);
				currentX += 0.005f * characterWidth;
			}
			//currentX += 0.01f * maxWidth;
			
			currentX += pieceWidth / 2;
			piece = Instantiate(right, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentY -= 0.01f * alphabet.letterHeight;
		}
		
		currentY += 0.005f * alphabet.letterHeight - pieceHeight / 2;
		currentX = startX + (bottomLeftWidth - pieceWidth) / 2;
		piece = Instantiate(bottomLeft, Vector3.zero, Quaternion.identity);
		piece.transform.parent = transform;
		piece.transform.position = new Vector3(currentX, currentY - (bottomLeftHeight - pieceHeight) / 2, 0);
		currentX += bottomLeftWidth / 2;
		while(currentX < startX - pieceWidth / 2 + 0.01f * maxWidth) {
			currentX += pieceWidth / 2;
			piece = Instantiate(bottom, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += pieceWidth / 2;
		}
		while(currentX + 0.01f < startX + pieceWidth / 2 + 0.01f * maxWidth) {
			currentX += 0.005f;
			piece = Instantiate(bottomOne, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += 0.005f;
		}
		currentX += pieceWidth / 2;
		piece = Instantiate(bottomRight, Vector3.zero, Quaternion.identity);
		piece.transform.parent = transform;
		piece.transform.position = new Vector3(currentX, currentY, 0);
		
		Debug.Log("targetWidth: " + targetWidth);
		Debug.Log("lineCount: " + lineCount);
		
		totalWidth = 16;
		int totalHeight = 16 + alphabet.letterHeight * lineCount;
		
		topLeft = Instantiate(topLeft, Vector3.zero, Quaternion.identity);
		topLeft.transform.parent = transform;
	}
	
    void Start()
    {
		alphabet = alphabetObject.GetComponent<Alphabet>();
        // speak("If you’re looking for crazy stuff out here, ain’t nothing crazier than Old Joe.");
		speak("Hi");
    }
	
    void Update()
    {
        
    }
}
