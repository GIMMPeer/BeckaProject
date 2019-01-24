using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioEventDispatcher : MonoBehaviour {


    public AudioSource m_AudioSource;
    public List<AudioEvent> m_AudioEvents;
    
	
	// Update is called once per frame
	void Update () {

        float curTimestamp = m_AudioSource.time;

        for (int i = 0; i < m_AudioEvents.Count; i++)
        {
            if(m_AudioEvents[i].m_Timestamp <= curTimestamp)
            {
                m_AudioEvents[i].m_Event.Invoke();
                m_AudioEvents.RemoveAt(i);
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
