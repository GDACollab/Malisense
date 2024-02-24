using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Jim Lee
// https://www.gamedeveloper.com/business/different-ways-of-shaking-camera-in-unity
// NOTE: DOESN'T ACTUALLY DO ANYTHING WITH SMART CAMERA ACTIVE
public class CameraShakeScript : MonoBehaviour
{
    public IEnumerator ShakeCamera(float duration, float magnitude) 
    {
        Vector3 originalPosition = transform.localPosition;
        float time = 0f;


        while (time < duration) 
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
