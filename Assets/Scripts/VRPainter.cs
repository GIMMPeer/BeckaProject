using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPainter : MonoBehaviour {

    public Camera m_ViewingCamera;
    public Camera m_CanvasCamera;

    public GameObject m_BrushEntity;
    public Transform m_BrushContainer;

    public RenderTexture m_CanvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material m_BaseMaterial; // The material of our base texture (Were we will save the painted texture)

    private bool saving = false;
    private int brushCounter;

    private const int MAX_BRUSH_COUNT = 100;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {        
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
	}

    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        /*RaycastHit hit;
        Ray cursorRay = new Ray(m_ViewingCamera.transform.position, m_ViewingCamera.transform.forward);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            if (hit.collider.gameObject.tag != "Drawable")
            {
                return false;
            }

            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x;
            uvWorldPosition.y = pixelUV.y;
            uvWorldPosition.z = 0.0f;
            return true;
        }
        else
        {
            return false;
        }*/

        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = Camera.main.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - m_CanvasCamera.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - m_CanvasCamera.orthographicSize;//To center the UV on Y
            uvWorldPosition.z = 0.0f;
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
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            GameObject brushObj = Instantiate(m_BrushEntity);
            brushObj.GetComponent<SpriteRenderer>().color = Color.red;
            
            brushObj.transform.parent = m_BrushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            saving = true;
            Invoke("SaveTexture", 0.1f);
        }
    }

    //Sets the base material with a our canvas texture, then removes all our brushes
    void SaveTexture()
    {
        brushCounter = 0;
        RenderTexture.active = m_CanvasTexture;
        Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, m_CanvasTexture.width, m_CanvasTexture.height), 0, 0);
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
