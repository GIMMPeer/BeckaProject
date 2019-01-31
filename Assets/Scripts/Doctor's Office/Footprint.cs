using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//footprint prefab that is spawned, faded in, faded out, then destroys itself
[RequireComponent(typeof(AudioSource))]
public class Footprint : MonoBehaviour {

    public float m_TimeToLive = 3.0f;
    public AnimationCurve m_BrightnessCurve; //animation curve for fade in/out of footprint

    public AudioClip m_RightFootSound;
    public AudioClip m_LeftFootSound;

    private float m_StartingTime;
    private float m_TimeOfDeath;
	// Use this for initialization
	void Start ()
    {
        m_StartingTime = Time.time;
        m_TimeOfDeath = Time.time + m_TimeToLive;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float alphaTime = Map(Time.time, m_StartingTime, m_TimeOfDeath, 0, 1.0f);

        GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1, m_BrightnessCurve.Evaluate(alphaTime)));

        if (Time.time - m_TimeOfDeath > 0)
        {
            //if time is past time of death destory footprint object
            Destroy(gameObject);
        }
	}

    private float Map(float value, float startMin, float startMax, float endMin, float endMax)
    {
        float diff = (value - startMin) / (startMax - startMin);

        float newValue = (endMin * (1 - diff)) + (endMax * diff);

        return newValue;
    }

    //Sets footedness of footprint and plays audio
    public void SetFootAudio(bool isLeftFoot)
    {
        if (isLeftFoot)
        {
            GetComponent<AudioSource>().clip = m_LeftFootSound;
        }
        else
        {
            GetComponent<AudioSource>().clip = m_RightFootSound;
        }

        GetComponent<AudioSource>().Play();
    }
}
