using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioEventDispatcher : MonoBehaviour {

    public AudioSource m_AudioSource; //Given from objects with AudioSource components.  
    public List<AudioEvent> m_AudioEvents; //List of all events to trigger when timestamps are triggered
	
	// Update is called once per frame
	void Update () {

        float curTimestamp = m_AudioSource.time; //Set current timestamp for 

        for (int i = 0; i < m_AudioEvents.Count; i++)
        {
            if(m_AudioEvents[i].m_Timestamp <= curTimestamp)
            {
                m_AudioEvents[i].m_Event.Invoke(); //Call the event
                m_AudioEvents.RemoveAt(i); //This removes the current object in the list to allow for multiple events to run more efficiently
            }
        }
	}
}

[System.Serializable]
public class AudioEvent
{
    public string m_Name;
    public float m_Timestamp;
    public UnityEngine.Events.UnityEvent m_Event;
    
}
