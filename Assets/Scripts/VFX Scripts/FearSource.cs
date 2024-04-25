using UnityEditor;
using UnityEngine;

/// <summary>
/// Triggers fear effects when a <see cref="FearTracker"/> is in view.
/// </summary>
/// <remarks>
/// This should be removed in favor of monsters calling <see cref="FearTracker.AddFear(float)"/> directly.
/// </remarks>
public class FearSource : MonoBehaviour
{
    public FearTracker target;

    [Tooltip("Maximum intensity of this fear source, from 0 to 1.")]
    [Range(0f, 1f)]
    public float intensity = 0.75f;

    [Tooltip("Radius at which fear will start.")]
    public float radius = 10f;

    [Tooltip("Radius at which fear will be at its maximum.")]
    public float innerRadius = 3f;

    [Tooltip("Indicates the layers that count as blocking line of sight.")]
    public LayerMask wallLayers = -1;

    private void Start() {
        target = GameObject.FindWithTag("Player").GetComponent<FearTracker>();
    }

    private bool CanSee(Vector2 point)
    {
        var hit = Physics2D.Raycast(
            origin: transform.position,
            direction: (point - (Vector2)transform.position).normalized,
            distance: Vector2.Distance(point, transform.position),
            layerMask: wallLayers);

        return hit.transform == null;
    }

    private void Update()
    {
        if(target
            && Vector2.Distance(target.transform.position, transform.position) < radius
            && CanSee(target.transform.position))
        {
            target.AddFear(Mathf.InverseLerp(radius, innerRadius, Vector2.Distance(target.transform.position, transform.position)) * intensity);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1f, 0.5f, 0.5f);
        Handles.CircleHandleCap(0, transform.position, transform.rotation, radius, EventType.Repaint);
        Handles.color = Color.red;
        Handles.CircleHandleCap(0, transform.position, transform.rotation, innerRadius, EventType.Repaint);
    }
#endif
}
