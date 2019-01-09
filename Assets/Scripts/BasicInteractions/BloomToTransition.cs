using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PostProcessing;

public class BloomToTransition : MonoBehaviour {

    public UnityEvent m_OnBloomComplete;
    public PostProcessingProfile m_PP;
    public AnimationCurve m_LightCurve; //curve which defines the rate of change for bloom

    public float m_Duration = 2.0f;
    public float m_IntensityGoal = 100.0f;
    public float m_ThresholdGoal = 0f;

    public bool m_InverseBloom = false; //make bloom start bright and end normal

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
            float mappedTime = Map(timeDifference, 0, m_Duration, 0f, 1f); //map time difference to lerp scale

            float inversedTime = m_InverseBloom ? 1.0f - mappedTime : mappedTime; //if inverse then flip value

            float curveTime = m_LightCurve.Evaluate(inversedTime);

            float intensityVal = Map(curveTime, 0.0f, 1.0f, m_PreviousIntensity, m_IntensityGoal);
            float thresholdVal = Map(curveTime, 0.0f, 1.0f, m_PreviousThreshold, m_ThresholdGoal);

            m_BloomSettings.bloom.intensity = intensityVal;
            m_BloomSettings.bloom.threshold = thresholdVal;

            m_PP.bloom.settings = m_BloomSettings; //apply bloom settings to profile

            if (mappedTime >= 0.99999f)
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
