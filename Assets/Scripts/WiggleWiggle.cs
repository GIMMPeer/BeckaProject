using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleWiggle : MonoBehaviour {
    
    public float speed; 
    public float wiggleLength;
    public float animLength;

    private Quaternion m_OriginalRot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) //KEYCODE ONLY USED FOR TESTING
        {
            m_OriginalRot = this.transform.rotation;
            StartCoroutine(StartWiggle());
        }
    } 
    
    IEnumerator StartWiggle()
    {
        float timePassed = Time.time;
        while (Time.time - timePassed < animLength)
        {
            Vector3 pos = new Vector3(Mathf.PingPong(Time.time * speed / 2, wiggleLength), transform.localRotation.y, transform.localRotation.z);
            this.transform.localRotation = Quaternion.Euler(pos);

            yield return null;
        }

        transform.rotation = m_OriginalRot;
    }
}
