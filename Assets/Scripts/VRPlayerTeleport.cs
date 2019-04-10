using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerTeleport : MonoBehaviour {

    public Transform m_RHandTransform;
    public Transform m_LHandTransform;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            TryTeleport(false); //try teleport with right hand
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            TryTeleport(true); //try teleport with left hand
        }

    }

    private void TryTeleport(bool isRightHand)
    {
        Vector3 origin = isRightHand ? m_RHandTransform.position : m_LHandTransform.position;
        Vector3 direction = isRightHand ? m_RHandTransform.forward : m_LHandTransform.forward;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        //only look for layer that is "VRTeleportLoc"
        int layerMask = 1 << LayerMask.NameToLayer("VRTeleportLoc");

        if (Physics.Raycast(ray, out hit, 10, layerMask))
        {
            //on hiting vrteleportlocation

            TeleportLocation tpl = hit.collider.gameObject.GetComponent<TeleportLocation>();
            if (tpl)
            {
                tpl.TeleportPlayer(transform);
            }
        }
    }
}
