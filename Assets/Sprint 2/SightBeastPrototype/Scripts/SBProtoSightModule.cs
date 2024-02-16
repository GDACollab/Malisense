using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;
using Unity.VisualScripting;

public class SBProtoSightModule : MonoBehaviour
{
    [Tooltip("Radius that this creature can see.")]
    public float visionRadius;

    [Tooltip("Initial vision cone angle size")]
    [Range(0, 360)]
    public float visionArcSize;
    [Range(0, 360)]
    public float visionAngle;


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
        foreach (var sc in shadowCasters)
        {
            var shadowPath = sc.shapePath;

            for (int i = 0; i < shadowPath.Length; i++)
            {
                var shadowStart = sc.transform.TransformPoint(shadowPath[i]);
                var shadowEnd = sc.transform.TransformPoint(shadowPath[(i + 1) % shadowPath.Length]);

                if (FasterLineSegmentIntersection(castStart, castEnd, shadowStart, shadowEnd))
                    return false;
            }
        }

        return true;
    }

    public void LookAt(Vector3 lookTarget)
    {
        Vector3 dir = (lookTarget - transform.position).normalized;
        float angleToTarget = Vector2.SignedAngle(transform.right, dir);
        visionAngle = (angleToTarget + 360) % 360;
    }

    public Visibility GetVisibility(Vector2 position, float radius)
    {
        // Exit if player exceeds view distance
        if (Vector2.Distance(transform.position, position) > visionRadius) return Visibility.None;

        // Exit if player is not within view range
        Vector3 dir = DirFromAngle(visionAngle);
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angle = Vector2.Angle(dir, dirToTarget);
        //Debug.Log(Time.time + ": Angle:" + angle + "Target: " + visionArcSize/2);
        if (angle > (visionArcSize / 2)) return Visibility.None;

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

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

#if UNITY_EDITOR
    // Depict vision cone
    private void OnDrawGizmos()
    {
        Handles.color = new Color(128, 00, 255);
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.right, 360, visionRadius);

        // Draw boundry lines
        Handles.DrawLine(transform.position, transform.position + DirFromAngle(-visionArcSize / 2 + visionAngle) * visionRadius);
        Handles.DrawLine(transform.position, transform.position + DirFromAngle(visionArcSize / 2 + visionAngle) * visionRadius);
    }
#endif
}
