using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

//gets objects that can be locked/linked to hand and locks them to hand

[RequireComponent(typeof(NVRHand))]
public class HandLocker : MonoBehaviour {

    public HandLocker m_OtherHandLocker; //maybe do this with finding?
    public Vector3 m_PositionOffset;
    public Quaternion m_RotationOffset;

    private Transform m_GrabbedObject;

    public void OnStartGrabbing()
    {
        Debug.Log("Grabbing Locker");
        Transform grabbedObject = GetComponent<NVRHand>().CurrentlyInteracting.gameObject.transform;

        if (grabbedObject.tag == "HandLockable")
        {
            if (m_GrabbedObject)
            {
                //if you are trying to remove your own grabbed object
                RemoveGrabbedObject();
            }
            else
            {
                if (m_OtherHandLocker.GetGrabbedObject())
                {
                    //check if other hand has object
                    //if so then remove it from other hand
                    m_OtherHandLocker.RemoveGrabbedObject();
                    SetGrabbedObject(GetComponent<NVRHand>().CurrentlyInteracting.gameObject.transform);
                }
                else
                {
                    SetGrabbedObject(GetComponent<NVRHand>().CurrentlyInteracting.gameObject.transform);
                }
            }
        }

    }

    public Transform GetGrabbedObject()
    {
        return m_GrabbedObject;
    }

    private void SetGrabbedObject(Transform grabbedObjectTransform)
    {
        m_GrabbedObject = grabbedObjectTransform;
        m_GrabbedObject.parent = transform;

        m_GrabbedObject.localPosition = Vector3.zero + m_PositionOffset;
        m_GrabbedObject.localRotation = Quaternion.identity * m_RotationOffset;

        m_GrabbedObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void RemoveGrabbedObject()
    {
        m_GrabbedObject.parent = null;
        m_GrabbedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        m_GrabbedObject = null;
    }
}
