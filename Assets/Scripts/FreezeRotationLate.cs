using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotationLate : MonoBehaviour {

    public bool m_FreezeXRotation;
    public bool m_FreezeYRotation;
    public bool m_FreezeZRotation;

    private Quaternion m_StartingRotation;

    //use lateupdate because this should be late thing to apply to text
    void LateUpdate ()
    {
        Vector3 setRotation = transform.localRotation.eulerAngles;

        //if any axis is frozen set that axis' rotation to starting rotation to lock it
        if (m_FreezeXRotation) { setRotation.x = m_StartingRotation.eulerAngles.x; }
        if (m_FreezeYRotation) { setRotation.y = m_StartingRotation.eulerAngles.y; }
        if (m_FreezeZRotation) { setRotation.z = m_StartingRotation.eulerAngles.z; }

        transform.localRotation = Quaternion.Euler(setRotation);
	}

    public void SetStartingRotation(Quaternion startingRotation)
    {
        m_StartingRotation = startingRotation;
    }
}
