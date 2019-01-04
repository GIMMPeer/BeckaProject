using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationRoomButton : MonoBehaviour {

    public GameManager.Room m_DestinationRoom;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDestination()
    {
        Debug.Log("Destination Setting");
        TransitionRoomManager.m_Singleton.SetDestinationRoom(m_DestinationRoom);
    }
}
