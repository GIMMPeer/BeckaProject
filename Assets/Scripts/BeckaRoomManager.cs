using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton in each room to manage and string together each event that needs to happen
public class BeckaRoomManager : MonoBehaviour
{
    public static BeckaRoomManager Singleton;

    public RoomEvent[] m_AllRoomEvents;

    private int m_RoomEventIndex = 0;
	// Use this for initialization
	void Awake ()
    {
        Singleton = this;
	}

    private void Start()
    {
        m_AllRoomEvents[0].StartEvent();
    }


    public void StartNextEvent()
    {
        m_RoomEventIndex++;

        Debug.Log("TaskNumber: " + m_RoomEventIndex);
        if (AllRoomEventsCompleted())
        {
            Debug.Log("All tasks done");
            //move to next scene
            return;
        }

        RoomEvent newRoomEvent = m_AllRoomEvents[m_RoomEventIndex];

        if (m_AllRoomEvents[m_RoomEventIndex] == null || m_AllRoomEvents.Length == 0)
        {
            Debug.LogError("RoomEvent is null or length of room events is 0");
            return;
        }

        newRoomEvent.StartEvent();
    }

    //check if sent in task is current task in sequence
    public bool IsCurrentTask(RoomEvent roomEvent)
    {
        return m_AllRoomEvents[m_RoomEventIndex].Equals(roomEvent);
    }

    private bool AllRoomEventsCompleted()
    {
        /*
        //TODO make roomevents array get smaller each time an event is finished to improve on performance
        foreach(RoomEvent re in m_AllRoomEvents)
        {
            if (re.IsComplete() == false)
            {
                //if any roomevent is not done then return false
                return false;
            }
        }

        return true; //if all rooms are not false then all room events are complete*/

        if (m_RoomEventIndex >= m_AllRoomEvents.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
