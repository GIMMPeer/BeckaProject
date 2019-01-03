using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPaintable : MonoBehaviour
{
    public Texture m_MainTexture;

    private RenderTexture m_RenderTexture;
	// Use this for initialization
	void Start ()
    {
        m_RenderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32); //create individual instance of render texture
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Draw()
    {

    }

    //merge texture from render texture to main texture
    public void SaveTexture(Material baseMaterial)
    { 
        RenderTexture.active = m_RenderTexture; //sets active render texture so tex.ReadPixels can read from that
        Texture2D tex = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0); //reads pixels from render 
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
    }

    public RenderTexture GetRenderTexture()
    {
        return m_RenderTexture;
    }

    public Texture GetMainTexture()
    {
        return m_MainTexture;
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        m_RenderTexture = renderTexture;
    }

    public void SetObjectMainTexture(Texture texture)
    {
        m_MainTexture = texture;
        GetComponent<MeshRenderer>().material.mainTexture = m_MainTexture;
    }

    private void OnDestroy()
    {
        m_RenderTexture.Release();
    }
}
