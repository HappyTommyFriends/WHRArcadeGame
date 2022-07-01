using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public virtual void interact(Interaction interaction) {
		Debug.Log("interact should be overwritten in InteractionController child class.");
	}
}
