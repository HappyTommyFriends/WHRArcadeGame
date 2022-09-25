using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinerCoordinator : MonoBehaviour
{
	public Joe joe;
	// public NPC	waitress;
	// public Cook cook;
	public GameObject cup;
	public float cupSpeed = 1f;
	
	Vector3 startingCupPosition;
	bool servingDrink = false;
	bool acceptingDrink = false;
	
    // Start is called before the first frame update
    void Start()
    {
        startingCupPosition = cup.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(joe.orderingDrink) {
			triggerDrinkServeProcess();
		} else {
			if(joe.returningCup)
				triggerDrinkAcceptingProcess();
		}
    }
	
	void triggerDrinkAcceptingProcess() {
		if(acceptingDrink)
			return;
		
		acceptingDrink = true;
		cup.SetActive(true);
		acceptDrink();
	}
	
	void acceptDrink() {
		joe.cupReturned();
		cup.GetComponent<Rigidbody2D>().velocity = new Vector2(-cupSpeed, 0);
		Invoke("stopDrink", 0.96f);
		Invoke("hideDrink", 1.56f);
	}
	
	void triggerDrinkServeProcess() {
		if(servingDrink)
			return;
		
		cup.transform.position = startingCupPosition;
		servingDrink = true;
		cup.SetActive(true);
		Invoke("serveDrink", 0.7f);
	}
	
	void serveDrink() {
		cup.GetComponent<Rigidbody2D>().velocity = new Vector2(cupSpeed, 0);
		//cup.GetComponent<Rigidbody2D>().AddForce(Vector2.right * cupServeForce);
		Invoke("finishDrinkServe", 0.96f);
	}
	
	void finishDrinkServe() {
		stopDrink();
		Invoke("joeReceiveDrink", 0.5f);
	}
	
	void joeReceiveDrink() {
		joe.receiveDrink();
		Invoke("hideDrink", 0.3f);
		Invoke("turnOffServingDrink", 5.3f);
	}
	
	void stopDrink() {
		cup.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
	
	void hideDrink() {
		cup.SetActive(false);
	}
	
	void resetDrink() {
		hideDrink();
		cup.transform.position = startingCupPosition;
	}
	
	void turnOffServingDrink() {
		servingDrink = false;
	}
}
