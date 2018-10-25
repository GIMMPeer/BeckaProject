using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioInteractable : MonoBehaviour {

    public AudioClip m_Clip;
	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().clip = m_Clip;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //if hand is touching object
        if (other.GetComponent<OVRGrabber>())
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
