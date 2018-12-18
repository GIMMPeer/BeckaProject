using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotationLate : MonoBehaviour {

    public bool m_FreezeXRotation;
    public bool m_FreezeYRotation;
    public bool m_FreezeZRotation;

    private Quaternion m_StartingRotation;

    private void Start()
    {
        m_StartingRotation = transform.rotation;
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        Vector3 setRotation = transform.rotation.eulerAngles;

        if (m_FreezeXRotation) setRotation.x = m_StartingRotation.eulerAngles.x;
        if (m_FreezeYRotation) setRotation.y = m_StartingRotation.eulerAngles.y;
        if (m_FreezeZRotation) setRotation.z = m_StartingRotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(setRotation);
	}
}
