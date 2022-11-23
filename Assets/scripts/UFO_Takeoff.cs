using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO_Takeoff : MonoBehaviour 
{
    Animator anim;

// Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space)) anim.SetTrigger("UFOTakeoffTrigger");
    }
    private void OnTriggerEnter2D()  {
    
    }
}
