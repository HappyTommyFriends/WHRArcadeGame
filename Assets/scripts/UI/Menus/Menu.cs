using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public string[] menuItems;
	public GameObject textPrefab;
    // Start is called before the first frame update
    void Start()
    {
		float startY = menuItems.Length * -30;
        foreach(string s in menuItems) {
			GameObject menuItem = createMenuItem(s);
			menuItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startY);
			startY += 60;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	GameObject createMenuItem(string s) {
/* 		GameObject UItextGO = new GameObject("Text");
		UItextGO.transform.SetParent(transform);

		RectTransform trans = UItextGO.AddComponent<RectTransform>();
		trans.anchoredPosition = new Vector2(0, 0);

		Text text = UItextGO.AddComponent<Text>();
		text.text = s;
		text.fontSize = 14; */
		GameObject menuItem = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		menuItem.transform.SetParent(transform);
		Text text = menuItem.GetComponent<Text>();
		text.text = s;
		
		return menuItem;
	}
}
