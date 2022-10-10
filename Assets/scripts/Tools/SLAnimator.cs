using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLAnimator : MonoBehaviour
{
	Dictionary<GameObject, Vector3[]> vectorAnimations = new Dictionary<GameObject, Vector3[]>();
	Dictionary<GameObject, float[]> vectorAnimationTimings = new Dictionary<GameObject, float[]>();

	public void Animate(GameObject go, Vector3 endVector, float animationDuration) {
		Animate(go, go.transform.position, endVector, animationDuration);
	}
	
	public void Animate(GameObject go, Vector3 startVector, Vector3 endVector, float animationDuration) {
		// Debug.Log("SLAnimator.Animate()...");
		go.transform.position = startVector;
		if(!vectorAnimations.ContainsKey(go)) {
			vectorAnimations.Add(go, new Vector3[2]);
			vectorAnimationTimings.Add(go, new float[2]);
		}
		
		vectorAnimations[go][0] = startVector;
		vectorAnimations[go][1] = endVector;
		vectorAnimationTimings[go][0] = Time.time;
		vectorAnimationTimings[go][1] = animationDuration;
	}
	
    // Update is called once per frame
    void Update()
    {
		List<GameObject> removes = new List<GameObject>();
		
        foreach(KeyValuePair<GameObject, Vector3[]> animation in vectorAnimations) {
			// Debug.Log(animation);
			float delta = (Time.time - vectorAnimationTimings[animation.Key][0]) / vectorAnimationTimings[animation.Key][1];
			if(delta > 1)
				delta = 1;
			animation.Key.transform.position = Vector3.Lerp(animation.Value[0], animation.Value[1], delta);
			if(delta >= 1) {
				removes.Add(animation.Key);
			}
		}
		
		foreach(GameObject go in removes) {
			vectorAnimations.Remove(go);
			vectorAnimationTimings.Remove(go);
		}
    }
}