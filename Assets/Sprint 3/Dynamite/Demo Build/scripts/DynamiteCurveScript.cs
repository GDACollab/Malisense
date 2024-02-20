using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteCurveScript : MonoBehaviour
{
    public AnimationCurve curve;

    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float heightY = 3.0f;

    public IEnumerable Curve(Vector3 start, Vector2 target)
    {
        float timePassed = 0f;

        Vector2 end = target;

        while (timePassed < duration)
        { 
            timePassed += Time.deltaTime;

            float linearT = timePassed / duration;
            float heightT = curve.Evaluate(linearT); // value from animation curve
            
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        
        }
    
    }
}
