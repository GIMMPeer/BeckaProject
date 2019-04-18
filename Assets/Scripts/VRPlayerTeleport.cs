using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerTeleport : MonoBehaviour {

    public Transform m_RHandTransform;
    public Transform m_LHandTransform;
    public Transform m_HeadTransform;
    
    private enum m_Handedness { head, left, right};

    private ParticleSystem m_lastParticleSystem;
    
    
	void Update ()
    {
        RaycastOut(m_Handedness.head);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            RaycastOut(m_Handedness.right); //try teleport with right hand
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            RaycastOut(m_Handedness.left); //try teleport with left hand
        }

    }

    private void RaycastOut(m_Handedness type)
    {

        /*
        Vector3 origin = Vector3.zero;
        Vector3 direction = Vector3.zero;
        if (type == m_Handedness.head)
        {
            origin = m_HeadTransform.position;
            direction = m_HeadTransform.forward;
        }else if (type == m_Handedness.left)
        {
            origin = m_LHandTransform.position;
            direction = m_LHandTransform.forward;
        }else if(type == m_Handedness.right)
        {
            origin = m_RHandTransform.position;
            direction = m_RHandTransform.forward;
        }
        */

        Vector3 origin = m_HeadTransform.position;
        Vector3 direction = m_HeadTransform.forward;

        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        int layerMask = 1 << LayerMask.NameToLayer("VRTeleportLoc"); //only look for layer that is "VRTeleportLoc"
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            if(type == m_Handedness.head)
            {
                if (hit.collider.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>() != null)
                {
                    ParticleSystem PS = hit.collider.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();

                    if(m_lastParticleSystem == null)
                    {
                        m_lastParticleSystem = PS;
                    }
                    else if(m_lastParticleSystem == PS)
                    {
                        if (!PS.isPlaying)
                        {
                            PS.Play();
                        }
                    }
                    else
                    {
                        m_lastParticleSystem.Stop();
                        m_lastParticleSystem.Clear();
                        m_lastParticleSystem = PS;
                    }
                }
                else{
                    Debug.Log("No particle system found, m_handedness.none");
                }
            }
            else
            {
                if(hit.collider.gameObject.GetComponent<TeleportLocation>() != null)
                {
                    TeleportLocation tpl = hit.collider.gameObject.GetComponent<TeleportLocation>();
                    if (tpl){
                        tpl.TeleportPlayer(transform);
                    }
                }
                else{
                    Debug.Log("No teleport location found, m_handedness.left or .right");
                }
            }   
        }
    }
}
