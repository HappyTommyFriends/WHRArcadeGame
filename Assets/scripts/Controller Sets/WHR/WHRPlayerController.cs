using UnityEngine;
using System.Collections;
 
public class WHRPlayerController : PlayerController {
    public float walkSpeed = 2;
	public float jumpStrength = 180;
	public float hp = 10;
	public float attackDelay = 0.8f;
	public float attackRange = 0.08f;
	public float attackPower = 1f;
	public HPDisplayHearts hpDisplay;
	
	bool _attacking = false;
	float lastJump = 0;
	Vector3 attackOrigin = new Vector3(0, 0.16f, 0);

	// Ghosts and Goblins
    const int STATE_IDLE = 0;
    const int STATE_CLIMB_UP = 1;
    const int STATE_WALK_RIGHT = 2;
    const int STATE_CLIMB_DOWN = 3;
    const int STATE_WALK_LEFT = 4;
	const int STATE_ATTACK = 10;
	const int STATE_ATTACK_UP = 11;
	const int STATE_ATTACK_RIGHT = 12;
	const int STATE_ATTACK_LEFT = 14;
	const int STATE_ATTACK_DOWN = 13;
	const int STATE_JUMP = 20;
	const int STATE_JUMP_RIGHT = 22;
	const int STATE_JUMP_LEFT = 24;
	const int STATE_DAMAGE_UP = 31;
	const int STATE_DAMAGE_RIGHT = 32;
	const int STATE_DAMAGE_DOWN = 33;
	const int STATE_DAMAGE_LEFT = 34;
	const int STATE_INTERACT_UP = 41;
	const int STATE_INTERACT_RIGHT = 42;
	const int STATE_INTERACT_DOWN = 43;
	const int STATE_INTERACT_LEFT = 44;
	const int STATE_SURPRISED_UP = 51;
	const int STATE_SURPRISED_RIGHT = 52;
	const int STATE_SURPRISED_DOWN = 53;
	const int STATE_SURPRISED_LEFT = 54;
	const int STATE_SMALL_IDLE = 61;
	const int STATE_MEDIUM_IDLE = 62;
	const int STATE_BIG_IDLE = 63;
	
	new void setStartingAnimationState() {
		_currentAnimationState = STATE_IDLE;
		updateHPDisplay();
	}
	
	protected override void takeAction(string action) {
		// Debug.Log("takeAction " + action);
		
		switch(action) {
			case "attackdown":
				attemptAttackDown();
				break;
			case "attacknone":
				attemptAttackCurrentDirection();
				break;
			case "attackupLeft":
			case "attackleft":
				attemptAttackLeft();
				break;
			case "attackupRight":
			case "attackright":
				attemptAttackRight();
				break;
			case "attackup":
				attemptAttackUp();
				break;
			case "idle":
				horizontalSpeed = 0;
				idle();
				break;
			case "walkRight":
				walkRight();
				break;
			case "walkLeft":
				walkLeft();
				break;
			case "upRight":
				jumpRight();
				break;
			case "up":
				horizontalSpeed = 0;
				attemptJump();
				break;
			case "upLeft":
				jumpLeft();
				break;
		}
	}
	
	void attemptAttackCurrentDirection() {
		switch(_currentDirection) {
			case "down":
				attemptAttackDown();
				break;
			case "left":
				attemptAttackLeft();
				break;
			case "right":
				attemptAttackRight();
				break;
			case "up":
				attemptAttackUp();
				break;
		}
	}
	
	void attemptAttackDown() {
		if(canAttack())
			attackDown();
	}
	
	void attemptAttackLeft() {
		if(canAttack())
			attackLeft();
	}
	
	void attemptAttackRight() {
		if(canAttack())
			attackRight();
	}
	
	void attemptAttackUp() {
		if(canAttack())
			attackUp();
	}
	
	bool canAttack() {
		if(_attacking)
			return false;
		
		_attacking = true;
		Invoke("releaseAttack", attackDelay);
		return true;
	}
	
	void releaseAttack() {
		_attacking = false;
	}
	
	void attackDown() {
		changeState(STATE_ATTACK_DOWN);
		executeAttack(Vector3.down);
	}
	
	void executeAttack(Vector3 direction) {
		if(grounded())
			horizontalSpeed = 0;
		// Debug.DrawLine(transform.position + attackOrigin, transform.position + attackOrigin + direction.normalized * attackRange);
		RaycastHit2D hit = Physics2D.Raycast(transform.position + attackOrigin, direction, attackRange);
		if(hit.collider != null)
			handleAttackConnect(hit.collider);
	}
	
	void handleAttackConnect(Collider2D collider) {
		Debug.Log("handleAttackConnect");
		Debug.Log(collider);
		Debug.Log(collider.GetComponent<InteractionController>());
		collider.GetComponent<InteractionController>().interact(Interaction.basicAttack(this.gameObject, attackPower));
	}
	
	void attackLeft() {
		changeState(STATE_ATTACK_LEFT);
		executeAttack(Vector3.left);
	}
	
	void attackRight() {
		changeState(STATE_ATTACK_RIGHT);
		executeAttack(Vector3.right);
	}
	
	void attackUp() {
		changeState(STATE_ATTACK_UP);
		executeAttack(Vector3.up);
	}
	
	void jumpLeft() {
		horizontalSpeed = -walkSpeed;
		_currentDirection = "left";
		attemptJump();
	}
	
	void jumpRight() {
		horizontalSpeed = walkSpeed;
		_currentDirection = "right";
		attemptJump();
	}
	
	void walkRight() {
		_currentDirection = "right";
		horizontalSpeed = walkSpeed;
		changeState(STATE_WALK_RIGHT);
	}
	
	void walkLeft() {
		_currentDirection = "left";
		horizontalSpeed = -walkSpeed;
		changeState(STATE_WALK_LEFT);
	}
	
	void idle() {
		changeState(STATE_IDLE);
	}
	
	void attemptDown() {
		// TODO
	}	
	
	void attemptJump() {
		if(lastJump + 0.1 > Time.realtimeSinceStartup)
			return;
		
		if(grounded())
			jump();
	}
	
	bool grounded() {
		if(rigidBody.velocity.y != 0)
			return false;
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 0.02f));
        return (hit.collider != null);
	}
	
	void jump() {
	   rigidBody.AddForce(new Vector2(0, jumpStrength));
       changeState(jumpState());
	   lastJump = Time.realtimeSinceStartup;
	}
	
	int jumpState() {
		switch (_currentDirection) {
			case "right":
				return STATE_JUMP_RIGHT;
			case "left":
				return STATE_JUMP_LEFT;
			default:
				return STATE_JUMP;
		}
	}
 
    //--------------------------------------
    // Change the players animation state
    //--------------------------------------
    void changeState(int state){
        if (_currentAnimationState == state)
			return;
 
		animator.SetInteger("state", state);
        _currentAnimationState = state;
    }
 
    //--------------------------------------
    // 
    //--------------------------------------
     void OnCollisionEnter2D(Collision2D coll)
     {
		 
 
     }
 
     //--------------------------------------
     // Flip player sprite for left/right walking
	 // We will not be using this. Makes the shadows incorrect.
     //--------------------------------------
     void changeDirection(string direction)
     {
		 
     }
 
	void updateIntent(string intent) {
		this.intent = intent;
	}
	
	public void damage(float amount) {
		hp -= amount;
		if(hp <= 0) {
			die();
			return;
		}
		
		updateHPDisplay();
	}
	
	public void die() {
		hp = 0;
		updateHPDisplay();
		Debug.Log("You have died.");
	}
	
	void updateHPDisplay() {
		hpDisplay.display(hp);
	}
}