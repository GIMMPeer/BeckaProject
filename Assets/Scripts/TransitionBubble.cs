using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to transition bubble and controls both resizing of sphere
//and teleporting of player
public class TransitionBubble : MonoBehaviour {

    public Vector3 m_ScalingFactor = new Vector3(1, 1, 1);
    public Transform m_TransitionLocation;

    private GameObject m_Player;

    private bool m_IsStartingTransport;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		//temporary for sizing the bubble manually
        if (Input.GetKey(KeyCode.P))
        {
            transform.localScale += m_ScalingFactor * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.O))
        {
            transform.localScale -= m_ScalingFactor * Time.deltaTime;
        }

        if (m_IsStartingTransport)
        {
            transform.localScale += m_ScalingFactor * Time.deltaTime;

            if (transform.localScale.magnitude >= 20) //20 is arbitrary size for teleport
            {
                TeleportPlayer();
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_Player = other.gameObject;
            m_IsStartingTransport = true;
        }
    }

    private void TeleportPlayer()
    {
        if (!m_Player) { return; }

        Vector3 distFromCenter = m_Player.transform.position - transform.position;
        Vector3 newPosition = m_TransitionLocation.position + distFromCenter;

        m_Player.transform.position = newPosition;

        m_IsStartingTransport = false;
        m_Player = null;
    }
}
