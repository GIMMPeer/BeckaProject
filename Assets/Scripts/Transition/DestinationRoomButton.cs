using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gets input from NVR interactable to set room destination
public class DestinationRoomButton : MonoBehaviour {

    public string m_SceneName;
    public GameManager.RoomNames m_Room;

    public void SetupButton(string sceneName, GameManager.RoomNames room)
    {
        m_SceneName = sceneName;
        m_Room = room;
    }

    public void SetDestination()
    {
        Debug.Log("Destination Setting");

        GameManager.m_Singleton.SetNextRoom(m_Room); //sets next room to teleport to

        SceneTransfer.m_Singleton.m_SceneName = m_SceneName;
    }
}
