using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//used for navigation from events to events in scene without needing to hardcode it into each script

//TODO switch name to RoomTask, as each of these has a start and a finish
public class RoomEvent : MonoBehaviour {

    public UnityEvent m_Actions; //actions to happen when event is started

    private bool m_IsStarted = false;
    private bool m_IsFinished = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void StartEvent()
    {
        if (BeckaRoomManager.Singleton.IsCurrentTask(this))
        {
            m_IsStarted = true;
            m_Actions.Invoke();
        }
    }

    //called from another script that is specific to the event happenin (ie: putting film on lightbox)
    public void FinishEvent()
    {
        //only if event is started can it be finished
        if (m_IsStarted == false)
        {
            return;
        }

        m_IsFinished = true;
        m_IsStarted = false;

        BeckaRoomManager.Singleton.StartNextEvent();
    }

    public bool IsComplete()
    {
        return m_IsFinished;
    }
}
