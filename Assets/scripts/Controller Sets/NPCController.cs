using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoeController : MonoBehaviour
{
    public Animator animator;
	public ActionController actionController;
	public IntentController intentController;
	public InteractionController interactionController;
	
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
