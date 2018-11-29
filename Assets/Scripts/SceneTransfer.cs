using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//placed in every scene to facilitate transfer of player to desired room
public class SceneTransfer : MonoBehaviour {

    public static SceneTransfer m_Singleton;
    public string m_SceneName;
    public GameManager.Room m_DestinationRoom;

    private void Awake()
    {
        m_Singleton = this;
    }

    //uses public fields to send player to next room
    public void GoToScene()
    {
        GameManager.m_Singleton.SetRoom(m_DestinationRoom);
        SceneManager.LoadScene(m_SceneName);
    }

    //called from event to send player to next room
    public void GoToSceneByName(string name)
    {
        GameManager.m_Singleton.SetRoom(m_DestinationRoom);
        SceneManager.LoadScene(name);
    }

}