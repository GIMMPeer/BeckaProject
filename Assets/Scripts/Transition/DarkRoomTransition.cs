using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomTransition : MonoBehaviour {

    public Transform m_Player;
    public Transform m_Door;
    public GameObject m_DarkBox;

    public float m_TriggerDistance = 2f;

    private bool m_PlayerInsideDarkBox = false;
   
	
	// Update is called once per frame
	void Update ()
    {
        float distance = Vector3.Distance(m_Player.position, m_Door.position);

        Vector3 playerDirection = m_Player.position - m_Door.position;

        float distanceDot = Vector3.Dot(playerDirection, m_Door.right); //door's right vector is door's forwards facing side, check to make sure player is on the correct side of the door
        float playerViewDot = Vector3.Dot(Camera.main.transform.forward, m_Door.right);

        if (distance >= m_TriggerDistance)
        {
            if (distanceDot <= 0.1f) //if player has crossed threshold of door
            {
                if (playerViewDot <= -0.4f) //if player is not looking at door
                {
                    if (m_PlayerInsideDarkBox)
                    {
                        SceneTransfer.m_Singleton.GoToScene();
                    }
                }
            }
        }
	}

    public void PlayerEnterDarkBox()
    {
        m_PlayerInsideDarkBox = true;
    }

    public void PlayerExitDarkBox()
    {
        m_PlayerInsideDarkBox = false;
    }
}
