using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//footprint prefab that is spawned, faded in, faded out, then destroys itself
public class Footprint : MonoBehaviour {

    public float m_TimeToLive = 1.5f;
    public AnimationCurve m_BrightnessCurve; //animation curve for fade in/out of footprint

    private float m_TextureAlpha = 0;
	// Use this for initialization
	void Start ()
    {
        m_BrightnessCurve.AddKey(0f, 0f);
        m_BrightnessCurve.AddKey(.1f, 1.0f);
        m_BrightnessCurve.AddKey(.7f, 1.0f);
        m_BrightnessCurve.AddKey(1f, 0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float time = Mathf.PingPong(Time.time, m_TimeToLive);
        m_TextureAlpha = Map(time, 0, m_TimeToLive, 0, 1.0f);

        GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1, m_BrightnessCurve.Evaluate(m_TextureAlpha)));
	}

    private float Map(float value, float startMin, float startMax, float endMin, float endMax)
    {
        float diff = (value - startMin) / (startMax - startMin);

        float newValue = (endMin * (1 - diff)) + (endMax * diff);

        return newValue;
    }
}
