using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//used for navigation from events to events in scene without needing to hard code it into each script
public class RoomTask : MonoBehaviour {

    public UnityEvent m_StartActions; //actions to happen when event is started
    public float m_DelayTime = 0.0f;

    private bool m_IsStarted = false;
    private bool m_IsFinished = false;


    public void StartTask()
    {
        if (BeckaRoomManager.Singleton.IsCurrentTask(this))
        {
            StartCoroutine(DelayCall());
        }

    }

    private IEnumerator DelayCall()
    {
        yield return new WaitForSeconds(m_DelayTime);

        m_IsStarted = true;
        m_StartActions.Invoke();

        Debug.Log("Starting Task");

    }


    //called from another script that is specific to the event happening (ie: putting film on lightbox)
    public void FinishTask()
    {
        //only if event is started can it be finished
        if (m_IsStarted == false)
        {
            return;
        }

        m_IsFinished = true;
        m_IsStarted = false;

        BeckaRoomManager.Singleton.StartNextTask();
    }

    public bool IsComplete()
    {
        return m_IsFinished;
    }
}
