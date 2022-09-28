using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformEnemyController : MonoBehaviour
{
	public GameManager gameManager;
	public PlayerController player;
	public float walkSpeed = 1;
	public float hp = 1f;
	public float directionMultiplier = 1;
	public float attackPower = 1f;
	public bool oscillate = true;
	public bool homing = false;
	public bool homingJump = false;
	public float hopStrength = 100;
	public float jumpStrength = 280;
	public float edgeFindOffset = 0.16f;
	public float wallDetectionDistance = 1f;
	public bool activated = false;
	public float homingDelay = 0.6f;
	public float lastHoming = 0;
	public int points = 50;
	public bool flipSpriteOnDirection = true;
	
	protected Animator animator;
	
	Vector3 edgeFindOffsetVector;
	bool _grounded = false;
	Vector3 lastPosition;
	bool dead = false;
	bool idling = false;
	
	Rigidbody2D rigidBody;
	SpriteRenderer spriteRenderer;
	
    // Start is called before the first frame update
    void Start()
    {
		animator = gameObject.GetComponent<Animator>();
		animator.SetInteger("state", 1);
		lastPosition = transform.position;
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        setEdgeFindOffsetVector();
    }

    // Update is called once per frame
    void Update()
    {
		if(dead)
			return;
		
		if(!activated)
			return;
		
		if(homing) {
			attemptHome();
			if(hittingWall()) {
				attemptJump();
			}
		} else {
			if(oscillate && _grounded && goingOverEdge()) {
				flipDirection();
			} else {
				if(hittingWall()) {
					flipDirection();
				}
			}
		}
		
		rigidBody.velocity = new Vector2(walkSpeed * directionMultiplier, rigidBody.velocity.y);
	}
	
	void attemptHome() {
		if(lastHoming + homingDelay < Time.realtimeSinceStartup)
			home();
	}
		
	void home() {
		lastHoming = Time.realtimeSinceStartup;
		if(transform.position.x > player.transform.position.x) {
			goLeft();
		} else {
			goRight();
		}
	}
		
	void FixedUpdate() {
		if(!idling && stuck())
			tinyHopFix();
    }
	
	void setEdgeFindOffsetVector() {
		if(directionMultiplier < 0) {
			edgeFindOffsetVector = new Vector3(-edgeFindOffset, 0, 0);
		} else {
			edgeFindOffsetVector = new Vector3(edgeFindOffset, 0, 0);
		}
	}
	
	public void goLeft() {
		idling = false;
		directionMultiplier = -1;
		setEdgeFindOffsetVector();
		if(flipSpriteOnDirection)
			spriteRenderer.flipX = false;
		if(animator == null)
			return;
		
		animator.SetBool("right", false);
		animator.SetInteger("state", 1);
	}
	
	public void goRight() {
		idling = false;
		directionMultiplier = 1;
		setEdgeFindOffsetVector();
		if(flipSpriteOnDirection)
			spriteRenderer.flipX = true;
		if(animator == null)
			return;
		
		animator.SetBool("right", true);
		animator.SetInteger("state", 1);
	}
	
	public void idle() {
		idling = true;
		directionMultiplier = 0;
		if(animator == null)
			return;
		
		animator.SetInteger("state", 0);
	}
	
	bool goingOverEdge() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position + edgeFindOffsetVector, Vector2.down, 0.2f);
		
        return (hit.collider == null);
	}
	
	bool hittingWall() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position + edgeFindOffsetVector, Vector2.right * directionMultiplier, wallDetectionDistance);
		if(hit.collider != null) {
			if(hit.collider.name == "Player")
				return false;
		}
        return (hit.collider != null);
	}
	
	void flipDirection() {
		directionMultiplier *= -1;
		setEdgeFindOffsetVector();
		if(flipSpriteOnDirection)
			spriteRenderer.flipX = (directionMultiplier > 0);
		if(animator == null)
			return;
		
		animator.SetBool("right", directionMultiplier > 0);
	}
	 
	 bool stuck() {
		Vector3 displacement = transform.position - lastPosition;
		lastPosition = transform.position;
		return displacement.magnitude < 0.001;
	 }
	 
	 void attackPlayer() {
		 Debug.Log("attackPlayer");
		 hop();
		 
		 player.interactionController.interact(Interaction.basicAttack(gameObject, attackPower));
	 }
	 
	 bool landing() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
		
        return (hit.collider != null);
	 }
	 
	 void land() {
		 rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
		 _grounded = true;
	 }
	 
	 void attemptJump() {
		 if(_grounded)
			 jump();
	 }
	 
	 void jump() {
		 rigidBody.AddForce(new Vector2(0, jumpStrength));
		 _grounded = false;
	 }
	 
	 void hop() {
		 rigidBody.AddForce(new Vector2(0, hopStrength));
		 _grounded = false;
	 }
	 
	 void tinyHopFix() {
		 if(_grounded)
			 tinyHop();
	 }
	 
	 void tinyHop() {
		 rigidBody.AddForce(new Vector2(0, hopStrength / 2));
		 _grounded = false;
	 }
	 
	 public void damage(float amount) {
		 hp -= amount;
		 if(hp <= 0)
			 die();
	 }
	 
	 void die() {
		 hp = 0;
		 spriteRenderer.flipY = true;
		 Destroy(GetComponent<BoxCollider2D>());
		 dead = true;
		 gameManager.addScore(points);
	 }
	
	void OnCollisionEnter2D(Collision2D collision)
	{		
		if(collision.gameObject.name == "Player") {
			attackPlayer();
			return;
		}

		if(!_grounded && landing()) {
			land();
			return;
		}
	}
	
	void activate() {
		Debug.Log("Enemy Activating...");
		activated = true;
	}
	
	void OnTriggerEnter(Collider collider) {
		Debug.Log(collider);
		Debug.Log(collider.gameObject);
		Debug.Log(collider.gameObject.name);
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log(collider);
		Debug.Log(collider.gameObject);
		Debug.Log(collider.gameObject.name);
		if(collider.gameObject.name == "Activator")
			activate();
	}
	
	void OnTrigger(Collider collider) {
		Debug.Log(collider);
		Debug.Log(collider.gameObject);
		Debug.Log(collider.gameObject.name);
	}
}
