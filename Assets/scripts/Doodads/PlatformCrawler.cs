using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlatformCrawler : MonoBehaviour
{
	public GameObject surface;
	public float size = 0.16f;
	public float walkSpeed = 0.16f;
	public float walkDuration = 1f;
	public float minimumIdleDuration = 1f;
	public float idleDurationVariance = 3f;
	
	Random random;
	float left;
	float top;
	float right;
	float bottom;
	float walkDistance;
	Animator animator;
	Rigidbody2D rigidBody;
	Vector2 movement = Vector2.zero;
	
    // Start is called before the first frame update
    void Start() {
		random = new Random();
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		walkDistance = walkSpeed * walkDuration;
		establishBounds();
		idle();
    }
	
	void idle() {
		movement = Vector2.zero;
		animator.SetBool("walking", false);
		Invoke("chooseWalk", idleDuration());
	}
	
	float idleDuration() {
		// TODO: Randomize
		float variance = (float) random.NextDouble();
		return minimumIdleDuration + idleDurationVariance * variance;
	}
	
	void establishBounds() {
		BoxCollider2D bc2d = surface.GetComponent<BoxCollider2D>();
		if(bc2d == null) {
			Debug.Log("ERROR: PlatformCrawler.establishBounds not yet programmed for surface without BoxCollider2D");
			return;
		}
		
		left = bc2d.bounds.min.x + size / 2;
		top = bc2d.bounds.max.y - size / 2;
		right = bc2d.bounds.max.x - size / 2;
		bottom = bc2d.bounds.min.y + size / 2;
	}
	
	void chooseWalk() {
		animator.SetBool("walking", true);
		List<string> possibleDirections = new List<string>();
		if(transform.position.y + walkDistance < top)
			possibleDirections.Add("up");
		if(transform.position.y - walkDistance > bottom)
			possibleDirections.Add("down");
		if(transform.position.x + walkDistance < right)
			possibleDirections.Add("right");
		if(transform.position.x - walkDistance > left)
			possibleDirections.Add("left");
		if(transform.position.x + walkDistance * 0.71f < right && transform.position.y + walkDistance * 0.71f < top)
			possibleDirections.Add("upRight");
		if(transform.position.x + walkDistance * 0.71f < right && transform.position.y - walkDistance * 0.71f > bottom)
			possibleDirections.Add("downRight");
		if(transform.position.x - walkDistance * 0.71f > left && transform.position.y + walkDistance * 0.71f < top)
			possibleDirections.Add("upLeft");
		if(transform.position.x - walkDistance * 0.71f > left && transform.position.y - walkDistance * 0.71f > bottom)
			possibleDirections.Add("downLeft");
		
		int index = random.Next(possibleDirections.Count);
		setDirection(possibleDirections[index]);
		Invoke("idle", walkDuration);
	}
	
	void setDirection(string direction) {
		switch(direction) {
			case "up":
				SetRotated(false);
				movement = new Vector2(0, walkSpeed);
				transform.localRotation = Quaternion.Euler(0, 0, 0);
				break;
			case "upRight":
				SetRotated(true);
				movement = new Vector2(walkSpeed * 0.71f, walkSpeed * 0.71f);
				transform.localRotation = Quaternion.Euler(0, 0, 0);
				break;
			case "right":
				SetRotated(false);
				movement = new Vector2(walkSpeed, 0);
				transform.localRotation = Quaternion.Euler(0, 0, 270);
				break;
			case "downRight":
				SetRotated(true);
				movement = new Vector2(walkSpeed * 0.71f, -walkSpeed * 0.71f);
				transform.localRotation = Quaternion.Euler(0, 0, 270);
				break;
			case "down":
				SetRotated(false);
				movement = new Vector2(0, -walkSpeed);
				transform.localRotation = Quaternion.Euler(0, 0, 180);
				break;
			case "downLeft":
				SetRotated(true);
				movement = new Vector2(-walkSpeed * 0.71f, -walkSpeed * 0.71f);
				transform.localRotation = Quaternion.Euler(0, 0, 180);
				break;
			case "left":
				SetRotated(false);
				movement = new Vector2(-walkSpeed, 0);
				transform.localRotation = Quaternion.Euler(0, 0, 90);
				break;
			case "upLeft":
				SetRotated(true);
				movement = new Vector2(-walkSpeed * 0.71f, walkSpeed * 0.71f);
				transform.localRotation = Quaternion.Euler(0, 0, 90);
				break;
				
		}
	}
	
	void SetRotated(bool rotated) {
		animator.SetBool("rotated", rotated);
	}

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = movement;
    }
}
