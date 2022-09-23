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
	public GameObject arrow;
	public float speakerHeight = 0.32f;
	public float arrowWidth = 0.17f;
	public float arrowHeight = 0.14f;
	public float bottomLeftWidth = 0.08f;
	public float bottomLeftHeight = 0.08f;
	public float xOffset = -0.08f;
	public float yOffset = -0.04f;
	public float minimumStartX = -100f;
	public float arrowOffset = 0;
	
	public float letterWidth = 0.08f;
	
	
	Alphabet alphabet;
	
	public void speak(string message) {
		clear();
		float pieceWidth = 0.08f;
		float pieceHeight = 0.08f;
		
		int lineCount = 1;
		int totalWidth = alphabet.width(message);
		Debug.Log("totalWidth: " + totalWidth);
		int estimatedLines = (int) Math.Ceiling(Math.Sqrt(totalWidth) / 8);
		List<string> lines = alphabet.split(message, (totalWidth + 16) / estimatedLines);
		lineCount = lines.Count;
		int maxWidth = (int) (100 * (arrowWidth - pieceWidth));
		foreach(string line in lines) {
			int lineWidth = alphabet.width(line);
			if(lineWidth > maxWidth)
				maxWidth = lineWidth;
		}
		
		float startX = transform.position.x + xOffset;
		if(maxWidth > 20) {
			startX -= maxWidth * 0.005f;
			startX += 0.1f;
		}
		Debug.Log("startX: " + startX);
		if(startX < minimumStartX)
			startX = minimumStartX;
		float startY = transform.position.y + yOffset + 0.01f * lines.Count * alphabet.letterHeight + pieceHeight * 2 + speakerHeight + 0.02f;
		
		float currentX = startX;
		float currentY = startY;
		
		// Top Line
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
		
		// Text Lines
		foreach(string line in lines) {
			currentX = startX;
			piece = Instantiate(left, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += pieceWidth / 2;
			
			string output = alphabet.alignCenter(line, maxWidth);
			// Put the letters up there
			foreach(char letter in output) {
				int characterWidth = alphabet.letterWidth(letter.ToString());
				currentX += 0.005f * characterWidth;
				GameObject letterObject = alphabet.getLetter(letter);
				letterObject.transform.parent = transform;
				letterObject.transform.position = new Vector3(currentX, currentY, 0);
				currentX += 0.005f * characterWidth;
			}
			//currentX += 0.01f * maxWidth;
			
			currentX += pieceWidth / 2;
			piece = Instantiate(right, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentY -= 0.01f * alphabet.letterHeight;
		}
		
		//Bottom Line
		bool noArrow = true;
		currentY += 0.005f * alphabet.letterHeight - pieceHeight / 2;
		currentX = startX + (bottomLeftWidth - pieceWidth) / 2;
		piece = Instantiate(bottomLeft, Vector3.zero, Quaternion.identity);
		piece.transform.parent = transform;
		piece.transform.position = new Vector3(currentX, currentY - (bottomLeftHeight - pieceHeight) / 2, 0);
		currentX += bottomLeftWidth / 2;	
		if(maxWidth <= 20) {
			currentX += arrowWidth / 2;
			noArrow = false;
			piece = Instantiate(arrow, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY - (arrowHeight - pieceHeight) / 2, 0);
			currentX += arrowWidth / 2;
		} else  {
			currentX += pieceWidth / 2;
			piece = Instantiate(bottom, Vector3.zero, Quaternion.identity);
			piece.transform.parent = transform;
			piece.transform.position = new Vector3(currentX, currentY, 0);
			currentX += pieceWidth / 2;
		}
		while(currentX < startX - pieceWidth / 2 + 0.01f * maxWidth) {
			if(noArrow && currentX >= transform.position.x + xOffset + arrowOffset) {
				currentX += arrowWidth / 2;
				piece = Instantiate(arrow, Vector3.zero, Quaternion.identity);
				piece.transform.parent = transform;
				piece.transform.position = new Vector3(currentX, currentY - (arrowHeight - pieceHeight) / 2, 0);
				currentX += arrowWidth / 2;
				noArrow = false;
			} else {
				currentX += pieceWidth / 2;
				piece = Instantiate(bottom, Vector3.zero, Quaternion.identity);
				piece.transform.parent = transform;
				piece.transform.position = new Vector3(currentX, currentY, 0);
				currentX += pieceWidth / 2;
			}
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
	}
	
	public void clear() {
		foreach(Transform child in this.transform)
		{
			Destroy(child.gameObject);
		}
	}
	
    void Start()
    {
		alphabet = alphabetObject.GetComponent<Alphabet>();
        // speak("If you’re looking for crazy stuff out here, ain’t nothing crazier than Old Joe.");
		// speak("Test");
    }
	
    void Update()
    {
        
    }
}
