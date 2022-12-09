using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EdgeDetectingTileBuilder : MonoBehaviour
{
	static Dictionary<string, Dictionary<int, Dictionary<int, GameObject>>> tileBuilders = new Dictionary<string, Dictionary<int, Dictionary<int, GameObject>>>();

	public string builderType;
	public GameObject topLeft;
	public GameObject topEdge;
	public GameObject topRight;
	public GameObject rightEdge;
	public GameObject bottomRight;
	public GameObject bottomEdge;
	public GameObject bottomLeft;
	public GameObject leftEdge;
	public GameObject center;
	public GameObject edge5;
	public GameObject edge7;
	public GameObject edge10;
	public GameObject edge11;
	public GameObject edge13;
	public GameObject edge14;
	public GameObject solitaire;

	public bool snapYPosition = true;
	public bool snapXPosition = true;

	public float tileWidth = 0.16f;
	public float tileHeight = 0.16f;

	protected int x;
	protected int y;

	public static void reset() {
		tileBuilders = new Dictionary<string, Dictionary<int, Dictionary<int, GameObject>>>();
	}

	static void register(EdgeDetectingTileBuilder builder) {
		string type = builder.builderType;
		ensureBuilderType(type);
		if(!tileBuilders[type].ContainsKey(builder.x))
			tileBuilders[type].Add(builder.x, new Dictionary<int, GameObject>());
		if(tileBuilders[type][builder.x].ContainsKey(builder.y)) {
			Destroy(builder.gameObject);
		} else {
			tileBuilders[type][builder.x].Add(builder.y, builder.gameObject);
		}

		// logTileBuilders();
	}

	static bool tileAt(string builderType, int x, int y) {
		// Debug.Log("tileAt(" + builderType + ", " + x + ", " + y + ")");
		// Debug.Log(tileBuilders[builderType].ContainsKey(x));
		if(!tileBuilders[builderType].ContainsKey(x))
			return false;

		// Debug.Log(tileBuilders[builderType][x].ContainsKey(y));
		return tileBuilders[builderType][x].ContainsKey(y);
	}

	static EdgeDetectingTileBuilder getTileAt(string builderType, int x, int y) {
		return tileBuilders[builderType][x][y].GetComponent<EdgeDetectingTileBuilder>();
	}

	static void logTileBuilders() {
		foreach(KeyValuePair<string, Dictionary<int, Dictionary<int, GameObject>>> typeRow in tileBuilders) {
			foreach(KeyValuePair<int, Dictionary<int, GameObject>> indexRow in typeRow.Value) {
				foreach(KeyValuePair<int, GameObject> columnObject in indexRow.Value) {
					Debug.Log(typeRow.Key + "::" + indexRow.Key + "," + columnObject.Key);
				}
			}
		}
	}

	static void ensureBuilderType(string type) {
		if(tileBuilders.ContainsKey(type))
			return;

		tileBuilders.Add(type, new Dictionary<int, Dictionary<int, GameObject>>());
	}

    // Start is called before the first frame update
    void Start()
    {
		// Debug.Log("My builderType is " + builderType);
		// Debug.Log(transform.position);
		if(snapYPosition)
			transform.position = new Vector3(transform.position.x, (float) (Math.Round(transform.position.y / tileWidth) * tileWidth), transform.position.z);
		if(snapXPosition)
			transform.position = new Vector3((float) (Math.Round(transform.position.x / tileWidth) * tileWidth), transform.position.y, transform.position.z);
		// Debug.Log(transform.position);
        x = (int) Math.Round(transform.position.x / tileWidth);
		y = (int) Math.Round(transform.position.y / tileHeight);
		// Debug.Log(x + ", " + y);
		register(this);
		Invoke("configure", 0.1f);
    }

	void configure() {
		clearChildren();
		int adjacentConfiguration = 0;
		if(tileOnTop())
			adjacentConfiguration++;
		if(tileOnRight())
			adjacentConfiguration += 2;
		if(tileOnBottom())
			adjacentConfiguration += 4;
		if(tileOnLeft())
			adjacentConfiguration += 8;
		// Debug.Log("adjacentConfiguration: " + adjacentConfiguration);
		GameObject prefab = null;
		switch(adjacentConfiguration) {
			case 0:
				prefab = solitaire;
				break;
			case 1:
				prefab = edge14;
				break;
			case 2:
				prefab = edge13;
				break;
			case 3:
				prefab = bottomLeft;
				break;
			case 4:
				prefab = edge11;
				break;
			case 5:
				prefab = edge10;
				break;
			case 6:
				prefab = topLeft;
				break;
			case 7:
				prefab = leftEdge;
				break;
			case 8:
				prefab = edge7;
				break;
			case 9:
				prefab = bottomRight;
				break;
			case 10:
				prefab = edge5;
				break;
			case 11:
				prefab = bottomEdge;
				break;
			case 12:
				prefab = topRight;
				break;
			case 13:
				prefab = rightEdge;
				break;
			case 14:
				prefab = topEdge;
				break;
			case 15:
				prefab = center;
				break;
		}

		GameObject tile = Instantiate(prefab, Vector3.zero, Quaternion.identity);
		tile.transform.parent = transform;
		tile.transform.localPosition = Vector3.zero;

		if(tileOnRight() && !tileOnLeft())
			unifyHorizontalBoundingBox();
	}

	bool tileOnTop() {
		return tileAt(builderType, x, y + 1);
	}

	bool tileOnRight() {
		return tileAt(builderType, x + 1, y);
	}

	bool tileOnBottom() {
		return tileAt(builderType, x, y - 1);
	}

	bool tileOnLeft() {
		return tileAt(builderType, x - 1, y);
	}

	void clearChildren() {
		foreach(Transform child in this.transform)
		{
			Destroy(child.gameObject);
		}
	}

    // Update is called once per frame
    void Update()
    {

    }

	protected void unifyHorizontalBoundingBox() {
		int count = 2;
		EdgeDetectingTileBuilder tile = getTileAt(builderType, x + 1, y);
		Destroy(tile.gameObject.GetComponent<BoxCollider2D>());
		while(tile.tileOnRight()) {
			tile = getTileAt(builderType, x + count, y);
			Destroy(tile.gameObject.GetComponent<BoxCollider2D>());
			count++;
		}

		float newWidth = tileWidth * count;
		BoxCollider2D bc2d = GetComponent<BoxCollider2D>();
		float newX = bc2d.offset.x + (tileWidth * (count - 1) / 2);
		bc2d.size = new Vector2(newWidth, bc2d.size.y);
		bc2d.offset = new Vector2(newX, bc2d.offset.y);
		// Debug.Log(name + " count: " + count);
	}
}
