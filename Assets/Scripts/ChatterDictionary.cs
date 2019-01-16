using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatterDictionary : MonoBehaviour {

    public Texture2D[] m_AllWords;
    public GameObject m_WordPrefab;
    public float m_TextDistanceMultiplier;

    public int m_TotalWordCount = 1;
	// Use this for initialization
	void Start ()
    {
		for (int i = 0; i < m_TotalWordCount; i++)
        {
            //offset is based on rotation of holder, always through local forward and up vectors
            //Vector3 offset = (transform.forward * Random.Range(-0.75f, 0.75f)) + (transform.up * Random.Range(-0.5f, 0.5f));
            Vector3 offset = (transform.forward * Random.Range(-m_WordPrefab.transform.localScale.x, m_WordPrefab.transform.localScale.x) * m_TextDistanceMultiplier) + (transform.up * Random.Range(-m_WordPrefab.transform.localScale.y, m_WordPrefab.transform.localScale.y)) * m_TextDistanceMultiplier * 1.2f;

            GameObject word = Instantiate(m_WordPrefab);
            word.transform.parent = transform;

            word.transform.localPosition = Vector3.zero + offset;

            //for text to stay on wall, set starting rotation to use as base
            word.GetComponent<FreezeRotationLate>().SetStartingRotation(word.transform.localRotation);

            //set texture of writing from all indexes of words
            word.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_AllWords[Random.Range(0, m_AllWords.Length)]);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
