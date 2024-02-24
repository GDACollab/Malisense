using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    Camera mainCamera;
    private GameObject player;
    public float screenShakeTime;
    public float magnitude;
    
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
        if(player!=null){
            mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
        }
        screenShakeTime -= Time.deltaTime;
        if (screenShakeTime > 0) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            mainCamera.transform.position += new Vector3(x, y, 0f);
        }
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
}
