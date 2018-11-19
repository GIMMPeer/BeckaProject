using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour {

    public static SceneTransfer m_Singleton;
    public string m_SceneName;

    private void Awake()
    {
        m_Singleton = this;
    }

    public void GoToScene()
    {
        SceneManager.LoadScene(m_SceneName);
    }
}