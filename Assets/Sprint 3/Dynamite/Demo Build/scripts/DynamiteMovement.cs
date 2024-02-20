using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// https://github.com/Tycro-Games/Parabolic-demo/blob/main/Parabolic%20Trajectory/Assets/Scripts/tower/Projectiles/ProjectileArcTarget.cs
public class DynamiteMovement : MonoBehaviour
{

    private Vector2 mousePosition;
    private Camera mainCamera;
    private DynamiteCurveScript arcMovement = null;
    private Vector3 target;
    private IEnumerator Move()
    {
        yield return StartCoroutine(arcMovement.Curve(transform.position, target));
        Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        arcMovement = GetComponent<DynamiteCurveScript>();
        target = mousePosition;

        StartCoroutine(Move());
    }
}
