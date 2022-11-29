using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
	public float totalSceneTime = 3f;
	public SceneController sceneController;
	public string magLockMessage = "whatever";

    // Start is called before the first frame update
    void Start()
    {
		Persistance.clearReplay("Desert 1");
        Invoke("End", totalSceneTime);
    }

	void End() {
    SerialController.instance.MessageSend(magLockMessage);
		sceneController.transitionTo("High Scores");
	}

    // Update is called once per frame
    void Update()
    {

    }
}
