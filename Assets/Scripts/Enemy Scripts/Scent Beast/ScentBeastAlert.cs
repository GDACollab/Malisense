using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

[RequireComponent(typeof(EnemyPathfinder))]
public class ScentBeastAlert : StateBaseClass
{
    [Header("Path Variables")]
    [SerializeField][Tooltip("The min and max radius to path around the player")] private float endPointRadius = 30f;
    [SerializeField][Tooltip("The maximum amount of time to stay on a calculated path")] private float maxPathTime = 10f;
    [Header("Turn on only during runtime")]
    [SerializeField] private bool enableGizmos = false;

    [Header("READ ONLY PLZ")]
    [SerializeField] private float pathTimeLeft = 10f;
    [SerializeField] private Vector2 endpoint;
    [SerializeField] private GameObject player;
    
    // public Vector2 actualEndpoint;
    Tilemap floorTilemap;
    EnemyPathfinder _pathfinder;
    
    override public void Init(){
        player = GameObject.FindWithTag("Player");
        floorTilemap = FindObjectOfType<Grid>().GetComponentsInChildren<Tilemap>().ToList().Find(x=>x.name=="Floor");
        _pathfinder = GetComponent<EnemyPathfinder>();
        pathTimeLeft = maxPathTime;
    }
    
    override public void On_Update(){
        if(_pathfinder.AtGoal || pathTimeLeft<=0f)
        {
            pathTimeLeft = maxPathTime;
            endpoint = GetRandomPoint();
            _pathfinder.SetTarget(endpoint);
        }
        pathTimeLeft -= Time.deltaTime;
    }
    
    /// <summary>
    /// Gets a VALID random point in the dungeon
    /// </summary>
    /// <returns>A random point</returns>
    public Vector2 GetRandomPoint()
    {
        Vector2 potentialEndpoint = Vector2.zero;
        bool validpoint = false;
        while (!validpoint)
        {
            potentialEndpoint = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))*endPointRadius + (Vector2)player.transform.position;
            var endNode = AstarPath.active.data.gridGraph.GetNearest(potentialEndpoint).node;
            validpoint = endNode.Walkable && floorTilemap.HasTile(floorTilemap.WorldToCell(potentialEndpoint));
            // actualEndpoint = (Vector2)(Vector3)endNode.position;
        }
        
        return potentialEndpoint;
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
            Gizmos.DrawWireCube(player.transform.position, Vector3.one*endPointRadius*2);

            Gizmos.color = col;
            Gizmos.matrix = mat;
        }
    }
#endif

}
