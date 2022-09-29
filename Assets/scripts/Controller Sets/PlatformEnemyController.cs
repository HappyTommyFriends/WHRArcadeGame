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
	public AudioClip activationSound;
	public AudioClip attackSound;
	public AudioClip deathSound;
	public bool flipOnDeath = true;
	public float deathExitDelay = 0f;
	
	protected Animator animator;
	
	Vector3 edgeFindOffsetVector;
	bool _grounded = false;
	Vector3 lastPosition;
	bool dead = false;
	bool idling = false;
	
	Rigidbody2D rigidBody;
	SpriteRenderer spriteRenderer;
	
    const int ATTACK_STATE = 2;
	const int DAMAGE_STATE = 3;
	
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
		if(!dead && !idling && stuck())
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
		StopMotion();
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
		hop();

		animator.SetInteger("state", ATTACK_STATE);
		if(player != null)
			player.interactionController.interact(Interaction.basicAttack(gameObject, attackPower));
		AudioSource source = GetComponent<AudioSource>();
		if(source == null)
			return;
		
		source.PlayOneShot(attackSound);
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
		animator.SetInteger("state", DAMAGE_STATE);
		hp -= amount;
		if(hp <= 0)
			die();
	 }
	 
	 void die() {
		if(dead)
			return;

		hp = 0;
		dead = true;
		animator.SetBool("dead", true);
		StopMotion();
		if(gameManager != null)
			gameManager.addScore(points);
		
		Destroy(GetComponent<BoxCollider2D>());
		rigidBody.gravityScale = 0;
		if(rigidBody.velocity.y > 0)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
		Invoke("DeathExit", deathExitDelay);
		AudioSource source = GetComponent<AudioSource>();
		if(source == null)
			return;
		
		source.PlayOneShot(deathSound);
	 }
	 
	 void StopMotion() {
		directionMultiplier = 0;
		rigidBody.velocity = new Vector2(0, 0);
	 }
	 
	 void DeathExit() {
		if(flipOnDeath)
			spriteRenderer.flipY = true;
		
		rigidBody.gravityScale = 1;
	 }
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(!dead && collision.gameObject.name == "Player") {
			attackPlayer();
			return;
		}

		if(!_grounded && landing()) {
			land();
			return;
		}
	}
	
	void activate() {
		if(activated)
			return;
		
		activated = true;
		AudioSource source = GetComponent<AudioSource>();
		if(source == null)
			return;
		
		source.PlayOneShot(activationSound);
	}
	
	void OnTriggerEnter(Collider collider) {
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.name == "Activator")
			activate();
	}
	
	void OnTrigger(Collider collider) {
	}
}
