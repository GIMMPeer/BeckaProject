using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAndOutInteraction : MonoBehaviour {

    public float Speed;
    public float SizeDivisor;

    private Vector3 m_OrignalScale;
    private bool shrinkDone = false;

	// Use this for initialization
	void Start () {

        m_OrignalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)) //KEYCODE ONLY USED FOR TESTING
        {
            StartCoroutine(Anim(Speed));
        }
    }

    IEnumerator Anim(float time)
    {
        Vector3 smallScale = m_OrignalScale / SizeDivisor;
        time = 1 / time;
        float startTime = time;


        while (time > 0.0f)
        {
            Debug.Log("While Loop Time: " + time);
            time -= Time.deltaTime;

            Debug.Log("Shrink Done: " + shrinkDone);
            if ( time > 0.005f && !shrinkDone)
            {
                transform.localScale = Vector3.Lerp(smallScale, m_OrignalScale, time / startTime);
            }
            else if (time > 0.005f && shrinkDone)
            {
                transform.localScale = Vector3.Lerp(m_OrignalScale, smallScale, time / startTime);
            }
            else if (time < 0.005f && !shrinkDone)
            {
                shrinkDone = true;
                time = startTime;
            }
            else
            {
                shrinkDone = false;
                time = 0;
            }

            yield return null;
        }
    }
}
