using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//persistent class that holds previous room locations and player's inventory

//TODO make generic Room class that contains important information like scene names and enums
public class GameManager : MonoBehaviour {

    //change name of room to roomnames
    public enum RoomNames
    {
        DoctorOffice,
        GirlsRoom,
        GroceryStore,
        TeenRoom,
        DepressionRoom,
        Bathroom,
        DoctorOfficeRevisit,
        TransitionRoom
    }

    public static GameManager m_Singleton;

    [HideInInspector]
    public bool m_IsPersistant = false;

    public RoomContainer[] m_AllRooms;

    private RoomContainer m_CurrentRoomContainer;
    private RoomContainer m_NextRoomContainer;

    private float m_NarrationVolume;
    private float m_SoundFXVolume;
    private float m_MusicVolume;

    private void Start()
    {
        /*m_AudioMixer.SetFloat("SoundFXVolume", m_SoundFXVolume);
        m_AudioMixer.SetFloat("NarrationVolume", m_NarrationVolume);
        m_AudioMixer.SetFloat("MusicVolume", m_MusicVolume);*/

        Invoke("SetCurRoomContainer", 0.5f);

        TransitionRoomManager.m_Singleton.CreateMenu();
    }

    private void SetCurRoomContainer()
    {
        if(m_AllRooms.Length > 0)
        {
            m_CurrentRoomContainer = GetMostRecentRoom();
        }
    }

    //start is not called on loading into scene
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetRoomPaintedStatus(bool status)
    {
        m_CurrentRoomContainer.m_IsComplete = true;
    }

    //On loaded sets up persistence and spawns player in maze
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupPersistance();

        if (m_IsPersistant == false)
        {
            return;
        }

        if (m_NextRoomContainer == null) return;

        m_CurrentRoomContainer = m_NextRoomContainer; //when we load into new scene next room is now current room
        Debug.Log("Current Room: " + m_CurrentRoomContainer.m_Name); 
    }

    private void SetupPersistance()
    {
        //Setup persistant game manager
        GameManager[] otherManagers = FindObjectsOfType<GameManager>();

        if (otherManagers.Length <= 1)
        {
            m_IsPersistant = true;
            DontDestroyOnLoad(gameObject);
        }

        if (m_IsPersistant == false)
        {
            //this object is a copy of the gamemanager and needs to be deleted
            Debug.Log("Persistant Game Manager Found. Deleting Local One");
            Destroy(gameObject); //apparently when you destory gameobject, it still completes the function it was called from
            return;
        }

        m_Singleton = this;
    }

    public RoomContainer GetMostRecentRoom()
    {
        foreach (RoomContainer container in m_AllRooms)
        {
            if (container.m_IsComplete == true)
            {
                continue;
            }
            else
            {
                return container;
            }
        }

        //shouldn't ever get here
        Debug.Log("Get most recent room failed, all rooms complete");
        return null;
    }

    public void SetNextRoom(RoomNames room)
    {
        foreach (RoomContainer container in m_AllRooms)
        {
            if (room == container.m_Room)
            {
                m_NextRoomContainer = container;
                Debug.Log("Next Room: " + m_NextRoomContainer.m_Name);
            }
        }
    }

    public void TestDebug()
    {
        Debug.Log("Testing Hit");
    }

    public void SetAudioLevels(float fxVol, float narrVol, float musicVol)
    {
        m_SoundFXVolume = fxVol;
        m_NarrationVolume = narrVol;
        m_MusicVolume = musicVol;
    }

    public bool IsRoomComplete(RoomNames roomName)
    {
        //iterate through all rooms, check if roomname is same as parameter, return if its complete or not
        for (int i = 0; i < m_AllRooms.Length; i++)
        {
            RoomContainer container = m_AllRooms[i];
            RoomNames containerRoomName = container.m_Room;
            
            if (containerRoomName == roomName)
            {
                return container.m_IsComplete;
            }
        }
        return false;
    }

    //Public Getters

    public RoomContainer GetCurrentRoomContainer()
    {
        return m_CurrentRoomContainer;
    }

    public RoomContainer[] GetAllRoomContainers()
    {
        return m_AllRooms;
    }

}

//switch all properities to private, as nothing should be able to be changed
[System.Serializable]
public class RoomContainer
{
    public string m_Name;
    public string m_SceneName;
    public string m_NextSceneName;
    public GameManager.RoomNames m_Room;
    public GameManager.RoomNames m_NextRoom;

    public Texture2D m_PaintingTexture;
    public Material m_CanvasMaterial; //transition room canvas material, associated with each room

    public bool m_IsComplete = false; //room is complete if all painting have been painted
}
