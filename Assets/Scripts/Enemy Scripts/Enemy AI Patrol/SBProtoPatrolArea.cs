using UnityEngine;

public class SBProtoPatrolArea : MonoBehaviour
{
    public Vector2 size = new Vector2(2f, 2f);
    public float weight = 1f;

    public bool Contains(Vector2 position)
    {
        var pos = transform.InverseTransformPoint(position);
        return pos.x > size.x * -0.5f
            && pos.x < size.x * 0.5f
            && pos.y > size.y * -0.5f
            && pos.y < size.y * 0.5f;
    }

    public Vector2 GetRandomPoint()
    {
        return transform.TransformPoint(new Vector2(Random.value - 0.5f, Random.value - 0.5f) * size);
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        var col = Gizmos.color;
        var mat = Gizmos.matrix;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.color = col;
        Gizmos.matrix = mat;
    }
#endif
}
