using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//checks painting status
//will be check by game manager on exiting room to see if it is complete
public class PaintingTask : MonoBehaviour
{
    private bool m_IsComplete = false;

    // Use this for initialization
    void Start ()
    {
        m_IsComplete = true; //temporary for testing
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public bool IsTaskComplete()
    {
        return m_IsComplete;
    }
}
