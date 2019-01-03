using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//persistent class that holds previous room locations and player's inventory
public class GameManager : MonoBehaviour {

    public static GameManager m_Singleton;

    //TODO don't have maze rooms, just have normal rooms and keep track of previous rooms for simplicity
    //Room enum MUST BE IN ORDER OF COMPLETION
    public enum Room
    {
        DoctorOffice,
        GirlsRoom,
        GroceryStore,
        TeenRoom,
        DepressionRoom,
        Bathroom,
        TransitionRoom
    }

    [HideInInspector]
    public bool m_IsPersistant = false;

    [SerializeField]
    private Room m_CurrentRoom;

    private Room m_PreviousRoom;

    private bool[] m_RoomsPainted; //array that holds each room status in order as a bool (DoctorOffice = 0, TransitionRoom = 6)

    private NewtonVR.NVRPlayer m_Player;

    //start is not called on loading into scene

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        m_RoomsPainted = new bool[7]; //7 rooms total

        for (int i = 0; i < m_RoomsPainted.Length; i++) //initialize all rooms status to false when starting game
        {
            m_RoomsPainted[i] = false;
        }
    }

    //On loaded sets up persistence and spawns player in maze
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupPersistance();

        if (m_IsPersistant == false)
        {
            return;
        }

        m_Player = FindObjectOfType<NewtonVR.NVRPlayer>();
        if (m_CurrentRoom == Room.TransitionRoom)
        {
            //player is in transition room
            //SpawnPlayerInMaze();
            TransitionRoomManager.m_Singleton.LoadCompletedPaintings();
        }
    }

    public Room GetCurrentRoom()
    {
        return m_CurrentRoom;
    }

    public void SetCurrentRoom(Room room)
    {
        m_PreviousRoom = m_CurrentRoom;
        m_CurrentRoom = room;
    }

    public void SetRoomPaintedStatus(bool status, Room room)
    {
        m_RoomsPainted[(int)room] = status;
    }

    public bool GetRoomStatus(Room room)
    {
        return m_RoomsPainted[(int)room];
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

    //goes to maze manager and spawns player in maze based on room
    private void SpawnPlayerInMaze()
    {
        Transform t = MazeManager.m_Singleton.GetSpawnLocation(m_CurrentRoom);
        m_Player.transform.position = t.position;
        m_Player.transform.rotation = t.rotation;
    }

}
