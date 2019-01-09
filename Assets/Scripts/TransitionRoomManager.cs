using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//get persistent data to make painting for each room either on or off depending on completion
public class TransitionRoomManager : MonoBehaviour
{
    public static TransitionRoomManager m_Singleton;

    public DestinationRoomButton m_DocOfficeBtn;
    public DestinationRoomButton m_GirlRoomBtn;
    public DestinationRoomButton m_GroceryStoreBtn;
    public DestinationRoomButton m_TeenRoomBtn;
    public DestinationRoomButton m_DepressionRoomBtn;
    public DestinationRoomButton m_DocOfficeRevisitBtn;
    public DestinationRoomButton m_BathroomBtn;

    private RoomContainer[] m_AllRooms;

    private void Awake()
    {
        m_Singleton = this;

        Invoke("LateStart", 0.5f);
    }

    private void LateStart()
    {
        m_AllRooms = GameManager.m_Singleton.GetAllRoomContainers();

        ResetAllPaintings();

        LoadRoomInfo();
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
            if (container.m_NextRoom == GameManager.Room.TransitionRoom)
            {
                Debug.LogError("Transition room should never be used as m_NextRoom for container");
                return;
            }

            if (container.m_IsComplete)
            {
                container.m_CanvasMaterial.mainTexture = container.m_PaintingTexture;

                DestinationRoomButton button = GetDestinationButton(container.m_NextRoom);
                button.SetupButton(container.m_NextSceneName, container.m_NextRoom); //feeds the scene name info and room enum to buttons

                button.gameObject.SetActive(true);
            }
        }
    }

    private DestinationRoomButton GetDestinationButton(GameManager.Room room)
    { 
        DestinationRoomButton btn = null;
        switch(room)
        {
            case GameManager.Room.DoctorOffice:
                btn = m_DocOfficeBtn;
                break;
            case GameManager.Room.GirlsRoom:
                btn = m_GirlRoomBtn;
                break;
            case GameManager.Room.GroceryStore:
                btn = m_GroceryStoreBtn;
                break;
            case GameManager.Room.TeenRoom:
                btn = m_TeenRoomBtn;
                break;
            case GameManager.Room.DepressionRoom:
                btn = m_DepressionRoomBtn;
                break;
            case GameManager.Room.DoctorOfficeRevisit:
                btn = m_DocOfficeRevisitBtn;
                break;
            case GameManager.Room.Bathroom:
                btn = m_BathroomBtn;
                break;
            default:
                Debug.Log(room);
                Debug.LogError("Enum sent into Get Destination Button doesn't have a case");
                break;
        }

        return btn;
    }
}
