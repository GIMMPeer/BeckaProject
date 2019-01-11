using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchingEye : MonoBehaviour {

    public Transform m_Target;

    public float m_MinBlinkSpeed = 0.75f;
    public float m_MaxBlinkSpeed = 1.25f;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Animator>().speed = Random.Range(m_MinBlinkSpeed, m_MaxBlinkSpeed);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(m_Target);
	}
}
