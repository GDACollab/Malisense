using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public SBProtoPatrolArea[] areas = new SBProtoPatrolArea[0];

    public SBProtoPatrolArea FindClosestArea(Vector2 position)
    {
        SBProtoPatrolArea closestArea = null;
        float closestDist = float.PositiveInfinity;

        foreach(var area in areas)
        {
            if (area == null) continue;

            var localPoint = area.transform.InverseTransformPoint(position);
            var localClosest = Vector2.Min(area.size / 2f, Vector2.Max(-area.size / 2f, localPoint));

            float dist = Vector2.Distance(
                area.transform.TransformPoint(localPoint),
                area.transform.TransformPoint(localClosest));

            if(dist < closestDist)
            {
                closestArea = area;
                closestDist = dist;
            }
        }

        return closestArea;
    }
}
