using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportLocation : MonoBehaviour {

    public UnityEvent m_OnPlayerTeleported;

    private ParticleSystem m_ParticleSystem;
	// Use this for initialization

    public void TeleportPlayer(Transform player)
    {
        Vector3 newPlayerPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        player.position = newPlayerPos;
        player.rotation = Quaternion.LookRotation(transform.forward);

        m_OnPlayerTeleported.Invoke();
    }
}
