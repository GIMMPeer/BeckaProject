using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolographicPoster : MonoBehaviour {

    public Transform m_Player;
    public float m_PowerVal = 2.0f;

    private Material m_PosterMaterial;
	// Use this for initialization
	void Start ()
    {
        m_PosterMaterial = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 posterToPlayerDir = m_Player.position - transform.position;
        float dotProduct = Vector3.Dot(-transform.forward, posterToPlayerDir.normalized);

        m_PosterMaterial.SetColor("_Color", new Color(1f, 1f, 1f, Mathf.Pow(dotProduct, m_PowerVal)));
    }
}
