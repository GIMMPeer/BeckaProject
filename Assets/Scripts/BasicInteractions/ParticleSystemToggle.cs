using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make this work on a trigger system with NVRHands
public class ParticleSystemToggle : MonoBehaviour {

    public ParticleSystem m_System;
    public Transform LHand;
    public Transform RHand;

    public float m_MinDistance = 2f;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float lHandDistance = Vector3.Distance(transform.position, LHand.position);
        float rHandDistance = Vector3.Distance(transform.position, RHand.position);

        //if hands are in range of system play, if not then turn off system
        if (lHandDistance <= m_MinDistance || rHandDistance <= m_MinDistance)
        {
            if (!m_System.isPlaying)
            {
                m_System.Play();
            }
        }
        else
        {
            if (m_System.isPlaying)
            {
                m_System.Stop();
            }
        }
    }

    public void ToggleSystem()
    {
        if (m_System.isPlaying)
        {
            m_System.Stop();
        }
        
        else
        {
            m_System.Play();
        }
    }
}
