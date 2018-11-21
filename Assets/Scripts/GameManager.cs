using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool m_IsPersistant = false;

    private Room m_CurrentRoom;
	// Use this for initialization
	void Awake ()
    {
        SetupPersistance();
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
    }

    private void SetupPersistance()
    {
        //Setup persistant game manager
        GameManager[] otherManagers = FindObjectsOfType<GameManager>();

        if (otherManagers.Length <= 1)
        {
            m_IsPersistant = true;
        }

        if (m_IsPersistant == false)
        {
            //this object is a copy of the gamemanager and needs to be deleted
            Debug.Log("Persistant Game Manager Found. Deleting Local One");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        m_Singleton = this;
    }
}
