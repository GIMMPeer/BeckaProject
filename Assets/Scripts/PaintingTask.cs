using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//checks painting status
//will be check by game manager on exiting room to see if it is complete
public class PaintingTask : MonoBehaviour
{
    private bool m_IsComplete = false;
	
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
        }
    }
}
