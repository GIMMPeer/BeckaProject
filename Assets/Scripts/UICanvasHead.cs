using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasHead : MonoBehaviour {

    public GameObject m_Head;

    private bool foundHead = false;
    
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Camera conected to canvas, Update: " + this.GetComponent<Canvas>().worldCamera.name);

        if (this.GetComponent<Canvas>().worldCamera != m_Head.GetComponent<Camera>())
        {
            this.GetComponent<Canvas>().worldCamera = m_Head.GetComponent<Camera>();
        }

        
    }
}
