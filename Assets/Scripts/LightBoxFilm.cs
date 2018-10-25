using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBoxFilm : MonoBehaviour {

    public Texture m_MaskingTexture; //texture used
    public Color m_MaskingTint;
    public List<Light> m_AssociatedLights;

    //for lerping film to lightbox
    public Transform m_Destination;
    public float m_LerpSpeed = 3.0f;

    public AudioClip m_Clip;

    private bool m_IsLerping = false;

	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_IsLerping)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, m_Destination.position, Time.deltaTime * m_LerpSpeed);
            transform.position = newPosition;

            if (Vector3.Distance(transform.position, m_Destination.position) <= 0.1f)
            {
                //film in position on lightbox
                m_IsLerping = false;
                transform.position = m_Destination.position;
                
                if (m_Clip != null)
                {
                    GetComponent<AudioSource>().clip = m_Clip;
                    GetComponent<AudioSource>().Play();
                }
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        LightBox lightBox = other.gameObject.GetComponent<LightBox>();

        if (lightBox != null)
        {
            lightBox.SetEmission(m_MaskingTexture, m_MaskingTint, m_AssociatedLights);
            m_IsLerping = true;

            //force release of grabbed object
            OVRGrabbable oVRGrabbable = GetComponent<OVRGrabbable>();

            if (oVRGrabbable)
            {
                if (oVRGrabbable.grabbedBy)
                {
                    oVRGrabbable.grabbedBy.ForceRelease(oVRGrabbable);
                }
            }

            GetComponent<Rigidbody>().isKinematic = true;

            transform.rotation = Quaternion.identity;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        LightBox lightBox = other.gameObject.GetComponent<LightBox>();

        if (lightBox != null && m_IsLerping == false)
        {
            lightBox.ResetBox(m_AssociatedLights);
        }

    }
}
