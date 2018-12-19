using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPoint : MonoBehaviour {

    public Transform m_SnapPointTransform;
    public Color m_HighlightedColor = Color.white;

    private Material m_Material;
    private GameObject m_AttachedObject;
	// Use this for initialization
	void Start ()
    {
        m_Material = GetComponent<MeshRenderer>().material;
        m_Material.SetColor("_Color", Color.clear);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        m_Material.SetColor("_Color", m_HighlightedColor);
    }

    private void OnTriggerStay(Collider other)
    {
        NewtonVR.NVRInteractableItem nVRInteractableItem = other.GetComponent<NewtonVR.NVRInteractableItem>();

        if (nVRInteractableItem)
        {
            //if item is released/is not grabbed while inside the collider
            if (!nVRInteractableItem.IsAttached)
            {
                SnapObjectToPoint(nVRInteractableItem.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //make snap point invisible
        m_Material.SetColor("_Color", Color.clear);

        if (m_AttachedObject)
        {
            m_AttachedObject.GetComponent<Rigidbody>().isKinematic = false;
            m_AttachedObject = null;
        }
    }

    private void SnapObjectToPoint(GameObject snappingObject)
    {
        if (snappingObject.GetComponent<Rigidbody>())
        {
            snappingObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        else
        {
            Debug.LogError("Snapping Object " + snappingObject.name + "doesn't have a Rigidbody");
            return;
        }

        Transform snappingObjectTransform = snappingObject.transform;

        snappingObjectTransform.position = m_SnapPointTransform.position;
        snappingObjectTransform.rotation = m_SnapPointTransform.rotation;

        m_AttachedObject = snappingObject;
        m_Material.SetColor("_Color", Color.clear);

    }
}
