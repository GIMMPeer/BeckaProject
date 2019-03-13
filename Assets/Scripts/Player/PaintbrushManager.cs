using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//must be attached to object with VRPainter
[RequireComponent(typeof(VRPainter))]
public class PaintbrushManager : MonoBehaviour {

    public enum PaintBrushState {LHand, RHand, Empty}

    [Header("Brush Settings")]
    public Transform m_LBrushTransform;
    public Transform m_RBrushTransform;
    public GameObject m_PaintBrushPrefab;

    public Vector3 m_Offset;
    public float m_BrushPaintDistance = 1.0f; //max distance away from paintable where paintbrush will spawn

    private GameObject m_InstantiatedPaintBrush;
    private bool m_PaintingTargetFound = false;

    private VRPainter m_VRPainter;
    private PaintBrushState m_PaintBrushState;

    // Use this for initialization
    void Start () {
        m_VRPainter = gameObject.GetComponent<VRPainter>();
	}

    // Update is called once per frame
    void Update()
    {
        if (m_InstantiatedPaintBrush)
        {
            m_VRPainter.Draw(m_InstantiatedPaintBrush.transform.GetChild(0)); //get only child of paintbrush which is paintbrush tip
        }

        //Searches all paintable objects in scene, compares distance, and returns true if within 2m.
        if (!IsInRangeOfPaintable()) return;

        //create paintbrush infront of hand so it it grabbed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) //the instant the primary trigger is hit
        {
            CreatePaintBrush(m_LBrushTransform, PaintBrushState.LHand);            
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) //The instant the secondary trigger is hit
        {
            CreatePaintBrush(m_RBrushTransform, PaintBrushState.RHand);
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) //The instant the secondary trigger is released
        {
            if (m_InstantiatedPaintBrush == null) return;

            if (m_PaintBrushState == PaintBrushState.RHand)
            {
                DestroyPaintBrush();
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)) //The instant the secondary trigger is released
        {
            if (m_InstantiatedPaintBrush == null) return;

            if (m_PaintBrushState == PaintBrushState.LHand)
            {
                DestroyPaintBrush();
            }
        }


        //if drawing is stopped
        else if (m_PaintingTargetFound)
        {
            m_VRPainter.EndDrawing();
        }

    }

    private void CreatePaintBrush(Transform handTransform, PaintBrushState state)
    {
        if (m_InstantiatedPaintBrush)
        {
            return;
        }

        m_InstantiatedPaintBrush = Instantiate(m_PaintBrushPrefab);
        m_InstantiatedPaintBrush.transform.position = handTransform.position + handTransform.forward * 0.05f;
        m_InstantiatedPaintBrush.transform.rotation = handTransform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
        handTransform.gameObject.GetComponent<NewtonVR.NVRHand>().BeginInteraction(m_InstantiatedPaintBrush.GetComponent<NewtonVR.NVRInteractable>());

        m_PaintBrushState = state;
    }

    private void DestroyPaintBrush()
    {
        if (!m_InstantiatedPaintBrush) return;

        m_LBrushTransform.gameObject.GetComponent<NewtonVR.NVRHand>().EndInteraction(m_InstantiatedPaintBrush.GetComponent<NewtonVR.NVRInteractable>());
        m_RBrushTransform.gameObject.GetComponent<NewtonVR.NVRHand>().EndInteraction(m_InstantiatedPaintBrush.GetComponent<NewtonVR.NVRInteractable>());

        m_VRPainter.EndDrawing(); //always end drawing whenever player destroys paintbrush

        Destroy(m_InstantiatedPaintBrush);
        m_InstantiatedPaintBrush = null;
    }

    //this function is called every frame and is probably very expensive    //We could run this through a coroutine and have it trigger only once every 1/8 second instead of every frame.
    //looks at all paintables in scene and checks distance
    bool IsInRangeOfPaintable()
    {
        VRPaintable[] paintables = FindObjectsOfType<VRPaintable>();
        foreach (VRPaintable paintable in paintables)
        {
            float distance = Vector3.Distance(paintable.gameObject.transform.position + m_Offset, m_RBrushTransform.position);

            if (distance <= m_BrushPaintDistance)
            {
                return true;
            }
        }

        if (m_InstantiatedPaintBrush)
        {
            DestroyPaintBrush();
        }

        return false;
    }
}
