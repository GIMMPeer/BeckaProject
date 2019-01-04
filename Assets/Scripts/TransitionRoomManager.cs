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
    // Use this for initialization

    private void Awake()
    {
        m_Singleton = this;

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
}
