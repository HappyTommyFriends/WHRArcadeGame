using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scorpion : MonoBehaviour
{
	public GameManager gameManager;
	public float walkSpeed = 1f;
	public float chargeSpeed = 1.3f;
	public float attackRange = 0.16f;	
	public float hp = 8;
	public WHRPlayerController player;
	
	public AudioClip clawAttack;
	public AudioClip tailAttack;
	public AudioClip jab;
	public AudioClip charge;
	public AudioClip shuffleNoise;
	
	protected Animator animator;
	protected Rigidbody2D rigidBody;
	
	float horizontalSpeed;
	int state;
	bool smoothMoving = true;
	bool freeStyle = false;
	float shuffleTime = 0.08f;
	float shuffleSize = 0.12f;
	float tailStrikeWait = 1.6f;
	float width = 1.6f;
	bool nextLeft = true;
	bool motionFrozen = false;
	bool dead = false;
	bool active = false;
	
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
		animator.SetInteger("state", 0);
		rigidBody = GetComponent<Rigidbody2D>();
		// Invoke("die", 1.2f);
		// Invoke("InvokePattern", 4f);
    }
	
	public void activate() {
		active = true;
		InvokePattern();
	}
	
	void free() {
		freeStyle = true;
		smoothMoving = true;
		setState(0);
	}
	
	void InvokePattern() {
		free();
		Invoke("InvokePattern2", 8f);
	}
	
	void InvokePattern2() {
		if(playerOnTop()) {
			tailStrike();
			Invoke("InvokePattern2", tailStrikeTime() + 0.1f);
		} else {
			attack2();
			float tally = 2f;
			Invoke("attack2", tally);
			tally += 4f;
			Invoke("backup", tally);
			tally += 2f;
			Invoke("shuffle", tally);
			tally += shuffleTime * 6;
			Invoke("chargeAttack", tally);
			tally += chargeAttackTime();
			Invoke("InvokePattern", tally);
			// Invoke("attack1", 12f);
			// Invoke("InvokePattern2", 18f);
		}
	}
	
	void attack1() {
		freeStyle = false;
		Debug.Log("attack1");
		if(nextLeft) {
			attack1Left();
		} else {
			attack1Right();
		}
		nextLeft = !nextLeft;
	}
	
	void attack1Left() {
		leftJab();
		Invoke("idle", 0.5f);
		Invoke("leftSnap", 1f);
		Invoke("idle", 1.6f);
	}
	
	void attack1Right() {
		rightJab();
		Invoke("idle", 0.5f);
		Invoke("rightSnap", 1f);
		Invoke("idle", 1.6f);
	}
	
	float attack1Time() {
		return 1.65f;
	}
	
	void leftJab() {
		playSound(jab);
		setState(24);
	}
	
	void leftSnap() {
		playSound(clawAttack);
		setState(34);
		attackFacing(2);
	}
	
	void rightJab() {
		playSound(jab);
		setState(22);
	}
	
	void rightSnap() {
		playSound(clawAttack);
		setState(32);
		attackFacing(2);
	}
	
	void chargeAttack() {
		Debug.Log("chargeAttack");
		freeStyle = false;
		smoothMoving = true;
		horizontalSpeed = -chargeSpeed;
		setState(50);
	}
	
	float chargeAttackTime() {
		return 1.9f;
	}
	
	void backup() {
		Debug.Log("backup");
		setState(0);
		freeStyle = false;
		smoothMoving = true;
		horizontalSpeed = walkSpeed;
	}
	
	void idle() {
		freeStyle = false;
		setState(0);
		horizontalSpeed = 0;
		smoothMoving = true;
	}
	
	void attack2() {
		freeStyle = false;
		Debug.Log("attack2");
		setState(14);
		smoothMoving = false;
		rigidBody.AddForce(new Vector2(-5000f, 7500f));
		Invoke("attack2b", 0.5f);
	}
	
	void attack2b() {
		setState(24);
		Invoke("idle", 0.5f);
		Invoke("attack2b2", 1.0f);
	}
	
	void attack2b2() {
		leftSnap();
		Invoke("idle", 0.5f);
	}
	
	void tailStrike() {
		freeStyle = false;
		Debug.Log("tailStrike");
		Invoke("shuffle", shuffleTime);
		Invoke("actualTailStrike", shuffleTime * 4 + tailStrikeWait);
	}
	
	float tailStrikeTime() {
		return shuffleTime * 4 + tailStrikeWait + 0.5f;
	}
	
	void actualTailStrike() {
		playSound(tailAttack);
		setState(33);
		if(playerOnTop()) {
			player.jumpLeft();
			player.faceRight();
			hitPlayer(4);
		}
	}
	
	void attackFacing(float damage) {
		if(playerInAttackZone())
			player.damage(damage);
	}
	
	bool playerInAttackZone() {
		if(player.transform.position.y > -4.57)
			return false;
		
		if(player.transform.position.x < transform.position.x)
			return (player.transform.position.x > transform.position.x - width / 2 - attackRange);
		
		return (player.transform.position.x < transform.position.x + width / 2 + attackRange);
	}
	
	void hitPlayer(float amount) {
		player.damage(amount);
	}
	
	bool playerOnTop() {
		return (player.transform.position.y > transform.position.y && Math.Abs(player.transform.position.x - transform.position.x) < 0.5f);
	}
	
	void shuffle() {
		playSound(shuffleNoise);
		setState(0);
		freeStyle = false;
		smoothMoving = false;
		transform.position = new Vector3(transform.position.x + shuffleSize, transform.position.y, transform.position.z);
		Invoke("shuffle2", shuffleTime);
	}
	
	void shuffle2() {
		transform.position = new Vector3(transform.position.x - shuffleSize, transform.position.y, transform.position.z);
		Invoke("shuffle3", shuffleTime);
	}
	
	void shuffle3() {
		transform.position = new Vector3(transform.position.x + shuffleSize, transform.position.y, transform.position.z);
		Invoke("shuffle4", shuffleTime);
	}
	
	void shuffle4() {
		transform.position = new Vector3(transform.position.x - shuffleSize, transform.position.y, transform.position.z);
	}
	
	void setState(int state) {
		animator.SetInteger("state", state);
		this.state = state;
	}
	
	void moveLeft() {
		faceLeft();
		horizontalSpeed = -walkSpeed;
		setState(0);
	}
	
	void moveRight() {
		faceRight();
		horizontalSpeed = walkSpeed;
		setState(0);
	}
	
	void faceLeft() {
		// GetComponent<SpriteRenderer>().flipX = false;
		transform.localRotation = Quaternion.Euler(0, 0, 0);
	}
	
	void faceRight() {
		// GetComponent<SpriteRenderer>().flipX = true;
		transform.localRotation = Quaternion.Euler(0, 180, 0);
	}
	
	void freezeMovement() {
		motionFrozen = true;
	}
	
	void freezeMovementFor(float duration) {
		motionFrozen = true;
		Invoke("unfreezeMovement", duration);
	}
	
	void unfreezeMovement() {
		motionFrozen = false;
	}
	
	void frozenAttack1() {
		Invoke("freezeMovement", 0.5f);
		attack1();
		Invoke("unfreezeMovement", attack1Time() + 0.1f);
	}

    // Update is called once per frame
    void Update()
    {
		if(!active || dead)
			return;
		
		if(freeStyle) {
			if(playerOnTop()) {
				tailStrike();
			} else {
				if(player.transform.position.x < transform.position.x) {
					if(player.transform.position.x > transform.position.x - width / 2 - attackRange) {
						frozenAttack1();
						Invoke("free", attack1Time());
					} else {
						Invoke("moveLeft", 1f);
					}
				} else {
					Invoke("moveRight", 1f);
				}
			}
		}
		if(!motionFrozen && smoothMoving)
			rigidBody.velocity = new Vector2(horizontalSpeed, 0);
    }
	
	public void damage(float amount = 1f) {
		Debug.Log("Scorpion.damage()...");
		hp -= amount;
		if(hp <= 0)
			die();
	}
	
	void die() {
		death();
		gameManager.addScore(1000);
	}
	
	// Kill NOT as the player (no score bonus)
	public void death() {
		CancelInvoke();
		dead = true;
		active = false;
		transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
		transform.localRotation = Quaternion.Euler(180, 0, 0);
		// Destroy(GetComponent<BoxCollider2D>());
		// Destroy(GetComponent<BoxCollider2D>());
		Component[] hingeJoints;

        hingeJoints = GetComponents(typeof(BoxCollider2D));

        foreach (BoxCollider2D joint in hingeJoints)
            Destroy(joint);
			
		rigidBody.AddForce(new Vector2(0, 750f));
	}
	
	void playSound(AudioClip clip) {
		GetComponent<AudioSource>().PlayOneShot(clip);
	}
	
	public bool isDead() {
		return dead;
	}
}