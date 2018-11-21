using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager m_Singleton;

    public enum Room
    {
        DoctorOffice,
        GirlsRoom,
        GirlMaze,
        GroceryStore,
        GroceryStoreMaze,
        TeenRoom,
        TeenRoomMaze,
        DepressionRoom,
        DepressionRoomMaze,
        DoctorOfficeReturn,
        DoctorOfficeMaze,
        Bathroom
    }

    public string m_MazeSceneName; //used for spawning player at right location when coming in and out of maze

    [HideInInspector]
    public bool m_IsPersistant = false;

    [SerializeField]
    private Room m_CurrentRoom;
    private NewtonVR.NVRPlayer m_Player;

    public int TestCounter = 0;

    //start is not called on loading into scene

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupPersistance();

        if (m_IsPersistant == false)
        {
            return;
        }

        m_Player = FindObjectOfType<NewtonVR.NVRPlayer>();

        Debug.Log("Current Room: " + m_CurrentRoom + "Test Index: " + TestCounter);

        if (SceneManager.GetActiveScene().name == m_MazeSceneName)
        {
            //player is in maze
            SpawnPlayerInMaze();
        }
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Current Room: " + m_CurrentRoom);
        }
	}

    public Room GetRoom()
    {
        return m_CurrentRoom;
    }

    public void SetRoom(Room room)
    {
        m_CurrentRoom = room;
        Debug.Log("Set Current Room to " + m_CurrentRoom);
        TestCounter++;
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

    private void SpawnPlayerInMaze()
    {
        Transform t = MazeManager.m_Singleton.GetSpawnLocation(m_CurrentRoom);
        m_Player.transform.position = t.position;
        m_Player.transform.rotation = t.rotation;
    }

}
