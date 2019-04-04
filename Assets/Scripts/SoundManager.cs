using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager m_Instance;

    private AudioSource m_CurrentSource;
    // Use this for initialization

    private void Awake()
    {
        m_Instance = this;
    }

    public void SetAudioSource(AudioSource source)
    {
        if (m_CurrentSource != null)
        {
            if (m_CurrentSource.isPlaying)
            {
                m_CurrentSource.Stop();
            }
        }

        m_CurrentSource = source;
    }
}
