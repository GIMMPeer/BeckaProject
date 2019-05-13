using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//calls events on start if selected room is complete 
public class RoomCompleteEventHandler : MonoBehaviour {

    public GameManager.RoomNames m_Room;
    public UnityEvent m_OnRoomComplete;
	// Use this for initialization
	void Start ()
    {
        RoomContainer[] containers = GameManager.m_Singleton.GetAllRoomContainers();

        foreach (RoomContainer container in containers)
        {
            if (m_Room == container.m_Room)
            {
                GetComponent<Renderer>().material = container.m_CanvasMaterial;
                Debug.Log("Materials: " + GetComponent<Renderer>().sharedMaterials.Length);
            }
        }

        bool roomIsComplete = GameManager.m_Singleton.IsRoomComplete(m_Room);

        if (roomIsComplete)
        {
            m_OnRoomComplete.Invoke();
        }
	}
}
