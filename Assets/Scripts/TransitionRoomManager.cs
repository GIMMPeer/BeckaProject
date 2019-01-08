using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//get persistent data to make painting for each room either on or off depending on completion
public class TransitionRoomManager : MonoBehaviour
{
    public static TransitionRoomManager m_Singleton;

    //TODO make these into arrays or make a large scale class that always has these 6 items 
    [Header("Painting Materials")]
    public Material m_DoctorOfficePainting;
    public Material m_GirlsRoomPainting;
    public Material m_GroceryStorePainting;
    public Material m_TeenRoomPainting;
    public Material m_DepressionRoomPainting;
    public Material m_BathroomPainting;

    [Header("Painting Textures")]
    public Texture2D m_DoctorOfficePaintingTex;
    public Texture2D m_GirlsRoomPaintingTex;
    public Texture2D m_GroceryStorePaintingTex;
    public Texture2D m_TeenRoomPaintingTex;
    public Texture2D m_DepressionRoomPaintingTex;
    public Texture2D m_BathroomPaintingTex;

    [Header("Room Scene Names")]
    public string m_DoctorOfficeSceneName;
    public string m_GirlsRoomSceneName;
    public string m_GroceryStoreSceneName;
    public string m_TeenRoomSceneName;
    public string m_DepressionSceneName;
    public string m_BathroomSceneName;
    // Use this for initialization

    private void Awake()
    {
        m_Singleton = this;

        ResetAllPaintings();
    }

    public void ResetAllPaintings()
    {
        //start all painting on wall to be blank
        m_DoctorOfficePainting.mainTexture = null;
        m_GirlsRoomPainting.mainTexture = null;
        m_GroceryStorePainting.mainTexture = null;
        m_TeenRoomPainting.mainTexture = null;
        m_DepressionRoomPainting.mainTexture = null;
        m_BathroomPainting.mainTexture = null;
    }

    public void LoadCompletedPaintings()
    {
        bool doctorStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.DoctorOffice);
        bool girlsStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.GirlsRoom);
        bool groceryStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.GroceryStore);
        bool teenStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.TeenRoom);
        bool depressionStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.DepressionRoom);
        bool bathroomStatus = GameManager.m_Singleton.GetRoomStatus(GameManager.Room.Bathroom);

        if (doctorStatus)
        {
            m_DoctorOfficePainting.mainTexture = m_DoctorOfficePaintingTex;
        }
        if (girlsStatus)
        {
            m_GirlsRoomPainting.mainTexture = m_GirlsRoomPaintingTex;
        }
        if (groceryStatus)
        {
            m_GroceryStorePainting.mainTexture = m_GroceryStorePaintingTex;
        }
        if (teenStatus)
        {
            m_TeenRoomPainting.mainTexture = m_TeenRoomPaintingTex;
        }
        if (depressionStatus)
        {
            m_DepressionRoomPainting.mainTexture = m_DepressionRoomPaintingTex;
        }
        if (bathroomStatus)
        {
            m_BathroomPainting.mainTexture = m_BathroomPaintingTex;
        }

    }

    public void SetDestinationRoom(GameManager.Room room)
    {
        switch (room)
        {
            case GameManager.Room.DoctorOffice:
                SceneTransfer.m_Singleton.m_SceneName = m_DoctorOfficeSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.DoctorOffice;
                break;
            case GameManager.Room.GirlsRoom:
                SceneTransfer.m_Singleton.m_SceneName = m_GirlsRoomSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.GirlsRoom;
                break;
            case GameManager.Room.GroceryStore:
                SceneTransfer.m_Singleton.m_SceneName = m_GroceryStoreSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.GroceryStore;
                break;
            case GameManager.Room.TeenRoom:
                SceneTransfer.m_Singleton.m_SceneName = m_TeenRoomSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.TeenRoom;
                break;
            case GameManager.Room.DepressionRoom:
                SceneTransfer.m_Singleton.m_SceneName = m_DepressionSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.DepressionRoom;
                break;
            case GameManager.Room.Bathroom:
                SceneTransfer.m_Singleton.m_SceneName = m_BathroomSceneName;
                SceneTransfer.m_Singleton.m_DestinationRoom = GameManager.Room.Bathroom;
                break;
        }
    }

    //looks at completed rooms and returns next room in sequence
    private GameManager.Room FindNextDestinationRoom()
    {
        //loop through all 6 rooms
        for (int i = 0; i < 6; i++)
        {
            GameManager.Room room = (GameManager.Room)i;
            bool roomStatus = GameManager.m_Singleton.GetRoomStatus(room);

            if (!roomStatus)
            {
                return room;
            }

        }

        return GameManager.Room.TransitionRoom;
    }
}
