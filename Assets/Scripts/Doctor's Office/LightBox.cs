using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

//TODO make a function for post processing

public class LightBox : MonoBehaviour {

    public Material m_LightBoxMaterial;

    public PostProcessingProfile m_Profile;
    public float m_DesaturateValue = 0.0f;
    public float m_SaturationValue = 1.695f;
    public float m_LerpSpeed = .5f;

    private Texture m_DefaultTexture;
    private Color m_DefaultColor;

    private float m_DefaultProfileSaturation;

    private bool m_IsLerping;
    // Use this for initialization
    void Start ()
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;

        m_DefaultTexture = m_LightBoxMaterial.GetTexture("_EmissionMap");
        m_DefaultColor = m_LightBoxMaterial.GetColor("_EmissionColor");

        ColorGradingModel.Settings basicSettings = m_Profile.colorGrading.settings;
        basicSettings.basic.saturation = m_DesaturateValue;

        m_Profile.colorGrading.settings = basicSettings;
    }

    private void Update()
    {
        if (m_IsLerping)
        {
            ColorGradingModel.Settings basicSettings = m_Profile.colorGrading.settings;
            float newSaturation = Mathf.Lerp(basicSettings.basic.saturation, m_SaturationValue, Time.deltaTime * m_LerpSpeed);

            SetPostProcessingSaturation(newSaturation);

            if (m_SaturationValue - m_Profile.colorGrading.settings.basic.saturation <= 0.1f)
            {
                //lerp is complete
                basicSettings.basic.saturation = m_SaturationValue;
                m_Profile.colorGrading.settings = basicSettings;
                m_IsLerping = false;
            }
        }
    }

    public void SetEmission(Texture texture, Color color, List<Light> lights)
    {
        m_LightBoxMaterial.SetTexture("_EmissionMap", texture);
        m_LightBoxMaterial.SetColor("_EmissionColor", color);

        foreach (Light l in lights)
        {
            l.gameObject.SetActive(true); //temporary will eventually lerp up to brightness
        }

        m_IsLerping = true;
    }

    //TODO remove the need to send in lights list to reset lightbox
    public void ResetBox(List<Light> lights)
    {
        m_LightBoxMaterial.SetTexture("_EmissionMap", m_DefaultTexture);
        m_LightBoxMaterial.SetColor("_EmissionColor", m_DefaultColor);

        //turn off associated lights from film
        foreach (Light l in lights)
        {
            l.gameObject.SetActive(false); //temporary will eventually lerp up to brightness
        }

        SetPostProcessingSaturation(m_DefaultProfileSaturation);

    }

    private void OnApplicationQuit()
    {
        m_LightBoxMaterial.SetTexture("_EmissionMap", m_DefaultTexture);
        m_LightBoxMaterial.SetColor("_EmissionColor", m_DefaultColor);
    }

    private void SetPostProcessingSaturation(float satValue)
    {
        ColorGradingModel.Settings basicSettings = m_Profile.colorGrading.settings;
        basicSettings.basic.saturation = satValue;
        m_Profile.colorGrading.settings = basicSettings;
    }
}
