using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmartPlatform : MonoBehaviour
{
	public GameObject tile54;
	public GameObject tile177;	
	public GameObject tile62;
	public GameObject tileFull;
	public GameObject tile248;
	public GameObject tile191;
	public GameObject tile255;
	public GameObject tile241;
	public GameObject tile187;
	
	public bool snapYPosition = true;
	public float tileWidth = 0.16f;
	public float tileHeight = 0.16f;
	
	int width;
	int height;
	
    // Start is called before the first frame update
    void Start()
    {
        width = (int) Math.Round(transform.localScale.x);
		height = (int) Math.Round(transform.localScale.y);
		
		transform.localScale = new Vector3(1f, 1f, 1f);
		
		rebuild();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void rebuild() {
		Destroy(GetComponent<SpriteRenderer>());
		if(snapYPosition)
			transform.position = new Vector3(transform.position.x, (float) Math.Round(transform.position.y * 2f / tileWidth) * tileWidth / 2, transform.position.z);
		if(width == 0) {
			Debug.Log("WARNING: Platform with width 0");
			return;
		}
		float yPosition = (height - 1) * 0.5f * tileHeight;
		if(width == 1) {
			GameObject tile = Instantiate(tile54, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(0, yPosition, 0);
			for(int i = 2; i <= height; i++) {
				yPosition -= tileHeight;
				tile = Instantiate(tile177, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(0, yPosition, 0);
			}
		}
		if(width == 2) {
			float xPosition = -tileWidth * 0.5f;
			GameObject tile = Instantiate(tile62, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
			tile = Instantiate(tile248, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition + tileWidth, yPosition, 0);
			for(int i = 2; i <= height; i++) {
				yPosition -= tileHeight;
				tile = Instantiate(tile191, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
				tile = Instantiate(tile241, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth, yPosition, 0);
			}
		}
		if(width > 2) {
			float xPosition = (width - 1) * tileWidth * -0.5f;
			GameObject tile = Instantiate(tile62, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
			for(int x = 2; x < width; x++) {
				tile = Instantiate(tileFull, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth * (x - 1), yPosition, 0);
			}
			tile = Instantiate(tile248, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition + tileWidth * (width - 1), yPosition, 0);
			for(int y = 2; y <= height; y++) {
				yPosition -= tileHeight;
				tile = Instantiate(tile191, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
				for(int x = 2; x < width; x++) {
					tile = Instantiate(tile255, Vector3.zero, Quaternion.identity);
					tile.transform.parent = transform;
					tile.transform.localPosition = new Vector3(xPosition + tileWidth * (x - 1), yPosition, 0);
				}
				tile = Instantiate(tile241, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth * (width - 1), yPosition, 0);
			}
		}
		// GetComponent<BoxCollider2D>().offset = new Vector2((width - 1) * tileWidth * -0.5f, 0);
		GetComponent<BoxCollider2D>().size = new Vector2(width * tileWidth, height * tileHeight);
	}
}
