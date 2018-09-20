using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum m_ColorTransitionType
{
    BasicColor, Material, Texture
}

public class LightboxManager : MonoBehaviour {

    public bool m_ActivateLightbox;
    public m_ColorTransitionType m_TransType;
    [Space]

    [Header("Objects To Change Color:")]
    public GameObject[] m_Objects;
    [Space]

    [Header("Materials:")]
    public Material[] m_ObjectsBrightMat;
    [Space]

    [Header ("Textures")]
    public Texture[] m_ObjectsBrightTxt;



    private Texture[] m_ObjectsDullTxt;
    private Material[] m_ObjectsDullMat;



    // Use this for initialization
    void Start () {
		
        if(m_TransType == m_ColorTransitionType.Material)
        {
            for (var i = 0; i < m_Objects.Length; i++)
            {
                m_ObjectsDullMat[i] = m_Objects[i].GetComponent<Renderer>().material;
            }
        }
        else if (m_TransType == m_ColorTransitionType.Texture)
        {
            for (var i = 0; i < m_Objects.Length; i++)
            {
                m_ObjectsDullTxt[i] = m_Objects[i].GetComponent<Renderer>().material.mainTexture;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		

        if (m_ActivateLightbox)
        {
            if (m_TransType == m_ColorTransitionType.BasicColor)
            {
                for (var i = 0; i < m_Objects.Length; i++)
                {
                    m_Objects[i].GetComponent<Renderer>().material.color = Random.ColorHSV(0,1,0.75f,1,0.75f,1);
                    m_ActivateLightbox = false;
                }
            }
            else if(m_TransType == m_ColorTransitionType.Material)
            {
                for (var i = 0; i < m_Objects.Length; i++)
                {
                    m_Objects[i].GetComponent<Renderer>().material = m_ObjectsDullMat[i];
                    m_ActivateLightbox = false;
                }
            }
            else if(m_TransType == m_ColorTransitionType.Texture)
            {
                for (var i = 0; i < m_Objects.Length; i++)
                {
                    m_Objects[i].GetComponent<Renderer>().material.mainTexture = m_ObjectsDullTxt[i];
                    m_ActivateLightbox = false;
                }
            }
            
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        //If gameobject with tag 
        if (other.gameObject.tag == "Slide")
        {
            m_ActivateLightbox = true;
        }

    }
}
