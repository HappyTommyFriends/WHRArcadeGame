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
	protected float verticalSpeed = 0;
	public Rigidbody2D rigidBody;
	public bool movesVertically = false;
	public bool xBounded = false;
	public float minX = 0;
	public float maxX = 0;
	public bool yBounded = false;
	public float minY = 0;
	public float maxY = 0;
 
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
		enforceBounds();
		rigidBody.velocity = new Vector2(horizontalSpeed, nextVerticalSpeed());
    }
	
	protected void enforceBounds() {
		float[] usableBounds = bounds();
		if(yBounded) {
			if(transform.position.y > usableBounds[3])
				transform.position = new Vector3(transform.position.x, usableBounds[3], transform.position.z);
			
			if(transform.position.y < usableBounds[2])
				transform.position = new Vector3(transform.position.x, usableBounds[2], transform.position.z);
		}
		if(xBounded) {
			if(transform.position.x > usableBounds[1])
				transform.position = new Vector3(usableBounds[1], transform.position.y, transform.position.z);
			
			if(transform.position.x < usableBounds[0])
				transform.position = new Vector3(usableBounds[0], transform.position.y, transform.position.z);
		}
	}
	
	protected float nextVerticalSpeed() {
		float presumedVelocity = movesVertically ? verticalSpeed : rigidBody.velocity.y;
		if(!yBounded)
			return presumedVelocity;
		
		if(transform.position.y >= maxY) {
			if(presumedVelocity > 0)
				return 0;
			
			return presumedVelocity;
		}
		
		if(transform.position.y <= minY) {
			if(presumedVelocity < 0)
				return 0;
			
			return presumedVelocity;
		}
		
		return presumedVelocity;
	}
	protected virtual float[] bounds() {
		float[] b = { minX, maxX, minY, maxY };
		return b;
	}
	
	protected virtual void takeAction(string action) {
		Debug.Log("takeAction should be overwritten in PlayerController child class.");
	}
}
