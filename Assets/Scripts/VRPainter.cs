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
    public RenderTexture m_CanvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material m_BaseMaterial; // The material of our base texture (Were we will save the painted texture)

    private bool saving = false;
    private int brushCounter;

    private float m_ColorTickVal;

    private const int MAX_BRUSH_COUNT = 100;
	
	// Update is called once per frame
	void Update ()
    {   
        //left index trigger held down
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Draw();
        }
	}

    //checks to see if you hit a drawable object, then if so sets UV worldPosition
    bool HitTestUVPosition(ref Vector3 uvWorldPosition, ref Material hitMaterial)
    {
        //only look for layer that is "drawable"
        int layerMask = 1 << LayerMask.NameToLayer("Drawable");

        RaycastHit hit;
        Ray cursorRay = new Ray(m_BrushTransform.position, m_BrushTransform.forward);
        if (Physics.Raycast(cursorRay, out hit, 200, layerMask))
        {
            //used to ensure we only paint on our mesh, not on anyone elses
            if (hit.collider.gameObject.name != gameObject.name)
            {
                return false;
            }

            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - m_CanvasCamera.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - m_CanvasCamera.orthographicSize;//To center the UV on Y
            uvWorldPosition.z = 0.0f;

            hitMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
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
        Material hitMaterial = null;

        if (HitTestUVPosition(ref uvWorldPosition, ref hitMaterial))
        {
            //create brush object at found uvWorldPosition
            GameObject brushObj = Instantiate(m_BrushEntity);
            
            brushObj.transform.parent = m_BrushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = uvWorldPosition;

            bool isColorable = IsShaderColorable(hitMaterial); //check if shader used on material is a splat map shader, or a simple colored over shader

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
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            saving = true;
            Invoke("SaveTexture", 0.1f);
        }
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
    void SaveTexture()
    {
        brushCounter = 0;

        RenderTexture.active = m_CanvasTexture; //sets active render texture so tex.ReadPixels can read from that
        Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, m_CanvasTexture.width, m_CanvasTexture.height), 0, 0); //reads pixels from render 
        tex.Apply();
        RenderTexture.active = null;
        m_BaseMaterial.mainTexture = tex; //Put the painted texture as the base

        foreach (Transform child in m_BrushContainer.transform)
        {//Clear brushes
            Destroy(child.gameObject);
        }
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);
    }

    void ShowCursor()
    {
        saving = false;
    }

}
