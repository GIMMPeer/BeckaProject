using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioInteractable : MonoBehaviour {

    public AudioClip m_Clip;

    public GameObject m_DistortionSpherePrefab;

    public Vector3 m_OffsetPosition;

    private GameObject m_CurrentDistortionSphere;

    private float m_AnimationStartTime = 0.0f;
    private bool m_SphereIsAnimating;
	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().clip = m_Clip;
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_SphereIsAnimating)
        {
            if (Time.time - m_AnimationStartTime >= 3f)
            {
                Destroy(m_CurrentDistortionSphere);
                m_CurrentDistortionSphere = null;
                m_SphereIsAnimating = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<AudioSource>().Stop();
        }
	}

    private void OnTriggerStay(Collider other)
    {
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            Debug.Log("Staying with: " + other.name);
            //if hand is touching object
            if (other.GetComponent<NewtonVR.NVRHand>() && m_CurrentDistortionSphere == null && !GetComponent<AudioSource>().isPlaying)
            {
                PlayAudioSystem();
            }
        }
 
    }

    public void PlayAudioSystem()
    {
        GetComponent<AudioSource>().Play();
        m_CurrentDistortionSphere = Instantiate(m_DistortionSpherePrefab);


        m_CurrentDistortionSphere.transform.position = transform.position + m_OffsetPosition;

        m_CurrentDistortionSphere.GetComponent<Animator>().Play("SphereExpand");
        m_AnimationStartTime = Time.time;
        m_SphereIsAnimating = true;
    }
}
