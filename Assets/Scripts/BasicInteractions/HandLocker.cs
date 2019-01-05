using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

//gets objects that can be locked/linked to hand and locks them to hand

[RequireComponent(typeof(NVRHand))]
public class HandLocker : MonoBehaviour {

    public Vector3 m_PositionOffset;
    public Quaternion m_RotationOffset;

    private Transform m_GrabbedObject;

    public void OnStartGrabbing()
    {
        Transform grabbedObject = GetComponent<NVRHand>().CurrentlyInteracting.gameObject.transform;

        if (grabbedObject.tag == "HandLockable")
        {
            SetGrabbedObject(GetComponent<NVRHand>().CurrentlyInteracting.gameObject.transform);
        }

    }

    private void SetGrabbedObject(Transform grabbedObjectTransform)
    {
        m_GrabbedObject = grabbedObjectTransform;
        m_GrabbedObject.parent = transform;

        m_GrabbedObject.localPosition = Vector3.zero + m_PositionOffset;
        m_GrabbedObject.localRotation = Quaternion.identity * m_RotationOffset;

        m_GrabbedObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
