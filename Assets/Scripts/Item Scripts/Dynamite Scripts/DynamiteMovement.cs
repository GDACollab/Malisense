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

    [Tooltip("Set the distance the dynamite is thrown from the player.")]
    public float distance;

    [Tooltip("The minimum distance the dynamite can land away from a wall.")]
    public float radius = 0.25f;

    [Tooltip("Speed in which dynamite spins in the air")]
    public float rotationSpeed;

    public GameObject ExplosionPrefab;
    // moves the dynamite, uses the dynamite curve script to calculate curve
    // destroys dynamite once it reaches destination
    private IEnumerator Move()
    {
        yield return StartCoroutine(arcMovement.Curve(transform.position, target));
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(ExplosionPrefab, target, Quaternion.identity);
        StartCoroutine(MultiExplosion());
    }

    // set up to call move(), moves dynamite in direction of player's triangle
    private void Start()
    {
        directionalTriangle = GameObject.FindGameObjectWithTag("Player").transform.Find("Circle");
        arcMovement = GetComponent<DynamiteCurveScript>();

        Vector3 direction = directionalTriangle.up;

        // TODO: Physics2D.GetLayerCollisionMask(gameObject.layer) would work better than hardcoding GetMask here, but merging tags is hard
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction, distance, LayerMask.GetMask("Dungeon"));
        target = hit.transform ? hit.centroid : transform.position + direction * distance; ;

        // rotation setup
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        if (directionalTriangle.position.x > playerPosition.position.x) { rotationSpeed = -rotationSpeed; }

        StartCoroutine(Move());
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
    
    IEnumerator MultiExplosion(){
        yield return new WaitForSeconds(0.1f);
        Instantiate(ExplosionPrefab, target+(Vector3)Random.insideUnitCircle, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Instantiate(ExplosionPrefab, target+(Vector3)Random.insideUnitCircle, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Instantiate(ExplosionPrefab, target+(Vector3)Random.insideUnitCircle, Quaternion.identity);
        Destroy(gameObject);
    }
}
