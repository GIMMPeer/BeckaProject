using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour {

    public static SceneTransfer m_Singleton;
    public string m_SceneName;
    public GameManager.Room m_DestinationRoom;

    private void Awake()
    {
        m_Singleton = this;
    }

    public void GoToScene()
    {
        GameManager.m_Singleton.SetRoom(m_DestinationRoom);
        SceneManager.LoadScene(m_SceneName);
    }

    //when coming from Maze this doesn't work as it doesn't send correct room,  use unity custom event to fix this maybe
    public void GoToSceneByName(string name)
    {
        GameManager.m_Singleton.SetRoom(m_DestinationRoom);
        SceneManager.LoadScene(name);
    }

}