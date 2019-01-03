using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Each VRPainter needs
 *   1 Dynamic Canvas (m_CanvasCamera, Quad, Brush Container)
 *   1 RenderTexture
 *   2 Materials (BaseMaterial, ObjectMaterial)
*/
public class VRPainter : MonoBehaviour {

    [Header("Shaders")]
    public Shader m_StandardShader;
    public Shader m_PaintableShader;

    [Space(5)]

    [Header("Brush Object Settings")]
    public Transform m_BrushTransform;
    public GameObject m_BrushEntity;
    public Gradient m_Gradient;

    [Space(5)]

    [Header("Object Specfic Settings")]
    public Transform m_BrushContainer;
    public Camera m_CanvasCamera;
    public Material m_BaseMaterial;
    //public RenderTexture m_CanvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    //public Material m_BaseMaterial; // The material of our base texture (Were we will save the painted texture)

    private bool saving = false;
    private int brushCounter;

    private float m_ColorTickVal;

    private const int MAX_BRUSH_COUNT = 100;

    private VRPaintable m_PaintingTarget;
    private RenderTexture m_PaintableRenderTexture;
    private bool m_PaintingTargetFound = false;
	
	// Update is called once per frame
	void Update ()
    {   
        //left index trigger held down
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Draw();
        }
        //if drawing is stopped
        else if (m_PaintingTargetFound)
        {
            OnDrawingEnd();
        }

    }

    //checks to see if you hit a drawable object, then if so sets UV worldPosition
    bool HitTestUVPosition(ref Vector3 uvWorldPosition, ref GameObject hitObject)
    {

        //only look for layer that is "drawable"
        int layerMask = 1 << LayerMask.NameToLayer("Drawable");

        RaycastHit hit;
        Ray cursorRay = new Ray(m_BrushTransform.position, m_BrushTransform.forward);
        if (Physics.Raycast(cursorRay, out hit, 200, layerMask))
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
    void Draw()
    {
        if (saving)
            return;
        Vector3 uvWorldPosition = Vector3.zero;
        GameObject hitObject = null;

        if (HitTestUVPosition(ref uvWorldPosition, ref hitObject))
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

            bool isColorable = IsShaderColorable(hitObject.GetComponent<MeshRenderer>().material); //check if shader used on material is a splat map shader, or a simple colored over shader

            if (isColorable)
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
            StartCoroutine(SaveTexture(false, 0.1f));
        }
    }

    //sets up scene objects with painting target
    void OnDrawingStart()
    {
        m_PaintingTargetFound = true;

        m_PaintableRenderTexture = m_PaintingTarget.GetRenderTexture();

        m_CanvasCamera.targetTexture = m_PaintableRenderTexture;
        m_BaseMaterial.mainTexture = m_PaintingTarget.GetMainTexture();

        bool isColorable = IsShaderColorable(m_PaintingTarget.gameObject.GetComponent<MeshRenderer>().material); //check if shader used on material is a splat map shader, or a simple colored over shader

        //if objects is normal paintable color main texture, if it is a reveal paint splat map
        if (isColorable)
        {
            m_PaintingTarget.SetObjectMainTexture(m_PaintableRenderTexture);
        }
        else
        {
            m_PaintingTarget.SetObjectSplatTexture(m_PaintableRenderTexture);
        }
    }

    //sets up scene objects with painting target
    void OnDrawingEnd()
    {
        m_PaintingTargetFound = false;

        bool isColorable = IsShaderColorable(m_PaintingTarget.gameObject.GetComponent<MeshRenderer>().material); //check if shader used on material is a splat map shader, or a simple colored over shader

        //if objects is normal paintable color main texture, if it is a reveal paint splat map
        if (isColorable)
        {
            m_PaintingTarget.SetObjectMainTexture(m_PaintingTarget.GetMainTexture());
        }
        else
        {
            m_PaintingTarget.SetObjectSplatTexture(m_PaintingTarget.GetMainTexture());
        }

        saving = true;
        StartCoroutine(SaveTexture(true, 0.1f));
    }

    bool IsShaderColorable(Material material)
    {
        Shader shader = material.shader;

        if (shader.name == m_StandardShader.name)
        {
            return true;
        }

        else if (shader.name == m_PaintableShader.name)
        {
            return false;
        }

        else
        {
            Debug.Log("Material does not have paintable shader");
            return false;
        }
    } 

    //Sets the base material with a our canvas texture, then removes all our brushes
    IEnumerator SaveTexture(bool endingDrawing, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        brushCounter = 0;

        m_PaintingTarget.SaveTexture(m_BaseMaterial);

        foreach (Transform child in m_BrushContainer.transform)
        {//Clear brushes
            Destroy(child.gameObject);
        }
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);

        if (endingDrawing) //TODO this may be entirely unnecessary because these globals are reset anyways OnDrawStart
        {
            m_PaintableRenderTexture = null;
            m_PaintingTarget = null; 
        }
    }

    void ShowCursor()
    {
        saving = false;
    }

}
