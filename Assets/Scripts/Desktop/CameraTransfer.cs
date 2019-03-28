using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransfer : MonoBehaviour {

    public Camera m_TransferCamera;
    public float m_LerpSpeed = 1f;

    private bool m_CameraIsSwapped = false;

    private bool m_IsLerping = false;
    private Vector3 m_LerpTarget;
    private Quaternion m_LerpRotation;

    private float m_LerpTime = 0.0f;
    // Use this for initialization
    void Start ()
    {
        m_TransferCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.C))
        {
            SwapCamera();
        }

        if (m_IsLerping)
        {
            m_TransferCamera.transform.position = Vector3.Lerp(m_TransferCamera.transform.position, m_LerpTarget, m_LerpTime);
            m_TransferCamera.transform.rotation = Quaternion.Lerp(m_TransferCamera.transform.rotation, m_LerpRotation, m_LerpTime);

            if (m_LerpTime >= 1.0f)
            {
                m_LerpTime = 0;
                m_IsLerping = false;
                //we made it
                if (!m_CameraIsSwapped)
                {
                    GetComponent<Camera>().enabled = true;
                    m_TransferCamera.enabled = false;
                    m_TransferCamera.transform.position = transform.position;
                    m_TransferCamera.transform.rotation = transform.rotation;
                    Cursor.visible = false;
                }
                else
                {
                    m_TransferCamera.transform.position = m_LerpTarget;
                    Cursor.visible = true;
                }
            }

            m_LerpTime += Time.deltaTime * m_LerpSpeed;

        }
	}

    void SwapCamera()
    {
        if (m_IsLerping == true)
        {
            //to stop people from trying to swap while camera is currently lerping
            return;
        }

        m_IsLerping = true;

        if (m_CameraIsSwapped)
        {
            m_LerpTarget = transform.position;
            m_LerpRotation = transform.rotation;
            m_CameraIsSwapped = false;
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.GetComponent<CameraTransitionLocation>())
            {
                m_TransferCamera.transform.position = GetComponent<Camera>().transform.position;
                m_TransferCamera.transform.rotation = GetComponent<Camera>().transform.rotation;

                m_LerpTarget = hit.collider.gameObject.GetComponent<CameraTransitionLocation>().m_CameraPosition.position;
                m_LerpRotation = hit.collider.gameObject.GetComponent<CameraTransitionLocation>().m_CameraPosition.rotation;

                GetComponent<Camera>().enabled = false;
                m_TransferCamera.enabled = true;

                m_CameraIsSwapped = true;
            }
        }


    }
}
