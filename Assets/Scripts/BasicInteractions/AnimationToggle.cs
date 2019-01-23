using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AnimationToggle : MonoBehaviour {

    public string m_TriggerName = "Toggle";
    public AudioClip m_Clip;

    private Animator m_Animator;
    // Use this for initialization

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        GetComponent<AudioSource>().clip = m_Clip;
    }

    public void ToggleAnimation()
    {
        m_Animator.SetTrigger(m_TriggerName);
        GetComponent<AudioSource>().Play();
    }
}
