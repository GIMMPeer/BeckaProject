using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshCollider))]
public class VRPaintable : MonoBehaviour
{
    [Header("Shaders")]
    public Shader m_StandardShader;
    public Shader m_PaintableShader;

    private Texture m_MainTexture; //texture can be texture for splat map
    private RenderTexture m_RenderTexture;
    private bool m_IsColorable;

	void Start ()
    {
        m_RenderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32); //create individual instance of render texture

        m_IsColorable = IsShaderColorable(GetComponent<MeshRenderer>().material);
	}

    //merge texture from render texture to main texture (all on base material, not object material)
    public void SaveTexture(Material baseMaterial)
    { 
        RenderTexture.active = m_RenderTexture; //sets active render texture so tex.ReadPixels can read from that
        Texture2D tex = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0); //reads pixels from render 
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base

        Debug.Log("Saving Texture");
    }

    public RenderTexture GetRenderTexture()
    {
        return m_RenderTexture;
    }

    public Texture GetMainTexture()
    {
        return m_MainTexture;
    }

    public bool IsColorable()
    {
        return m_IsColorable;
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        m_RenderTexture = renderTexture;
    }

    //called from VRPainter and will set either splatmap or main texture based on object shader
    public void SetObjectTexture(Texture texture)
    {
        if (m_IsColorable)
        {
            SetMainTexture(texture);
        }
        else
        {
            SetSplatTexture(texture);
        }
    }

    private void SetMainTexture(Texture texture)
    {
        m_MainTexture = texture;
        GetComponent<MeshRenderer>().material.mainTexture = m_MainTexture;
    }

    private void SetSplatTexture(Texture texture)
    {
        m_MainTexture = texture;
        GetComponent<MeshRenderer>().material.SetTexture("_SplatMap", m_MainTexture);
    }

    private bool IsShaderColorable(Material material)
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

    private void OnDestroy()
    {
        m_RenderTexture.Release();
    }

}
