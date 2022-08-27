using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour // This should be inheritable at some point, or smartly analyze image to get letter dimensions but for now, fuck it
{
	// Turns out creating separate images with separate masks is surprisingly undersupported in Unity. Gonna' do this the stupid oldschool way with a separate sprite for every fucking letter.
	public GameObject a;
	public GameObject b;
	public GameObject c;
	public GameObject d;
	public GameObject e;
	public GameObject f;
	public GameObject g;
	public GameObject h;
	public GameObject i;
	public GameObject j;
	public GameObject k;
	public GameObject l;
	public GameObject m;
	public GameObject n;
	public GameObject o;
	public GameObject p;
	public GameObject q;
	public GameObject r;
	public GameObject s;
	public GameObject t;
	public GameObject u;
	public GameObject v;
	public GameObject w;
	public GameObject x;
	public GameObject y;
	public GameObject z;
	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject D;
	public GameObject E;
	public GameObject F;
	public GameObject G;
	public GameObject H;
	public GameObject I;
	public GameObject J;
	public GameObject K;
	public GameObject L;
	public GameObject M;
	public GameObject N;
	public GameObject O;
	public GameObject P;
	public GameObject Q;
	public GameObject R;
	public GameObject S;
	public GameObject T;
	public GameObject U;
	public GameObject V;
	public GameObject W;
	public GameObject X;
	public GameObject Y;
	public GameObject Z;
	public GameObject n0;
	public GameObject n1;
	public GameObject n2;
	public GameObject n3;
	public GameObject n4;
	public GameObject n5;
	public GameObject n6;
	public GameObject n7;
	public GameObject n8;
	public GameObject n9;
	public GameObject exclamationPoint;
	public GameObject questionMark;
	public GameObject comma;
	public GameObject period;
	public GameObject image;
	public GameObject hashtag;
	public GameObject space;
	public GameObject apostrophe;
	public GameObject spacer;
	public int letterHeight = 9;
	
	public int letterWidth(string letter) {
		return innerLetterWidth(letter) + 1;
	}
	
	public int innerLetterWidth(string letter) {
		if(letter == spacerCharacter().ToString())
			return 0;
		
		switch(letter) {
			case ".":
			case "'":
			case "’":
			case "i":
			case "l":
				return 1;
			case " ":
			case ",":
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
			case "I":
			case "O":
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
			case "J":
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
		int total = 0;
		foreach(char letter in message){
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
		Debug.Log("alignCenter: " + messageWidth);
		int spaces = messageWidth - width(message);
		while(spaces > width(" ") * 2 - 1) {
			message += " ";
			spaces -= width(" ");
			if(spaces < width(" "))
				continue;
			
			message = " " + message;
			spaces -= width(" ");
		}
		while(spaces > 0) {
			message += spacerCharacter().ToString();
			spaces--;
			
			if(spaces < 1)
				continue;
			
			message = spacerCharacter().ToString() + message;
			spaces--;
		}
		
		Debug.Log(width(message));
		return message;
	}
	
	public char spacerCharacter() {
		return '|';
	}
	
	public GameObject getLetter(char letter) {
		GameObject lo = Instantiate(letterObject(letter), Vector3.zero, Quaternion.identity);
		
		return lo;
	}
	
	private GameObject letterObject(char letter) {
		if(letter == spacerCharacter())
			return spacer;
		
		switch(letter) {
			case ' ':
				return space;
			case 'a':
				return a;
			case 'b':
				return b;
			case 'c':
				return c;
			case 'd':
				return d;
			case 'e':
				return e;
			case 'f':
				return f;
			case 'g':
				return g;
			case 'h':
				return h;
			case 'i':
				return i;
			case 'j':
				return j;
			case 'k':
				return k;
			case 'l':
				return l;
			case 'm':
				return m;
			case 'n':
				return n;
			case 'o':
				return o;
			case 'p':
				return p;
			case 'q':
				return q;
			case 'r':
				return r;
			case 's':
				return s;
			case 't':
				return t;
			case 'u':
				return u;
			case 'v':
				return v;
			case 'w':
				return w;
			case 'x':
				return x;
			case 'y':
				return y;
			case 'z':
				return z;
			case 'A':
				return A;
			case 'B':
				return B;
			case 'C':
				return C;
			case 'D':
				return D;
			case 'E':
				return E;
			case 'F':
				return F;
			case 'G':
				return G;
			case 'H':
				return H;
			case 'I':
				return I;
			case 'J':
				return J;
			case 'K':
				return K;
			case 'L':
				return L;
			case 'M':
				return M;
			case 'N':
				return N;
			case 'O':
				return O;
			case 'P':
				return P;
			case 'Q':
				return Q;
			case 'R':
				return R;
			case 'S':
				return S;
			case 'T':
				return T;
			case 'U':
				return U;
			case 'V':
				return V;
			case 'W':
				return W;
			case 'X':
				return X;
			case 'Y':
				return Y;
			case 'Z':
				return Z;
			case '.':
				return period;
			case '?':
				return questionMark;
			case '!':
				return exclamationPoint;
			case ',':
				return comma;
			case '’':
			case '\'':
				return apostrophe;
			default:
				return hashtag;
		}
	}
}
