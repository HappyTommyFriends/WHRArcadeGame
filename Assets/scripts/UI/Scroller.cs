using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public GameObject go;

    Alphabet alphabet;
    char[] characters;
    int index = 0;

    protected GameObject lastLetter;
    protected Blinker blinker;

    public void Build(Alphabet alphabet, string characterSet) {
      go = new GameObject();
      this.alphabet = alphabet;
      characters = characterSet.ToCharArray();
      blinker = Blinker.blink(go);
      replaceLetter();
    }

    public string currentLetter() {
      return characters[index].ToString();
    }

    public void setIndex(int index) {
      this.index = index;
      replaceLetter();
    }

    void replaceLetter() {
      blinker.Pause(1.5f);
      // clearGoChildren();
      Destroy(lastLetter);
      lastLetter = alphabet.getLetter(characters[index]);
      lastLetter.transform.parent = go.transform;
      lastLetter.transform.localPosition = Vector3.zero;
      // Debug.Log("replaceLetter letter.transform.localPosition");
      // Debug.Log(letter.transform.localPosition);
      // Debug.Log("replaceLetter letter.transform.position");
      // Debug.Log(letter.transform.position);
    }

    public void decrement() {
      Debug.Log("Scroller.decrement");
      index--;
      if(index < 0)
        index = characters.Length - 1;
      replaceLetter();
    }

    public void increment() {
      Debug.Log("Scroller.increment");
      index++;
      if(index >= characters.Length)
        index = 0;
      replaceLetter();
    }

  	public void clearGoChildren() {
  		foreach(Transform child in go.transform)
  		{
  			Destroy(child.gameObject);
  		}
  	}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
