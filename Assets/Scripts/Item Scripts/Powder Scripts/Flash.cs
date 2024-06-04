using System;
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

    private Light2D light;
    private float outerRad;
    private float innerRad;
    private float intensity;
    private float lifetime;
    
    private CircleCollider2D circleCollider;

    private void Start()
    {
        light = GetComponent<Light2D>();
        outerRad = light.pointLightOuterRadius;
        innerRad = light.pointLightInnerRadius;
        intensity = light.intensity;
        lifetime = duration;
        circleCollider = GetComponent<CircleCollider2D>();
        GameObject.Find("Global Teapot").GetComponent<AudioManager>().PlayFlashDustSFX();
        DistractSight();
    }

    void Update()
    {
        lifetime -= Time.deltaTime;

        if(lifetime < 0f)
        {
            DistractSight();
            Destroy(gameObject);
            return;
        }

        float factor = Mathf.Pow(lifetime / duration, exponent);
        light.pointLightInnerRadius = innerRad * factor;
        light.pointLightOuterRadius = outerRad * factor;
        light.intensity = intensity * factor;
    }
    
    private void DistractSight(){
        circleCollider.radius = outerRad;
        Collider2D[] touchList = new Collider2D[32];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(~circleCollider.excludeLayers);
        filter.useTriggers = true;
        circleCollider.OverlapCollider(filter, touchList);
        foreach(var touched in touchList){
            SightBeastSightModule _sight;
            if(touched && touched.TryGetComponent(out _sight) && _sight.CanSee(transform.position, circleCollider.radius)){
                _sight.GetComponent<StateMachine>().currentState = StateMachine.State.Distracted;
            }
        }
    }
}
