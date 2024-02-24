using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    //public Camera cameraToShake;
    public float time;
    public float magnitude;
    [Tooltip("How long the explosion prefab will last")]
    [SerializeField] public float explosionDuration;

    private float explosionTime = 0f;

    private void Update()
    {
        explosionTime += Time.deltaTime;
        if (explosionTime >= explosionDuration) { 
            Destroy(gameObject);
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>().ShakeCamera(time, magnitude));
    }
}
