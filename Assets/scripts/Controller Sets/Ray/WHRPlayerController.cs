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
	public float digOffset = 0.16f;
	public float digDistance = 0.02f;
	public float digDuration = 0.3f;
	public AudioClip jumpNoise;
	public float height = 0.32f;
	
	bool _attacking = false;
	float lastJump = 0;
	Vector3 attackOrigin = new Vector3(0, 0, 0);
	bool preCheckJumps = false;
	AudioSource audioSource;

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
	const int STATE_DIG_UP = 71;
	const int STATE_DIG_RIGHT = 72;
	const int STATE_DIG_DOWN = 73;
	const int STATE_DIG_LEFT = 74;
	
	new void setStartingAnimationState() {
		Debug.Log("setStartingAnimationState");
		_currentAnimationState = STATE_IDLE;
		updateHPDisplay();
	}
	
	protected override void takeAction(string action) {
		Debug.Log("takeAction " + action);
		lastAction = action;
		
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
			case "down":
				attemptDig();
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
		
		if(movementFrozen) {
			horizontalSpeed = 0;
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
	
	Vector2 digStartVector() {
		switch(_currentDirection) {
			case "down":
				return new Vector2(0, -digOffset);
			case "left":
				return new Vector2(-digOffset, 0.08f);
			case "right":
				return new Vector2(digOffset, 0.08f);
			case "up":
				return new Vector2(0, digOffset);
		}
		
		return Vector2.zero;
	}
	
	Vector2 directionVector() {
		switch(_currentDirection) {
			case "down":
				return Vector2.down.normalized;
			case "left":
				return Vector2.left.normalized;
			case "right":
				return Vector2.right.normalized;
			case "up":
				return Vector2.up.normalized;
		}
		
		return Vector2.zero;
	}
	
	Vector2 motionDirectionVector() {
		if(horizontalSpeed < 0)
			return Vector2.left.normalized;
		
		if(horizontalSpeed > 1)
			return Vector2.right.normalized;
		
		return Vector2.zero;
	}
	
	void digFix() {
		preCheckJumps = true;
		freezeMovement();
		Vector2 adjustmentVector = directionVector() * -0.02f;
		transform.position = transform.position + new Vector3(adjustmentVector.x, adjustmentVector.y, 0);
		Invoke("unfreezeMovement", digDuration);
	}
	
	void digIfApplicable() {
		if(!canDig())
			return;
		
		if(rigidBody.velocity.y != 0)
			return;
		
		Vector2 rayStart = new Vector2(transform.position.x, transform.position.y) + digStartVector();
		// Debug.DrawLine(rayStart, rayStart + directionVector() * digDistance);
		
		RaycastHit2D hit = Physics2D.Raycast(rayStart, directionVector(), digDistance);
        if(hit.collider != null) {
			changeToDigState();
			SmartPlatform platform = hit.collider.gameObject.GetComponent<SmartPlatform>();
			if(platform != null && platform.diggable) {
				platform.attemptDig(hit.point);
				if(!movementFrozen) {
					digFix();
				}
			}
		}
		
		hit = Physics2D.Raycast(rayStart + new Vector2(0, 0.16f), directionVector(), digDistance);
        if(hit.collider == null)
			return;
		
		SmartPlatform platform2 = hit.collider.gameObject.GetComponent<SmartPlatform>();
		if(platform2 == null)
			return;
		
		if(!platform2.diggable)
			return;
		
		platform2.attemptDig(hit.point);
		if(!movementFrozen) {
			digFix();
		}
		
		if(movementFrozen) {
			secondDig();
		}
	}
	
	void secondDig() {
		if(!canDig())
			return;
		
		if(rigidBody.velocity.y != 0)
			return;
		
		Vector2 rayStart = new Vector2(transform.position.x, transform.position.y) + digStartVector();
		// Debug.DrawLine(rayStart, rayStart + directionVector() * digDistance);
		
		RaycastHit2D hit = Physics2D.Raycast(rayStart, directionVector(), digDistance);
        if(hit.collider != null) {
			SmartPlatform platform = hit.collider.gameObject.GetComponent<SmartPlatform>();
			if(platform != null && platform.diggable) {
				platform.attemptDig(hit.point);
			}
		}
		
		hit = Physics2D.Raycast(rayStart + new Vector2(0, 0.16f), directionVector(), digDistance);
        if(hit.collider == null)
			return;
		
		SmartPlatform platform2 = hit.collider.gameObject.GetComponent<SmartPlatform>();
		if(platform2 == null)
			return;
		
		if(!platform2.diggable)
			return;
		
		platform2.attemptDig(hit.point);
	}
	
	void attemptDig() {
		Debug.Log("attemptDig");
		if(!canDig())
			return;
		
		if(rigidBody.velocity.y != 0)
			return;
		
		Vector2 rayStart = new Vector2(transform.position.x, transform.position.y) + digOffsetVector();
		Debug.Log(rayStart);
		Debug.Log(rayStart + Vector2.down.normalized * digDistance);
		Debug.DrawLine(rayStart, rayStart + Vector2.down.normalized * digDistance);
		RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, digDistance);
        if(hit.collider == null)
			return;
		
		changeState(STATE_DIG_DOWN);
		Debug.Log(hit.collider);
		Debug.Log(hit.collider.name);
		if(hit.collider.gameObject.name == "Platform") {
			SmartPlatform platform = hit.collider.gameObject.GetComponent<SmartPlatform>();
			if(platform == null)
				return;
			
			if(platform.diggable)
				platform.attemptDig(hit.point);
		}
	}
	
	void FixedUpdate() {
		//Vector2 rayStart = new Vector2(transform.position.x, transform.position.y) + digOffsetVector();
		//Debug.DrawLine(rayStart, rayStart + Vector2.down.normalized * digDistance);
		
		/* Vector2 rayStart = new Vector2(transform.position.x, transform.position.y) + digStartVector();
		Debug.Log(rayStart);
		Debug.Log(directionVector() * digDistance);
		Debug.DrawLine(rayStart, rayStart + directionVector() * digDistance); */
		
		
		// Overhead jump check
		// Debug.DrawLine(jumpCheckOrigin(), jumpCheckOrigin() + jumpCheckDirection() * 0.06f);
		
		Debug.DrawLine(groundedVectorStart(), groundedVectorEnd());
		Debug.DrawLine(transform.position + attackOrigin, transform.position + attackOrigin + Vector3.right.normalized * attackRange);
		
		if(horizontalSpeed != 0)
			digIfApplicable();
	}
	
	Vector2 digOffsetVector() {
		return new Vector2(0, digOffset);
	}
	
	bool canDig() {
		if(rigidBody.velocity.y != 0)
			return false;
		
		return true;
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
		if(movementFrozen || lastJump + 0.1 > Time.realtimeSinceStartup)
			return;
		
		if(grounded())
			jump();
	}
	
	bool grounded() {
		if(rigidBody.velocity.y != 0)
			return false;
		
		// RaycastHit2D hit = Physics2D.Raycast(groundedVectorStart(), groundedVectorCast2D);
		
		RaycastHit2D hit = Physics2D.Raycast(groundedVectorStart(), Vector2.down, 0.02f);
        return (hit.collider != null);
	}
	
	Vector3 groundedVectorStart() {
		return transform.position - new Vector3(0, (height / 2) - 0.01f, 0);
	}
	
	Vector2 groundedVectorCast2D() {
		Vector3 v = groundedVectorCast();
		return new Vector2(v.x, v.y);
	}
	
	Vector3 groundedVectorCast() {
		return new Vector3(0, 0.02f, 0);
	}
	
	Vector3 groundedVectorEnd() {
		return groundedVectorStart() + Vector3.down * 0.06f;
	}
	
	void jump() {
		if(preCheckJumps && !preJumpCheck())
			return;
		
		GetComponent<AudioSource>().PlayOneShot(jumpNoise);
		rigidBody.AddForce(new Vector2(0, jumpStrength));
		changeState(jumpState());
		lastJump = Time.realtimeSinceStartup;
		
		preCheckJumps = false;
	}
	
	bool preJumpCheck() {
		RaycastHit2D hit = Physics2D.Raycast(jumpCheckOrigin(), jumpCheckDirection(), 0.06f);
		if(hit.collider != null)
			return false;
		
		return true;
	}
	
	Vector2 jumpCheckOrigin() {
		return new Vector2(transform.position.x, transform.position.y + 0.30f) + motionDirectionVector() * 0.08f;
	}
	
	Vector2 jumpCheckDirection() {
		return Vector2.up + motionDirectionVector();
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
	
	void changeToDamageState() {
		changeState(damageState());
	}
	
	int damageState() {
		switch (_currentDirection) {
			case "right":
				return STATE_DAMAGE_RIGHT;
			case "left":
				return STATE_DAMAGE_LEFT;
			default:
				return STATE_DAMAGE_RIGHT;
		}
	}
	
	void changeToDigState() {
		changeState(digState());
	}
	
	int digState() {
		switch (_currentDirection) {
			case "right":
				return STATE_DIG_RIGHT;
			case "left":
				return STATE_DIG_LEFT;
			case "down":
				return STATE_DIG_DOWN;
			default:
				return STATE_DIG_DOWN;
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
		changeToDamageState();
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