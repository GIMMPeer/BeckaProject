using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour {

    public GameObject m_FootprintPrefab;
    public Transform[] m_FootprintLocations;

    public float m_StepInterval = 1.0f; //steps per second

    private float m_StartingTime;
    private bool m_IsPlayingFootsteps = false;
    private int m_FootstepIndex = 0;
	// Use this for initialization
	void Start ()
    {
        m_StartingTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_IsPlayingFootsteps)
        {
            Debug.Log("Starting Footsteps");
            if (Time.time - (m_StartingTime + m_StepInterval) >= 0)
            {
                //interval is complete
                if (m_FootstepIndex >= m_FootprintLocations.Length)
                {
                    m_IsPlayingFootsteps = false;
                    return;
                }

                PlaceNewFootstep(m_FootstepIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartFootsteps();
        }
	}

    private void PlaceNewFootstep(int locationIndex)
    {
        GameObject g = Instantiate(m_FootprintPrefab, m_FootprintLocations[locationIndex].position, Quaternion.identity, transform);
        g.transform.localScale = m_FootprintLocations[locationIndex].localScale;
        g.transform.localRotation = m_FootprintLocations[locationIndex].localRotation;

        m_StartingTime = Time.time;

        m_FootstepIndex++;
    }

    public void StartFootsteps()
    {
        m_IsPlayingFootsteps = true;
        m_FootstepIndex = 0;

        PlaceNewFootstep(0);
    }
}
