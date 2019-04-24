using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//get persistent data to make painting for each room either on or off depending on completion
public class TransitionRoomManager : MonoBehaviour
{
    public static TransitionRoomManager m_Singleton;

    public bool m_AutoQueueNextRoom = false;
    public bool m_ShowStartingMenu = true;
    public GameObject m_MainMenuObject;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController m_FirstPersonController;

    public UnityEvent m_OnAllRoomsComplete;

    private RoomContainer[] m_AllRooms;

    private void Awake()
    {
        m_Singleton = this;

        Invoke("LateStart", 1.0f);
    }

    private void LateStart()
    {
        m_AllRooms = GameManager.m_Singleton.GetAllRoomContainers();

        ResetAllPaintings();

        LoadRoomInfo();
    }

    public void CreateMenu()
    {
        m_MainMenuObject.SetActive(true);
        //m_FirstPersonController.SetWalkSpeed(0);
    }

    public void ResetAllPaintings()
    {
        foreach (RoomContainer container in m_AllRooms)
        {
            container.m_CanvasMaterial.mainTexture = null;
        }
    }

    //loads textures and destination buttton data from gamemanager into transition room objects
    public void LoadRoomInfo()
    {
        foreach (RoomContainer container in m_AllRooms)
        {
            //if room has not been painted, and we are not looking at the transition room
            //sets painting to be filled and buttons to be active per room
            if (container.m_NextRoom == GameManager.RoomNames.TransitionRoom)
            {
                Debug.LogError("Transition room should never be used as m_NextRoom for container");
                return;
            }

            if (container.m_IsComplete)
            {
                container.m_CanvasMaterial.mainTexture = container.m_PaintingTexture;
            }
        }

        if (GameManager.m_Singleton.GetMostRecentRoom() == null)
        {
            //all rooms are complete
            m_OnAllRoomsComplete.Invoke();
        }
        else if (m_AutoQueueNextRoom)
        {
            //confused about this a bit
            //when loading back into tranistion scene, the current roomcontainer is actually next room to be loaded
            //therefore to autoqueue just assign scene transfer to gamemanagers current (on deck) room container
            Debug.Log("Current  Room Container: " + GameManager.m_Singleton.GetCurrentRoomContainer().m_Name);
            SceneTransfer.m_Singleton.m_SceneName = GameManager.m_Singleton.GetCurrentRoomContainer().m_SceneName;
        }
    }
}
