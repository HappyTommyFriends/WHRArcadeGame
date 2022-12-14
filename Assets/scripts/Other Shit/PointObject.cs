using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
	public GameManager gameManager;
	public int pointValue;
	public AudioClip sound;
	public float removalDelay = 0.9f;
	
	bool retrieved = false;
	
	private void OnTriggerEnter2D(Collider2D other)
    {
		if(retrieved)
			return;
		
		if(other.name == gameManager.player.name)
			retrieve();
    }
    // Start is called before the first frame update
    void retrieve() {
		retrieved = true;
		GetComponent<AudioSource>().PlayOneShot(sound);
		gameManager.addScore(pointValue);
		GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.25f);
		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.75f);
		Invoke("HalfFade", removalDelay / 3);
		Invoke("ThreeQuartersFade", removalDelay * 2 / 3);
		Invoke("Remove", removalDelay);
	}
	
	void HalfFade() {
		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.5f);
	}
	
	void ThreeQuartersFade() {
		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.25f);
	}
	
	void Remove() {
		Destroy(gameObject);
	}
}