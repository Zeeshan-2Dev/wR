using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayAndNightCycles : MonoBehaviour
{
    [Header("Time Settings")]
    [Range(0f, 24f)]
    public float currentTime;
    public float timeSpeed = 1f;

    [Header("Current Time")]
    public string currentTimeString;

    [Header("Light Settings")]
    public Light sunLight;
    public float sunPosition = 1f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve lightTemperatureCurve;

    public bool isDay = true;
    
    void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
    }

    
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime >= 24)
        {
            currentTime = 0;
        }

        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ":" + ((currentTime % 1) * 60).ToString("00");

    }

    void UpdateLight()
    {
        float sunRotation = currentTime / 24 * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, sunPosition, 0f);

        float normalizedTime = currentTime / 24f;
        float InternsityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);

        //HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();

       /* if (sunLightData != null)
        {
            sunLightData.intensity = InternsityCurve * sunIntensity;
        }

        float temperatureMultiplier = lightTemperatureCurve.Evaluate(normalizedTime);
        Light lightComponent = sunLight.GetComponent<Light>();

        if (lightComponent != null)
        {
            lightComponent.colorTemperature = temperatureMultiplier * 10000f;
        }*/
    }

    void CheckShadowStatus()
    {
        //HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        float currentSunRotation = currentTime;

    }
}
