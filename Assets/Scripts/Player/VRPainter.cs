using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPainter : MonoBehaviour {

    [Space(5)]

    [Header("Brush Object Settings")]
    public Transform m_LBrushTransform;
    public Transform m_RBrushTransform;
    public GameObject m_LPaintBrushPrefab;
    public GameObject m_RPaintBrushPrefab;
    public GameObject m_BrushEntity;
    public Gradient m_Gradient;
    public float m_BrushSize = 1f;

    [Space(5)] 

    [Header("Object Specfic Settings")]
    public Transform m_BrushContainer;
    public Camera m_CanvasCamera;
    public Material m_BaseMaterial;

    private bool saving = false;
    private int brushCounter;

    private float m_ColorTickVal;

    private const int MAX_BRUSH_COUNT = 100;

    private GameObject m_LInstantiatedPaintBrush;
    private GameObject m_RInstantiatedPaintBrush;
    private VRPaintable m_PaintingTarget;
    private RenderTexture m_PaintableRenderTexture;
    private bool m_PaintingTargetFound = false;
	
	// Update is called once per frame
	void Update ()
    {
        //Searches all paintable objects in scene, compares distance, and returns true if within 2m.
        if (!IsInRangeOfPaintable()) return;

        //create paintbrush infront of hand so it it grabbed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) //the instant the primary trigger is hit
        {
            //TODO clean this up a bit
            m_LInstantiatedPaintBrush = Instantiate(m_LPaintBrushPrefab);
            m_LInstantiatedPaintBrush.transform.position = m_LBrushTransform.position + m_LBrushTransform.forward * 0.05f;
            m_LInstantiatedPaintBrush.transform.rotation = m_LBrushTransform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
            m_LBrushTransform.gameObject.GetComponent<NewtonVR.NVRHand>().BeginInteraction(m_LInstantiatedPaintBrush.GetComponent<NewtonVR.NVRInteractable>());
        }

        else if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) //Runs as long as the primary trigger is held
        {
            Draw(m_LInstantiatedPaintBrush.transform.GetChild(0)); //get only child of paintbrush which is paintbrush tip
        }

        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)) //the instant the primary trigger is released
        {
            Destroy(m_LInstantiatedPaintBrush);
            m_LInstantiatedPaintBrush = null;
        }

        else if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) //The instant the secondary trigger is hit
        {
            //TODO clean this up a bit
            m_RInstantiatedPaintBrush = Instantiate(m_RPaintBrushPrefab);
            m_RInstantiatedPaintBrush.transform.position = m_RBrushTransform.position + m_RBrushTransform.forward * 0.05f;
            m_RInstantiatedPaintBrush.transform.rotation = m_RBrushTransform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
            m_RBrushTransform.gameObject.GetComponent<NewtonVR.NVRHand>().BeginInteraction(m_RInstantiatedPaintBrush.GetComponent<NewtonVR.NVRInteractable>());
        }

        else if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) //Runs as long as the secondary trigger is held 
        {
            Draw(m_RInstantiatedPaintBrush.transform.GetChild(0)); //get only child of paintbrush which is paintbrush tip
        }

        else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) //The instant the secondary trigger is released
        {
            Destroy(m_RInstantiatedPaintBrush);
            m_RInstantiatedPaintBrush = null;
        }
        
        //if drawing is stopped
        else if (m_PaintingTargetFound)
        {
            OnDrawingEnd();
        }

    }

    //this function is called every frame and is probably very expensive    //We could run this through a coroutine and have it trigger only once every 1/8 second instead of every frame.
    //looks at all paintables in scene and checks distance
    bool IsInRangeOfPaintable()
    {
        VRPaintable[] paintables = FindObjectsOfType<VRPaintable>();
        foreach(VRPaintable paintable in paintables)
        {
            float distance = Vector3.Distance(paintable.gameObject.transform.position, m_RBrushTransform.position);

            if (distance <= 1.0)
            {
                return true;
            }
        }

        return false;
    }


    //checks to see if you hit a drawable object, then if so sets UV worldPosition
    bool HitTestUVPosition(Transform brushTransform, ref Vector3 uvWorldPosition, ref GameObject hitObject)
    {
        //only look for layer that is "drawable"
        int layerMask = 1 << LayerMask.NameToLayer("Drawable");

        RaycastHit hit;
        Ray cursorRay = new Ray(brushTransform.position, brushTransform.forward);
        if (Physics.Raycast(cursorRay, out hit, 0.2f, layerMask))
        {
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - m_CanvasCamera.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - m_CanvasCamera.orthographicSize;//To center the UV on Y
            uvWorldPosition.z = 0.0f;


            hitObject = hit.collider.gameObject;
            m_PaintingTarget = hitObject.GetComponent<VRPaintable>();
            return true;
        }
        else
        {
            return false;
        }
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void Draw(Transform brushTransform)
    {
        if (saving)
            return;
        Vector3 uvWorldPosition = Vector3.zero;
        GameObject hitObject = null;

        if (HitTestUVPosition(brushTransform, ref uvWorldPosition, ref hitObject))
        {
            //if not target previously i.e: start with new drawable
            if (m_PaintingTargetFound == false)
            {
                m_PaintingTarget = hitObject.GetComponent<VRPaintable>();
                OnDrawingStart();
            }

            //create brush object at found uvWorldPosition
            GameObject brushObj = Instantiate(m_BrushEntity);
            
            brushObj.transform.parent = m_BrushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = uvWorldPosition;
            brushObj.transform.localScale *= m_BrushSize; //Change brush size based on canvas size - GG

            if (m_PaintingTarget.IsColorable())
            {
                float lerpVal = Mathf.PingPong(m_ColorTickVal, 1);
                Color paintColor = m_Gradient.Evaluate(lerpVal);

                brushObj.GetComponent<SpriteRenderer>().color = paintColor;

                m_ColorTickVal += 0.01f;
            }

            else
            {
                brushObj.GetComponent<SpriteRenderer>().color = Color.black;
            }

        }
        //if we didn't hit what we drawable
        else
        {
            //if we previously had a render texture found
            if (m_PaintingTargetFound)
            {
                OnDrawingEnd();
            }
        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT && m_PaintingTarget != null)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            saving = true;
            Invoke("SaveTexture", 0.1f);
        }
    }

    //sets up scene objects with painting target
    void OnDrawingStart()
    {
        m_PaintingTargetFound = true;

        m_PaintableRenderTexture = m_PaintingTarget.GetRenderTexture();

        m_CanvasCamera.targetTexture = m_PaintableRenderTexture;
        m_BaseMaterial.mainTexture = m_PaintingTarget.GetMainTexture();

        m_PaintingTarget.SetObjectTexture(m_PaintableRenderTexture);
    }

    //sets up scene objects with painting target
    void OnDrawingEnd()
    {
        m_PaintingTargetFound = false;

        m_PaintingTarget.SetObjectTexture(m_PaintingTarget.GetMainTexture());

        saving = true;
        Invoke("SaveTexture", 0.1f);
    } 

    //Sets the base material with a our canvas texture, then removes all our brushes
    void SaveTexture()
    {
        brushCounter = 0;

        m_PaintingTarget.SaveTexture(m_BaseMaterial);

        foreach (Transform child in m_BrushContainer.transform)
        {//Clear brushes
            Destroy(child.gameObject);
        }

        Invoke("ShowCursor", 0.1f);
    }

    void ShowCursor()
    {
        saving = false;
    }

}
