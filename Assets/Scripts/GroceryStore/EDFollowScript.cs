using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDFollowScript : MonoBehaviour {

    public GameObject m_Player = null;
    public float m_Speed = 1.0f;
    public float m_MinDistance = 0.1f; //in Unity units
    public float m_DelayTime = 2.0f; //in seconds

    private float StartMoveTime = 0.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.LookAt(m_Player.transform);

        if (IsAbleToMove())
        {
            Vector3 moveVector = Vector3.Lerp(transform.position, m_Player.transform.position, Time.deltaTime * m_Speed);
            moveVector.y = 0; //so ED doesn't fly
            transform.position = moveVector;
        }
    }

    bool IsAbleToMove()
    {
        if (Vector3.Distance(m_Player.transform.position, transform.position) <= m_MinDistance) return false;

        if (Time.time - StartMoveTime >= m_DelayTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
