using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
	public SceneController sceneController;
	public GameObject selectionIcon;
	public SLAnimator slAnimator;
	public GameObject title;
	public float titleDropTime = 1.5f;
	public AudioClip menuSelectionSound;
	public float spacing = 0.2f;
	public string[] options;
	
	int index = 0;
	bool openToInput = true;
	float startingY;
	
    // Start is called before the first frame update
    void Start()
    {
		startingY = selectionIcon.transform.position.y;
        slAnimator.Animate(title, new Vector3(0, 0, title.transform.position.z), titleDropTime);
    }

    // Update is called once per frame
    void Update()
    {
		if(selectButtonPressed()) {
			selectCurrentIndex();
			return;
		}
		
        if(!openToInput)
			return;
		
		float v = Input.GetAxisRaw("Vertical");
		if(v == 0)
			return;
		
		openToInput = false;
		Invoke("openInput", 0.35f);
		GetComponent<AudioSource>().PlayOneShot(menuSelectionSound);
		if(v > 0) {
			navigatePrevious();
			return;
		}
		
		if(v < 0) {
			navigateNext();
			return;
		}
    }
	
	void openInput() {
		openToInput = true;
	}
	
	void navigateNext() {
		index++;
		if(index >= options.Length)
			index = 0;
		
		navigateCurrentIndex();
	}
	
	void navigatePrevious() {
		index--;
		if(index < 0)
			index = options.Length - 1;
		
		navigateCurrentIndex();
	}
	
	void navigateCurrentIndex() {
		selectionIcon.transform.position = new Vector3(selectionIcon.transform.position.x, startingY - spacing * index, selectionIcon.transform.position.z);
	}
	
	bool selectButtonPressed() {
		if(Input.GetButtonDown("Jump"))
			return true;
		
		return Input.GetAxisRaw("Fire1") > 0;
	}
	
	void selectCurrentIndex() {
		string selection = options[index];
		switch(selection) {
			case "Start":
				sceneController.transitionTo("Desert 1");
				return;
			case "High Score":
				sceneController.transitionTo("High Scores");
				return;
			case "Credits":
				sceneController.transitionTo("Credits");
				return;
		}
	}
}