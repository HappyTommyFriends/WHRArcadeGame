using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{
	GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Level3.Start()...");
		gameManager = gameObject.GetComponent<GameManager>();
		gameManager.score = 0;
		gameManager.addScore(Persistance.score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
