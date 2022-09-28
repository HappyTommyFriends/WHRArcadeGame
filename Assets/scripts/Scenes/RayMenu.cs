using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMenu : IntentController
{
	string intent;
	
    public override string establishIntent() {
		return intent;
	}
	
    // Start is called before the first frame update
    void Start()
    {
		invokeActions();
	}
	
	void invokeActions() {
        intent = "none";
		Invoke("right", 3f);
		Invoke("none", 4f);
		Invoke("right", 9f);
		Invoke("upRight", 9.5f);
		Invoke("right", 10f);
		Invoke("left", 11f);
		Invoke("attackLeft", 12f);
		Invoke("left", 12.5f);
		Invoke("attackLeft", 13f);
		Invoke("left", 13.5f);
		Invoke("attackLeft", 14.2f);
		Invoke("none", 14.7f);
    }
	
	void right() {
		intent = "right";
	}
	
	void none() {
		intent = "none";
	}
	
	void upRight() {
		intent = "upRight";
	}
	
	void left() {
		intent = "left";
	}
	
	void attackLeft() {
		intent = "attackleft";
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
