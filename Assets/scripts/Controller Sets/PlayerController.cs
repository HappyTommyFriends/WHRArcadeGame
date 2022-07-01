using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	protected string intent = "none";
	protected string action = "idle";
    protected string _currentDirection = "right";
    protected int _currentAnimationState;
	protected float horizontalSpeed = 0;
	public Rigidbody2D rigidBody;
 
    public Animator animator;
	public ActionController actionController;
	public IntentController intentController;
	public InteractionController interactionController;
	
    void Start()
    {
        animator = this.GetComponent<Animator>();
		setStartingAnimationState();
		rigidBody = GetComponent<Rigidbody2D>();
    }
	
	protected void setStartingAnimationState() {
		_currentAnimationState = 0;
	}

    protected void Update()
    {
		intent = intentController.establishIntent();
		string newAction = actionController.establishAction(intent);
		if(newAction != action) {
			action = newAction;
			takeAction(action);
		}
		rigidBody.velocity = new Vector2(horizontalSpeed, rigidBody.velocity.y);
    }
	
	protected virtual void takeAction(string action) {
		Debug.Log("takeAction should be overwritten in PlayerController child class.");
	}
}
