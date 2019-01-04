using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearableObject : MonoBehaviour {

    public AudioClip m_AudioClip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetAudioClip()
    {
        return m_AudioClip;
    }
}
