using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationToggle : MonoBehaviour {

    public string m_TriggerName = "Toggle";

    private Animator m_Animator;
    // Use this for initialization

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void ToggleAnimation()
    {
        m_Animator.SetTrigger(m_TriggerName);
    }
}
