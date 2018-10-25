using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this object gets the audio from hearableobject, which is triggered from a trigger collider
//audio volume will be based on distance to object
//audio source is separate from player, and stethoscope sets it to player's location
public class Stethoscope : MonoBehaviour {

    public Transform m_Player;
    public AudioSource m_StethoscopeAudioSource;

    public float m_MaxHearingRange = 2.0f;

    private HearableObject m_HearableObject = null;

    private float m_MaxAudioDistance = float.MaxValue;
	// Use this for initialization
	void Start ()
    {
        //set position of audio source right on player
        m_StethoscopeAudioSource.transform.parent = m_Player;
        m_StethoscopeAudioSource.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check to see if hearable object is being tracked by stethoscope
        if (m_HearableObject != null)
        {
            float distanceToObject = Vector3.Distance(m_HearableObject.transform.position, transform.position);

            float mappedVolume = 1.0f - Map(distanceToObject, 0.0f, m_MaxAudioDistance, 0.0f, 1.0f); //1.0 minus to invert value
            m_StethoscopeAudioSource.volume = mappedVolume;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        HearableObject ho = other.gameObject.GetComponent<HearableObject>();
        if (ho == null) { return; } //if collided object is not a Hearable Object then bail
        if (m_StethoscopeAudioSource.isPlaying) { return; } // this means you can only hear one object at a time

        m_HearableObject = ho;

        m_StethoscopeAudioSource.clip = m_HearableObject.GetAudioClip();
        m_StethoscopeAudioSource.Play();

        //used to set up range where player can hear object, trigger range being the farthest point away
        m_MaxAudioDistance = Vector3.Distance(m_HearableObject.transform.position, transform.position);
    }

    private float Map(float value, float startMin, float startMax, float endMin, float endMax)
    {
        float diff = (value - startMin) / (startMax - startMin);

        float newValue = (endMin * (1 - diff)) + (endMax * diff);

        return newValue;
    }
}
