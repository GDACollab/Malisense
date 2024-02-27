using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flash : MonoBehaviour
{
    public float duration;
    public Color endColor = Color.white;
    public float exponent = 1f;

    private new Light2D light;
    private float outerRad;
    private float innerRad;
    private float intensity;
    private float lifetime;

    private void Start()
    {
        light = GetComponent<Light2D>();
        outerRad = light.pointLightOuterRadius;
        innerRad = light.pointLightInnerRadius;
        intensity = light.intensity;
        lifetime = duration;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;

        if(lifetime < 0f)
        {
            Destroy(gameObject);
            return;
        }

        float factor = Mathf.Pow(lifetime / duration, exponent);
        light.pointLightInnerRadius = innerRad * factor;
        light.pointLightOuterRadius = outerRad * factor;
        light.intensity = intensity * factor;
    }
}
