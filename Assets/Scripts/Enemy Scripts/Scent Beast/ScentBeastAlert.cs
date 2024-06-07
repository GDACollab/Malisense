using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;
using FMOD;

[RequireComponent(typeof(EnemyPathfinder))]
public class ScentBeastAlert : StateBaseClass
{
    [Header("Path Variables")]
    [SerializeField][Tooltip("The min and max radius to path around the player")] private Vector2 endPointRadius = new Vector2(20f, 40f);
    [SerializeField][Tooltip("The maximum amount of time to stay on a calculated path")] private float maxPathTime = 10f;
    [SerializeField][Tooltip("The min and max speed the scent beast can be at")] private Vector2 speedRange = new Vector2(20f, 30f);
    [Header("Turn on only during runtime")]
    [SerializeField] private bool enableGizmos = false;


    [Header("READ ONLY PLZ")]
    [SerializeField] private float currSpeed = 20f;
    [SerializeField] private float currRadius = 40f;
    [SerializeField] private float pathTimeLeft = 10f;
    [SerializeField] private Vector2 endpoint;
    [SerializeField] private GameObject player;
    
    // public Vector2 actualEndpoint;
    Tilemap floorTilemap;
    EnemyPathfinder _pathfinder;
    ScentDetection detection;

    private Animator animator;

    override public void Init(){
        player = GameObject.FindWithTag("Player");
        floorTilemap = FindObjectsOfType<Grid>().ToList().Find(x=>x.name=="Grid").GetComponentsInChildren<Tilemap>().ToList().Find(x=>x.name=="Floor");        _pathfinder = GetComponent<EnemyPathfinder>();
        pathTimeLeft = maxPathTime;
        detection = GetComponent<ScentDetection>();
        animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Alert");
    }
    
    override public void On_Update(){
        if(_pathfinder.AtGoal || pathTimeLeft<=0f)
        {
            pathTimeLeft = maxPathTime;
            endpoint = GetRandomPoint();
            _pathfinder.SetTarget(endpoint);
            _pathfinder.acceleration = currSpeed;
        }
        pathTimeLeft -= Time.deltaTime;
        
        currSpeed = LerpViaScent(speedRange);
        _pathfinder.acceleration = currSpeed;
    }
    
    /// <summary>
    /// Gets a VALID random point in the dungeon nearby the player
    /// </summary>
    /// <returns>A random point</returns>
    private Vector2 GetRandomPoint()
    {
        Vector2 potentialEndpoint = Vector2.zero;
        bool validpoint = false;
        while (!validpoint)
        {
            currRadius = LerpViaScent(endPointRadius.y, endPointRadius.x);
            potentialEndpoint = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))*currRadius + (Vector2)player.transform.position;
            var endNode = AstarPath.active.data.gridGraph.GetNearest(potentialEndpoint).node;
            validpoint = endNode.Walkable && floorTilemap.HasTile(floorTilemap.WorldToCell(potentialEndpoint));
            // actualEndpoint = (Vector2)(Vector3)endNode.position;
        }
        
        return potentialEndpoint;
    }
    
    /// <summary>
    /// Returns a lerped value based on the player's scent
    /// </summary>
    /// <param name="range">The range to lerp</param>
    /// <returns>A lerped value</returns>
    private float LerpViaScent(Vector2 range){
        return Mathf.Lerp(range.x, range.y, 1-((detection.scentT_Alert2Chase-detection.GetScent())/(detection.scentT_Alert2Chase-detection.scentT_Patrol2Alert)));
    }
    
    private float LerpViaScent(float min, float max){
        return Mathf.Lerp(min, max, 1-((detection.scentT_Alert2Chase-detection.GetScent())/(detection.scentT_Alert2Chase-detection.scentT_Patrol2Alert)));
    }
    
#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if(enableGizmos){
            var col = Gizmos.color;
            var mat = Gizmos.matrix;
            // Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endpoint, 0.5f);
            
            // Gizmos.color = Color.green;
            // Gizmos.DrawWireSphere(actualEndpoint, 0.5f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(player.transform.position, Vector3.one*currRadius*2);

            Gizmos.color = col;
            Gizmos.matrix = mat;
        }
    }
#endif

}
