using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmartPlatform : MonoBehaviour
{
	public GameObject soloTile;
	public GameObject tile177;	
	public GameObject topLeftTile;
	public GameObject topTile;
	public GameObject topRightTile;
	public GameObject leftTile;
	public GameObject centerTile;
	public GameObject rightTile;
	
	public bool snapYPosition = true;
	public float tileWidth = 0.16f;
	public float tileHeight = 0.16f;
	public bool noCorners = false;
	public bool diggable = false;
	public bool reconstructive = false;
	public bool noTop = false;
	public bool rightSplit = false;
	public bool leftSplit = false;
	public float digAnimationDuration = 0.5f;
	
	public GameObject soloDig;
	public GameObject bothEdgeDig;
	public GameObject topLeftDig;
	public GameObject topDig;
	public GameObject topRightDig;
	public GameObject leftDig;
	public GameObject centerDig;
	public GameObject rightDig;
	
	public GameObject smartPlatformPrefab;
	
	int width;
	int height;
	List<List<GameObject>> tiles;
	
    // Start is called before the first frame update
    void Start()
    {
        width = (int) Math.Round(transform.localScale.x);
		height = (int) Math.Round(transform.localScale.y);
		
		transform.localScale = new Vector3(1f, 1f, 1f);
		
		
		tiles = new List<List<GameObject>>();
		rebuild();
/* 		foreach(List<GameObject> row in tiles) {
			foreach(GameObject tile in row) {
				Debug.Log(tile);
			}
		} */
    }

    // Update is called once per frame
    void Update()
    {
/*         Debug.DrawLine(topLeft() + new Vector3(-0.08f, 0.08f, 0), topLeft() + new Vector3(0.08f, -0.08f));
        Debug.DrawLine(topLeft() + new Vector3(-0.08f, -0.08f, 0), topLeft() + new Vector3(0.08f, 0.08f)); */
    }
	
	Vector3 topLeft() {
		return new Vector3(transform.position.x - (width * tileWidth / 2), transform.position.y + (height * tileHeight / 2), 0);
	}
	
	Vector3 topRight() {
		return new Vector3(transform.position.x + (width * tileWidth / 2), transform.position.y + (height * tileHeight / 2), 0);
	}
	
	void rebuild() {
		// Debug.Log("Rebuilding");
		// Debug.Log(this);
		// Debug.Log(width + "x" + height);
		
		
		if(rightSplit) {
			soloTile = topRightTile;
			tile177 = rightTile;
			topLeftTile = topTile;
			leftTile = centerTile;
		}
		if(leftSplit) {
			soloTile = topLeftTile;
			tile177 = leftTile;
			rightTile = centerTile;
			topRightTile = topTile;
		}
		if(noCorners) {
			soloTile = tile177;
			topLeftTile = leftTile;
			topRightTile = rightTile;
		}
		if(noTop) {
			soloTile = tile177;
			topLeftTile = leftTile;
			topRightTile = rightTile;
			topTile = centerTile;
		}
		
		if(GetComponent<SpriteRenderer>() != null)
			Destroy(GetComponent<SpriteRenderer>());
		clearChildren();
		tiles = new List<List<GameObject>>();
		if(snapYPosition)
			transform.position = new Vector3(transform.position.x, (float) Math.Round(transform.position.y * 2f / tileWidth) * tileWidth / 2, transform.position.z);
		if(width == 0) {
			Debug.Log("WARNING: Platform with width 0");
			return;
		}
		tiles.Add(new List<GameObject>());
		float yPosition = (height - 1) * 0.5f * tileHeight;
		if(width == 1) {
			GameObject tile = Instantiate(soloTile, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(0, yPosition, 0);
			tiles[0].Add(tile);
			for(int i = 2; i <= height; i++) {
				tiles.Add(new List<GameObject>());
				yPosition -= tileHeight;
				tile = Instantiate(tile177, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(0, yPosition, 0);
				tiles[i - 1].Add(tile);
			}
		}
		if(width == 2) {
			float xPosition = -tileWidth * 0.5f;
			GameObject tile = Instantiate(topLeftTile, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
			tiles[0].Add(tile);
			tile = Instantiate(topRightTile, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition + tileWidth, yPosition, 0);
			tiles[0].Add(tile);
			for(int i = 2; i <= height; i++) {
				tiles.Add(new List<GameObject>());
				yPosition -= tileHeight;
				tile = Instantiate(leftTile, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
				tiles[i - 1].Add(tile);
				tile = Instantiate(rightTile, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth, yPosition, 0);
				tiles[i - 1].Add(tile);
			}
		}
		if(width > 2) {
			float xPosition = (width - 1) * tileWidth * -0.5f;
			GameObject tile = Instantiate(topLeftTile, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
			tiles[0].Add(tile);
			for(int x = 2; x < width; x++) {
				tile = Instantiate(topTile, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth * (x - 1), yPosition, 0);
				tiles[0].Add(tile);
			}
			tile = Instantiate(topRightTile, Vector3.zero, Quaternion.identity);
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(xPosition + tileWidth * (width - 1), yPosition, 0);
			tiles[0].Add(tile);
			for(int y = 2; y <= height; y++) {
				tiles.Add(new List<GameObject>());
				yPosition -= tileHeight;
				tile = Instantiate(leftTile, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition, yPosition, 0);
				tiles[y - 1].Add(tile);
				for(int x = 2; x < width; x++) {
					tile = Instantiate(centerTile, Vector3.zero, Quaternion.identity);
					tile.transform.parent = transform;
					tile.transform.localPosition = new Vector3(xPosition + tileWidth * (x - 1), yPosition, 0);
					tiles[y - 1].Add(tile);
				}
				tile = Instantiate(rightTile, Vector3.zero, Quaternion.identity);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3(xPosition + tileWidth * (width - 1), yPosition, 0);
				tiles[y - 1].Add(tile);
			}
		}
		// GetComponent<BoxCollider2D>().offset = new Vector2((width - 1) * tileWidth * -0.5f, 0);
		GetComponent<BoxCollider2D>().size = new Vector2(width * tileWidth, height * tileHeight);
	}
	
	void clearChildren() {
		if(tiles == null)
			return;
		
		foreach(List<GameObject> row in tiles) {
			foreach(GameObject tile in row) {
				Destroy(tile);
			}
		}
		foreach(Transform child in this.transform)
		{
			Destroy(child.gameObject);
		}
	}
	
	public void attemptDig(Vector3 point) {
		// Debug.Log("Attempting dig");
		// Debug.Log(point);
/* 		foreach (Transform child in transform) {
			Debug.Log(child.gameObject);
		} */
		int x = (int) Math.Floor((point.x - topLeft().x) / tileWidth);
		int y = (int) Math.Floor((topLeft().y - point.y) / tileHeight);
		if(y >= tiles.Count)
			y = tiles.Count - 1;
		if(y < 0)
			y = 0;
		if(x >= tiles[y].Count)
			x = tiles[y].Count - 1;
		if(x < 0)
			x = 0;
		// Debug.Log(x + "," + y);
		GameObject digT = digTile(x, y);
		digT.transform.position = tiles[y][x].transform.position;
		this.Invoke(() => DestroyObject(digT), digAnimationDuration);
		Destroy(tiles[y][x]);
		dig(x, y);
	}
	
	private void DestroyObject(GameObject go) {
		Destroy(go);
	}
	
	public GameObject digTile(int x, int y) {
		return Instantiate(topDig, transform.position + new Vector3(0, (height - y) * tileHeight / 2, 0), Quaternion.identity);
	}
	
	public void dig(int x, int y) {
		if(y == 0) {
			digTop(x);
			return;
		}
		if(y == height - 1) {
			digBottom(x);
			return;
		}
		digMiddle(x, y);
	}
	
	private void digMiddle(int x, int y) {
		// Debug.Log("digMiddle");
		if(reconstructive) {
			Debug.Log("Reconstructive platforms not yet programmed");
			return;
		}
		GameObject platformGameobject;
		SmartPlatform platform;
		clearChildren();
		platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(0, (height - y) * tileHeight / 2, 0), Quaternion.identity);
		platformGameobject.name = "Platform";
		platform = platformGameobject.GetComponent<SmartPlatform>();
		platform.transform.localScale = new Vector3(width, (float) y, 1f);
		
		transform.position = new Vector3(transform.position.x, transform.position.y - (y + 1) * tileHeight / 2, transform.position.z);
		height -= (y + 1);
		rebuild();
		
		return;
		if(width > 1) {
			if(x == 0) {
				clearChildren();
				platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(tileWidth / 2, (height - y) * tileHeight / 2, 0), Quaternion.identity);
				platformGameobject.name = "Platform";
				platform = platformGameobject.GetComponent<SmartPlatform>();
				platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
				platform.rightSplit = true;
				platform.noTop = true;
			}
			if(x == width - 1) {
				clearChildren();
				platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(-tileWidth / 2, 0, 0), Quaternion.identity);
				platformGameobject.name = "Platform";
				platform = platformGameobject.GetComponent<SmartPlatform>();
				platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
				platform.leftSplit = true;
				platform.noTop = true;
			}
		}
	}
	
	private void digBottom(int x) {
		// Debug.Log("digBottom");
		// Debug.Log(x);
		if(reconstructive) {
			Debug.Log("Reconstructive platforms not yet programmed");
			return;
		}
		GameObject platformGameobject;
		SmartPlatform platform;
		if(x == 0) {
			if(width == 1) {
				if(height == 1) {
					Destroy(gameObject);
					return;
				}
				transform.position = new Vector3(transform.position.x, transform.position.y + tileHeight / 2, transform.position.z);
				height--;
				rebuild();
			}
			if(width > 1) {
				clearChildren();
				platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(tileWidth / 2, (1 - height) * tileHeight / 2, 0), Quaternion.identity);
				platformGameobject.name = "Platform";
				platform = platformGameobject.GetComponent<SmartPlatform>();
				platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
				platform.rightSplit = true;
				
				if(height == 1) {
					Destroy(gameObject);
					return;
				}
				
				platform.noTop = true;
				
				transform.position = new Vector3(transform.position.x, transform.position.y + tileHeight / 2, transform.position.z);
				height--;
				rebuild();
				return;
			}
		}
		if(x == width - 1) {
			clearChildren();
			platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(-tileWidth / 2, (1 - height) * tileHeight / 2, 0), Quaternion.identity);
			platformGameobject.name = "Platform";
			platform = platformGameobject.GetComponent<SmartPlatform>();
			platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
			platform.leftSplit = true;
			
			if(height == 1) {
				Destroy(gameObject);
				return;
			}
			
			platform.noTop = true;
			
			transform.position = new Vector3(transform.position.x, transform.position.y + tileHeight / 2, transform.position.z);
			height--;
			rebuild();
			return;
		}
		clearChildren();
		platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(topLeft().x + x * tileWidth / 2 - transform.position.x, (height - 1) * tileHeight / 2, 0), Quaternion.identity);
		platformGameobject.name = "Platform";
		platform = platformGameobject.GetComponent<SmartPlatform>();
		platform.transform.localScale = new Vector3((float) x, 1f, 1f);
		platform.leftSplit = true;
		
		float rightWidth = (float) (width - x - 1);
		platformGameobject = Instantiate(smartPlatformPrefab, topRight() + new Vector3(-rightWidth * tileWidth / 2, -tileHeight / 2, 0), Quaternion.identity);
		platformGameobject.name = "Platform";
		platform = platformGameobject.GetComponent<SmartPlatform>();
		platform.transform.localScale = new Vector3(rightWidth, 1f, 1f);
		platform.rightSplit = true;
				
		if(height == 1) {
			Destroy(gameObject);
			return;
		}
		
		platform.noTop = true;
		
		transform.position = new Vector3(transform.position.x, transform.position.y + tileHeight / 2, transform.position.z);
		height--;
		rebuild();
	}
	
	private void digTop(int x) {
		if(reconstructive) {
			Debug.Log("Reconstructive platforms not yet programmed");
			return;
		}
		GameObject platformGameobject;
		SmartPlatform platform;
		if(x == 0) {
			if(width == 1) {
				if(height == 1) {
					Destroy(gameObject);
					return;
				}
				transform.position = new Vector3(transform.position.x, transform.position.y - tileHeight / 2, transform.position.z);
				height--;
				noTop = true;
				rebuild();
			}
			if(width > 1) {
				clearChildren();
				platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(tileWidth / 2, (height - 1) * tileHeight / 2, 0), Quaternion.identity);
				platformGameobject.name = "Platform";
				platform = platformGameobject.GetComponent<SmartPlatform>();
				platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
				platform.rightSplit = true;
				
				if(height == 1) {
					Destroy(gameObject);
					return;
				}
				
				transform.position = new Vector3(transform.position.x, transform.position.y - tileHeight / 2, transform.position.z);
				height--;
				noTop = true;
				rebuild();
				return;
			}
		}
		if(x == width - 1) {
			clearChildren();
			platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(-tileWidth / 2, (height - 1) * tileHeight / 2, 0), Quaternion.identity);
			platformGameobject.name = "Platform";
			platform = platformGameobject.GetComponent<SmartPlatform>();
			platform.transform.localScale = new Vector3((float) (width - 1), 1f, 1f);
			platform.leftSplit = true;
			
			if(height == 1) {
				Destroy(gameObject);
				return;
			}
			
			transform.position = new Vector3(transform.position.x, transform.position.y - tileHeight / 2, transform.position.z);
			height--;
			noTop = true;
			rebuild();
			return;
		}
		// Top (not a corner)
		clearChildren();
		platformGameobject = Instantiate(smartPlatformPrefab, transform.position + new Vector3(topLeft().x + x * tileWidth / 2 - transform.position.x, (height - 1) * tileHeight / 2, 0), Quaternion.identity);
		platformGameobject.name = "Platform";
		platform = platformGameobject.GetComponent<SmartPlatform>();
		platform.transform.localScale = new Vector3((float) x, 1f, 1f);
		platform.leftSplit = true;
		
		float rightWidth = (float) (width - x - 1);
		platformGameobject = Instantiate(smartPlatformPrefab, topRight() + new Vector3(-rightWidth * tileWidth / 2, -tileHeight / 2, transform.position.z), Quaternion.identity);
		platformGameobject.name = "Platform";
		platform = platformGameobject.GetComponent<SmartPlatform>();
		platform.transform.localScale = new Vector3(rightWidth, 1f, 1f);
		platform.rightSplit = true;
		if(height == 1) {
			Destroy(gameObject);
			return;
		}
		
		transform.position = new Vector3(transform.position.x, transform.position.y - tileHeight / 2, transform.position.z);
		height--;
		noTop = true;
		rebuild();
	}
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }
 
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}