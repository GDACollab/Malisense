using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    Camera mainCamera;
    private GameObject player;
    private float shakeTimeLeft;
    private float shakeTotalTime;
    private float shakeMagnitude;
    
    void Start () 
    {
        // Get the camera
        mainCamera = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera.backgroundColor = Color.black;
    }
    
    private void FixedUpdate() {
        setAspectRatio(); // Move to start if game doesn't support windowed mode
    }
    
    private void LateUpdate() {
        if (!player) return;

        // Track player
        Vector3 pos = player.transform.position;
        pos.z = transform.position.z;

        // Do screen shake
        if (shakeTimeLeft > 0)
        {
            shakeTimeLeft -= Time.deltaTime;
            float intensity = shakeMagnitude * (shakeTimeLeft / shakeTotalTime);
            pos.x += Random.Range(-1f, 1f) * intensity;
            pos.y += Random.Range(-1f, 1f) * intensity;
        }

        transform.position = pos;
    }
    
    private void setAspectRatio(){
        // Desired Aspect Ratio
        float aspectRatio = 16.0f / 9.0f;

        // Get current aspect ratio
        float currentAspect = (float)Screen.width / (float)Screen.height;

        // Get the scale difference 
        float viewportHeight = currentAspect / aspectRatio;

        // If viewport height is less than current height, add letterbox
        if (viewportHeight != 1f){
            if (viewportHeight < 1.0f)
            {  
                Rect rect = mainCamera.rect;

                rect.width = 1.0f;
                rect.height = viewportHeight;
                rect.x = 0;
                rect.y = (1.0f - viewportHeight) / 2.0f;
                
                mainCamera.rect = rect;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / viewportHeight;

                Rect rect = mainCamera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                mainCamera.rect = rect;
            }
        }
    }

    public void ScreenShake(float duration, float magnitude)
    {
        if (shakeTimeLeft <= 0f || magnitude > shakeMagnitude)
        {
            shakeTimeLeft = duration;
            shakeTotalTime = duration;
            shakeMagnitude = magnitude;
        }
    }
}
