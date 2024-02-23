using System.Collections;
using UnityEngine;

// Jim Lee


[RequireComponent(typeof(DynamiteCurveScript))]
public class DynamiteMovement : MonoBehaviour
{
    
    private Transform directionalTriangle;
    private Transform playerPosition;
    private DynamiteCurveScript arcMovement = null;
    private Vector3 target;

    [Tooltip("Set the distance the dynamite is thrown. Multiplicative, setting it to 1 will throw dynamite on the player's triangle.")]
    [SerializeField] public float distance;

    [Tooltip("Speed in which dynamite spins in the air")]
    [SerializeField] public float rotationSpeed;

    [SerializeField] public GameObject ExplosionPrefab;
    // moves the dynamite, uses the dynamite curve script to calculate curve
    // destroys dynamite once it reaches destination
    private IEnumerator Move()
    {
        yield return StartCoroutine(arcMovement.Curve(transform.position, target));
        Destroy(gameObject);
        Instantiate(ExplosionPrefab, target, Quaternion.identity);
    }

    // set up to call move(), moves dynamite in direction of player's triangle
    private void Start()
    {
        directionalTriangle = GameObject.FindGameObjectWithTag("Player").transform.Find("Circle").Find("Triangle");
        arcMovement = GetComponent<DynamiteCurveScript>();
        Vector3 direction = -(transform.position - directionalTriangle.position).normalized;
        target = transform.position + (direction * distance);

        // rotation setup
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        if (directionalTriangle.position.x > playerPosition.position.x) { rotationSpeed = -rotationSpeed; }

        StartCoroutine(Move());
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}
