using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLooker : MonoBehaviour {

	public GameObject player;
	public GameObject head;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		head.transform.LookAt(player.transform.position);
	}
}
