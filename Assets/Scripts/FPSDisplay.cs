using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public float avgFrameRate;
    public Text[] display_Text;

    public void Update()
    {
        for (int i = 0; i < display_Text.Length; i++)
        {
            float current = 0;
            current = 1.0f / Time.deltaTime;
            avgFrameRate += current * 0.01f;
            avgFrameRate *= 0.99f;
            avgFrameRate = Mathf.RoundToInt(avgFrameRate);
            display_Text[i].text = avgFrameRate.ToString() + " FPS";
        }
        
    }
}