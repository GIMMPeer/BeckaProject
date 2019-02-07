using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLooker : MonoBehaviour {

	public GameObject player;
	public GameObject head;
    public Vector3 m_Offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 lookPosition = player.transform.position + m_Offset;
		head.transform.LookAt(lookPosition);
	}
}
