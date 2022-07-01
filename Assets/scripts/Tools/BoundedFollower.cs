using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedFollower : MonoBehaviour
{
	public GameObject target;
	public float xOffset = 0;
	public float yOffset = 0;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float potentialX = target.transform.position.x + xOffset;
		if(potentialX < minX)
			potentialX = minX;
		if(potentialX > maxX)
			potentialX = maxX;
		
		float potentialY = target.transform.position.y + yOffset;
		if(potentialY < minY)
			potentialY = minY;
		if(potentialY > maxY)
			potentialY = maxY;
		
		transform.position = new Vector3(potentialX, potentialY, transform.position.z);
    }
}
