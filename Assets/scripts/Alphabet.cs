using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour // This should be inheritable at some point, or smartly analyze image to get letter dimensions but for now, fuck it
{
	public GameObject image;
	public int letterHeight = 9;
	
	public int letterWidth(string letter) {
		switch(letter) {
			case "i":
			case "l":
				return 1;
			case " ":
			case "j":
			case "s":
				return 2;
			case "c":
			case "k":
			case "n":
			case "o":
			case "p":
			case "r":
			case "t":
			case "v":
			case "x":
			case "y":
			case "D":
			case "E":
			case "F":
				return 3;
			case "b":
			case "d":
			case "e":
			case "f":
			case "g":
			case "h":
			case "q":
			case "u":
			case "z":
			case "B":
			case "C":
			case "G":
			case "H":
				return 4;
			case "a":
			case "m":
			case "w":
			case "A":
				return 5;
			default:
				return 8;
		}
	}
	
	public int width(string message) {
		bool first = true;
		int total = 0;
		foreach(char letter in message){
			if(!first)
				total++;
			first = false;
			total += letterWidth(letter.ToString());
		}
		
		return total;
	}	

	public List<string> split(string message, int maxWidth) {
		List<string> lines = new List<string>();
		
		while(message.Trim().Length > 0) {
			string line = upTo(message, maxWidth);
			lines.Add(line);
			message = message.Substring(message.LastIndexOf(line) + line.Length).Trim();
		}
		
		return lines;
	}
	
	public string upTo(string message, int maxWidth) {
		string currentString = "";
		string[] words = message.Split(' ');
		int currentWidth = 0;
		bool first = true;
		foreach(string word in words) {
			int wordWidth = width(word);
			if(currentWidth + wordWidth > maxWidth)
				return currentString;
			
			if(!first)
				currentString += " ";
			first = false;
			currentString += word;
			currentWidth += wordWidth;
		}
		
		return message;
	}
	
	public string alignLeft(string message, int messageWidth) {
		int spaces = messageWidth - width(message);
		while(spaces < 0) {
			message += " ";
			spaces--;
		}
		
		return message;
	}
	
	public string alignCenter(string message, int messageWidth) {
		int spaces = messageWidth - width(message);
		while(spaces > 0) {
			message += " ";
			spaces -= width(" ");
			if(spaces < 1)
				return message;
			
			message = " " + message;
			spaces -= 2;
		}
		
		return message;
	}
	
	public GameObject getLetter(char letter) {
		GameObject letterObject = Instantiate(image, Vector3.zero, Quaternion.identity);
		
		return letterObject;
	}
}
