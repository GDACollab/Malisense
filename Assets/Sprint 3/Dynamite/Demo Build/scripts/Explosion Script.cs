using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class ExplosionScript : MonoBehaviour
{
    private SmartCamera cam;
    public float screenShakeDuration;
    public float magnitude;
    [Tooltip("How long the explosion prefab will last")]
    [SerializeField] public float explosionDuration;
    public scr_noise noise;
    public float noise_level;

    private float explosionTime = 0f;

    private void Start()
    {
        cam = Camera.main.GetComponent<SmartCamera>();
        noise.MakeSound(transform.position, noise_level);
        ScreenShake();
    }

    private void Update()
    {
        explosionTime += Time.deltaTime;
        if (explosionTime >= explosionDuration) { 
            Destroy(gameObject);
        }
    }

    public void ScreenShake() {
        cam.screenShakeTime = screenShakeDuration;
        cam.magnitude = magnitude;
        //Debug.Log("dynamite spawned");
    }
}
