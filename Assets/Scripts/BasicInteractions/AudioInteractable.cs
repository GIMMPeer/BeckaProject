using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioInteractable : MonoBehaviour {

    public AudioClip m_Clip;

    public GameObject m_DistortionSpherePrefab;

    public bool m_UseDistortionSphere = true;

    public UnityEvent m_OnAudioComplete;

    private GameObject m_CurrentDistortionSphere;

    private float m_AnimationStartTime = 0.0f;
    private bool m_SphereIsAnimating;

    private bool m_IsInteracted = false; //true when player interacts with audio, false when audio has been completed
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

        if (m_IsInteracted)
        {
            //if audio is no longer playing
            if (!GetComponent<AudioSource>().isPlaying)
            {
                m_IsInteracted = false;
                m_OnAudioComplete.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<AudioSource>().Stop();
        }
	}

    public void PlayAudioSystem()
    {
        if (m_CurrentDistortionSphere != null || GetComponent<AudioSource>().isPlaying)
        {
            return;
        }

        if (GetComponent<AudioSource>().clip == null)
        {
            Debug.LogWarning("Audio Interactable doesn't have audio clip");
        }

        GetComponent<AudioSource>().Play();
        m_IsInteracted = true;

        if (m_UseDistortionSphere)
        {
            m_CurrentDistortionSphere = Instantiate(m_DistortionSpherePrefab);

            m_CurrentDistortionSphere.transform.position = transform.position; //TODO distortion sphere should spawn at player hand

            m_CurrentDistortionSphere.GetComponent<Animator>().Play("SphereExpand");
            m_AnimationStartTime = Time.time;
            m_SphereIsAnimating = true;
        }
    }
}
