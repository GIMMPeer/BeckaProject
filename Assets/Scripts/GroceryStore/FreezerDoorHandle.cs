using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerDoorHandle : MonoBehaviour {
    
    public float m_Speed = 1;
    public float m_degreeRotation = 90;
    public bool m_DoorIsOnRightSide = false;

    private float lerpPercent = 0;
    private bool startLerp = false;
    private bool doorOpen = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (startLerp)
        {
            
        }
	}

    public void OpenDoor()
    {

        //Lerp
        //Unactive box collider when door is moving.
        

        this.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = false; //Turn back on when lerp is finished

        Vector3 parentRotation = this.transform.parent.transform.localRotation.eulerAngles;
        Vector3 newRotation = new Vector3();
        if (!doorOpen && m_DoorIsOnRightSide)
        {
            newRotation = new Vector3(parentRotation.x, -m_degreeRotation, parentRotation.z);
            doorOpen = true;
        }
        if (!doorOpen && !m_DoorIsOnRightSide)
        {
            newRotation = new Vector3(parentRotation.x, m_degreeRotation, parentRotation.z);
            doorOpen = true;
        }
        else
        {
            newRotation = new Vector3(parentRotation.x, 0, parentRotation.z);
            doorOpen = false;
        }

        this.transform.parent.transform.localRotation = Quaternion.Euler(newRotation);
    }
}
