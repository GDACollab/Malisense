using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

public class SightBeastLight : MonoBehaviour
{
    [SerializeField]
    private Light2D visualCone; // the cone of light coming from the sight beast
    [SerializeField]
    private Light2D baseGlow; // the glow light around the base of the cone
    [SerializeField]
    private ParticleSystem particleSys;
    private Color lightColor; // the current light Color
    private float lightIntensity; // the current light Intensity
    private float lightRadius; // the current light Radius

    [Header("Light Settings")]
    [Header("Wander Light Preset")]
    public float wanderIntensity = 2;
    public Color wanderColor;
    public float wanderRadius = 9;
    public bool debugSETLIGHTWANDER = false;
    [Header("Patrol Light Preset")]
    public float patrolIntensity = 2;
    public Color patrolColor;
    public float patrolRadius = 9;
    public bool debugSETLIGHTPATROL = false;
    [Header("Chase Light Preset")]
    public float chaseIntensity = 2;
    public Color chaseColor;
    public float chaseRadius = 9;
    public bool debugSETLIGHTCHASE = false;

    // LIGHT SET FUNCTIONS
    // call these to change the current light state to the presets
    public void setLightWander() {
        lightIntensity = wanderIntensity;
        lightColor = wanderColor;
        lightRadius = wanderRadius;
        updateLight();
    }
    public void setLightPatrol() {
        lightIntensity = patrolIntensity;
        lightColor = patrolColor;
        lightRadius = patrolRadius;
        updateLight();
    }
    public void setLightChase() {
        lightIntensity = chaseIntensity;
        lightColor = chaseColor;
        lightRadius = chaseRadius;
        updateLight();
    }

    private void Start() {
        // start light in Wander State
        setLightWander();
    }

    private void FixedUpdate() {
        
        // Debug setup to manually change the light during gameplay with the editor
        if (debugSETLIGHTWANDER) {
            setLightWander();
            debugSETLIGHTWANDER = false;
        } else if (debugSETLIGHTPATROL) {
            setLightPatrol();
            debugSETLIGHTPATROL = false;
        } else if (debugSETLIGHTCHASE) {
            setLightChase();
            debugSETLIGHTCHASE = false;
        }

    }


    // updates all the systems connected to the light values
    // called every time the preset is changed
    private void updateLight() {
        // update the particle system
        var pShape = particleSys.shape;
        pShape.radius = lightRadius;
        var pMain = particleSys.main;
        pMain.startColor = new Color(lightColor.r, lightColor.g, lightColor.b, 0.5f);
        particleSys.GetComponent<CircleCollider2D>().radius = lightRadius;

        // update the cone of light
        visualCone.pointLightOuterRadius = lightRadius * 1.1f;
        visualCone.pointLightInnerRadius = lightRadius;
        visualCone.color = lightColor;
        visualCone.intensity = lightIntensity * 4;

        // update the base glow
        baseGlow.color = lightColor;
        baseGlow.intensity = lightIntensity;
    }


}
