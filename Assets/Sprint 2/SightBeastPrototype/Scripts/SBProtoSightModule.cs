using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SBProtoSightModule : MonoBehaviour
{
    [Tooltip("Radius that this creature can see.")]
    public float visionRadius;

    [Tooltip("A mask of layers that should block vision")]
    public LayerMask wallLayers;

    [Tooltip("The thing to chase.")]
    public Transform target;

    [Tooltip("The radius of the target. Higher values mean the sight beast can see further around walls.")]
    public float targetRadius;

    bool FasterLineSegmentIntersection(Vector2 fromA, Vector2 toA, Vector2 fromB, Vector2 toB)
    {
        Vector2 a = toA - fromA;
        Vector2 b = fromB - toB;
        Vector2 c = fromA - fromB;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float denominator = a.y * b.x - a.x * b.y;

        if (Mathf.Abs(denominator) < 0.00001f)
        {
            return false;
        }
        else if (denominator > 0f)
        {
            if (alphaNumerator < 0f || alphaNumerator > denominator || betaNumerator < 0f || betaNumerator > denominator)
            {
                return false;
            }
        }
        else if (alphaNumerator > 0f || alphaNumerator < denominator || betaNumerator > 0f || betaNumerator < denominator)
        {
            return false;
        }
        return true;
    }

    private bool CanSee(Vector2 castEnd, IEnumerable<ShadowCaster2D> shadowCasters)
    {
        Vector2 castStart = transform.position;

        // Test against all shadow casters
        foreach(var sc in shadowCasters)
        {
            var shadowPath = sc.shapePath;

            for(int i = 0; i < shadowPath.Length; i++)
            {
                var shadowStart = sc.transform.TransformPoint(shadowPath[i]);
                var shadowEnd = sc.transform.TransformPoint(shadowPath[(i + 1) % shadowPath.Length]);

                if (FasterLineSegmentIntersection(castStart, castEnd, shadowStart, shadowEnd))
                    return false;
            }
        }

        return true;
    }

    public Visibility GetVisibility(Vector2 position, float radius)
    {
        if (Vector2.Distance(transform.position, position) > visionRadius) return Visibility.None;

        var perpendicular = Vector2.Perpendicular(position - (Vector2)transform.position).normalized;
        int count = 0;

        var shadowCasters = FindObjectsOfType<ShadowCaster2D>();

        if (CanSee(position, shadowCasters)) count++;
        if (CanSee(position + perpendicular * radius, shadowCasters)) count++;
        if (CanSee(position - perpendicular * radius, shadowCasters)) count++;

        if (count >= 2) return Visibility.Full;
        else if (count >= 1) return Visibility.Partial;
        else return Visibility.None;
    }

    public Visibility GetTargetVisibility() => GetVisibility(target.position, targetRadius);

    public enum Visibility
    {
        Full,
        Partial,
        None
    }
}
