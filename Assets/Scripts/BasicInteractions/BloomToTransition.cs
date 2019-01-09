using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PostProcessing;

public class BloomToTransition : MonoBehaviour {

    public UnityEvent m_OnBloomComplete;
    public PostProcessingProfile m_PP;

    public float m_Duration = 2.0f;
    public float m_IntensityGoal = 100.0f;
    public float m_ThresholdGoal = 0f;

    private BloomModel.Settings m_BloomSettings;

    private float m_TimeStarted;
    private float m_PreviousThreshold;
    private float m_PreviousIntensity;

    private bool m_IsLerping = false;

    private void Start()
    {
        m_BloomSettings = m_PP.bloom.settings;
    }

    private void Update()
    {
        if (m_IsLerping)
        {
            float timeDifference = Time.time - m_TimeStarted;
            float lerpVal = Map(timeDifference, 0, m_Duration, 0f, 1f); //map time difference to lerp scale

            float intensityVal = Mathf.Lerp(m_PreviousIntensity, m_IntensityGoal, lerpVal); //lerp is clamped so it can never go above goal
            float thresholdVal = Mathf.Lerp(m_PreviousIntensity, m_ThresholdGoal, lerpVal);

            m_BloomSettings.bloom.intensity = intensityVal;
            m_BloomSettings.bloom.threshold = thresholdVal;

            m_PP.bloom.settings = m_BloomSettings; //apply bloom settings to profile

            if (lerpVal >= 0.99999f)
            {
                //transition is done
                EndTransition();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartTransition();
        }
    }

    public void StartTransition()
    {
        m_PreviousThreshold = m_BloomSettings.bloom.threshold;
        m_PreviousIntensity = m_BloomSettings.bloom.intensity;

        m_TimeStarted = Time.time;

        m_IsLerping = true;
    }

    private void EndTransition()
    {
        m_IsLerping = false;
        m_OnBloomComplete.Invoke();
    }

    private void OnDestroy()
    {
        m_BloomSettings.bloom.intensity = m_PreviousIntensity;
        m_BloomSettings.bloom.threshold = m_PreviousThreshold;

        m_PP.bloom.settings = m_BloomSettings;
    }

    private float Map(float value, float startMin, float startMax, float endMin, float endMax)
    {
        float diff = (value - startMin) / (startMax - startMin);

        float newValue = (endMin * (1 - diff)) + (endMax * diff);

        return newValue;
    }
}
