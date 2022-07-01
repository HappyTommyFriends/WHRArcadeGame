using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDisplayHearts : MonoBehaviour
{
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public GameObject heart4;
	public GameObject heart5;
	public GameObject heart6;
	public GameObject heart7;
	public GameObject heart8;
	public GameObject heart9;
	public GameObject heart10;
	
	public void display(float hp) {
		heart1.SetActive(hp > 0);
		heart2.SetActive(hp > 1);
		heart3.SetActive(hp > 2);
		heart4.SetActive(hp > 3);
		heart5.SetActive(hp > 4);
		heart6.SetActive(hp > 5);
		heart7.SetActive(hp > 6);
		heart8.SetActive(hp > 7);
		heart9.SetActive(hp > 8);
		heart10.SetActive(hp > 9);
	}
}
