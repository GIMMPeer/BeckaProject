using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//checks painting status
//will be check by game manager on exiting room to see if it is complete
public class PaintingTask : MonoBehaviour
{
    public UnityEvent m_OnStartingPainting;
    public UnityEvent m_OnFinishPainting;

    private bool m_IsComplete = false;
    private bool m_IsStarted = false;

    private int m_StartingNodeCount = 0;

    private void Start()
    {
        m_StartingNodeCount = transform.childCount;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public bool IsTaskComplete()
    {
        return m_IsComplete;
    }

    //called when child collider is hit
    public void UpdatePaintingStatus(GameObject colliderObj)
    {
        Destroy(colliderObj);

        if (transform.childCount <= 1) //destorying child doesn't update child count immedietely
        {
            Debug.Log("Painting Complete");
            m_IsComplete = true;
            m_OnFinishPainting.Invoke();
        }
        else if (transform.childCount <= m_StartingNodeCount - 1 && !m_IsStarted) //trigger when one painting node is gone (player has just started painting)
        {
            m_OnStartingPainting.Invoke();
            m_IsStarted = true;
        }
    }
}
