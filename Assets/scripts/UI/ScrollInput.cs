using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollInput : MonoBehaviour
{
    public GameObject go;

    public float holdTimeForRapid = 0.8f;
    public float rapidDelay = 0.10f;
    public string text = "";

    Alphabet _alphabet;
    int _numberOfCharacters;
    int _index = 0;
    Scroller scroller;

    private bool holdingDecrement = false;
    private bool holdingIncrement = false;
    private bool holdingNext = false;
    private bool holdingPrevious = false;
    private GameObject textObject;
    private Receiver receiver;

    public void Build(Alphabet alphabet, int numberOfCharacters) {
      holdTimeForRapid = 0.8f;
      rapidDelay = 0.10f;
      go = new GameObject();
      _alphabet = alphabet;
      _numberOfCharacters = numberOfCharacters;
  		scroller = go.AddComponent<Scroller>();
  		scroller.Build(_alphabet, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz. ");
      scroller.go.transform.parent = go.transform;
      scroller.go.transform.localPosition = new Vector3(0.03f, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    bool decrementPressed() {
      return Input.GetAxisRaw("Vertical") < 0;
    }

    void startDecrement() {
      Log("startDecrement");
      Log(holdTimeForRapid);
      holdingDecrement = true;
      decrement();
      Invoke("decrementTick", holdTimeForRapid);
    }

    void stopDecrement() {
      Log("stopDecrement");
      holdingDecrement = false;
      CancelInvoke("decrementTick");
    }

    void decrementTick() {
      Log("decrementTick");
      Log(rapidDelay);
      decrement();
      Invoke("decrementTick", rapidDelay);
    }

    void decrement() {
      Log("ScrollInput.decrement");
      scroller.decrement();
    }

    bool incrementPressed() {
      return Input.GetAxisRaw("Vertical") > 0;
    }

    void startIncrement() {
      Log("incrementPressed");
      holdingIncrement = true;
      increment();
      Invoke("incrementTick", holdTimeForRapid);
    }

    void stopIncrement() {
      Log("stopIncrement");
      holdingIncrement = false;
      CancelInvoke("incrementTick");
    }

    void incrementTick() {
      Log("incrementTick");
      increment();
      Invoke("incrementTick", rapidDelay);
    }

    void increment() {
      Log("ScrollInput.increment");
      scroller.increment();
    }

    bool nextPressed() {
      return Input.GetAxisRaw("Horizontal") > 0;
    }

    void startNext() {
      holdingNext = true;
      addLetter(scroller.currentLetter());

      Debug.Log("text.Length: " + text.Length);
      if(text.Length >= _numberOfCharacters) {
        finish();
        return;
      }

      scootScroller();
      scroller.setIndex(0);
    }

    void addLetter(string letter) {
      text += letter;
      redisplayText();
    }

    void finish() {
      scroller.clearGoChildren();
      Destroy(scroller);
      clearGoChildren();
      receiver.Receive(new Message(text));
    }

  	public void clearGoChildren() {
  		foreach(Transform child in go.transform)
  		{
  			Destroy(child.gameObject);
  		}
  	}

    void redisplayText() {
      if(textObject != null)
        Destroy(textObject);

      textObject = _alphabet.WordObject(text);
      textObject.transform.parent = scroller.go.transform.parent;
      textObject.transform.localPosition = Vector3.zero;
    }

    void scootScroller() {
      // Debug.Log(scroller.go.transform.localPosition);
      // Debug.Log(scroller.go.transform.position);
      scroller.go.transform.localPosition = new Vector3(scroller.go.transform.localPosition.x + ((float) _alphabet.width(scroller.currentLetter())) / 100, scroller.go.transform.localPosition.y, scroller.go.transform.localPosition.z);

      // Debug.Log(scroller.go.transform.localPosition);
      // Debug.Log(scroller.go.transform.position);
    }

    // Update is called once per frame
    void Update() {
      if(decrementPressed()) {
        if(!holdingDecrement) {
          startDecrement();
        }
      } else {
        if(holdingDecrement) {
          stopDecrement();
        }
      }

      if(incrementPressed()) {
        if(!holdingIncrement) {
          startIncrement();
        }
      } else {
        if(holdingIncrement) {
          stopIncrement();
        }
      }

      if(nextPressed()) {
        if(!holdingNext) {
          startNext();
        }
      } else {
        holdingNext = false;
      }
    }

    public void setReceiver(Receiver receiver) {
      this.receiver = receiver;
    }

    int actionID = 0;
    void Log(string message) {
      actionID++;
      Debug.Log("ScrollInput.Action " + actionID + ": " + message);
    }

    void Log(float message) {
      actionID++;
      Debug.Log("ScrollInput.Action " + actionID + ": " + message);
    }
}
