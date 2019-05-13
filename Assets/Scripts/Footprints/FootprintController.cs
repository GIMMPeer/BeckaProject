using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//uses array of transforms and spawns footprints in order with delays
public class FootprintController : MonoBehaviour {

    public GameObject m_FootprintPrefab;
    public Transform[] m_FootprintLocations;

    [Space]

    public float m_StepInterval = 1.0f; //steps per second
    [Range(0.0f, 10.0f)]
    public float m_StepVolume = 5f;
    public bool m_Loop = false;
    public bool m_PlayAudio = true;
    public bool m_PlayOnStart = false;

    public UnityEvent m_OnCompleteCycle;

    private float m_StartingTime;
    private bool m_IsPlayingFootsteps = false;
    private int m_FootstepIndex = 0;
	// Use this for initialization
	void Start ()
    {
        m_StartingTime = Time.time;

        if (m_PlayOnStart)
        {
            StartFootsteps();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_IsPlayingFootsteps)
        {
            if (Time.time - (m_StartingTime + m_StepInterval) >= 0)
            {
                //full series of foot cycles is complete
                if (m_FootstepIndex >= m_FootprintLocations.Length)
                {
                    if (m_Loop)
                    {
                        StartFootsteps(); //loop endlessly
                        return;
                    }

                    m_IsPlayingFootsteps = false;
                    m_OnCompleteCycle.Invoke();
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

    //spawns footstep object at footstep index transform
    private void PlaceNewFootstep(int locationIndex)
    {
        GameObject g = Instantiate(m_FootprintPrefab, m_FootprintLocations[locationIndex].position, Quaternion.identity, transform);
        g.transform.localScale = m_FootprintLocations[locationIndex].localScale;
        g.transform.localRotation = m_FootprintLocations[locationIndex].localRotation;

        bool isLeftFoot = m_FootstepIndex % 2 == 0 ? true : false; //if footstep index is even, then we say it is a left footstep

        if (m_PlayAudio)
        {
            g.GetComponent<Footprint>().SetFootAudio(isLeftFoot, m_StepVolume);
        }

        m_StartingTime = Time.time;

        m_FootstepIndex++;
    }

    //start new footstep series at first transform position
    public void StartFootsteps()
    {
        m_IsPlayingFootsteps = true;
        m_FootstepIndex = 0;

        PlaceNewFootstep(0);
    }

    public void StopFootsteps()
    {
        m_IsPlayingFootsteps = false;
    }
}
