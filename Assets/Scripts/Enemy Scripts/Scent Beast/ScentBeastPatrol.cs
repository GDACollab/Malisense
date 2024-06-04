using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;

public class ScentBeastPatrol : StateBaseClass
{
    [Header("Path Variables")]
    [SerializeField][Tooltip("The min and max amount of time to stay on a calculated path")] private Vector2 pathTimeRange = new Vector2(15f, 20f);
    [SerializeField] private float speed = 15f;
    [SerializeField] private bool enableGizmos = true;

    [Header("READ ONLY PLZ")]
    [SerializeField] private float pathTimeLeft = 10f;
    [SerializeField] private Vector2 endpoint;
    
    // public Vector2 actualEndpoint;
    Tilemap floorTilemap;
    EnemyPathfinder _pathfinder;
    
    override public void Init(){
        floorTilemap = FindObjectsOfType<Grid>().ToList().Find(x=>x.name=="Grid").GetComponentsInChildren<Tilemap>().ToList().Find(x=>x.name=="Floor");
        _pathfinder = GetComponent<EnemyPathfinder>();
        pathTimeLeft = Random.Range(pathTimeRange.x, pathTimeRange.y);
    }
    
    override public void On_Update(){
        if(_pathfinder.AtGoal || pathTimeLeft<=0f)
        {
            pathTimeLeft = Random.Range(pathTimeRange.x, pathTimeRange.y);
            endpoint = GetRandomPoint();
            _pathfinder.SetTarget(endpoint);
            _pathfinder.acceleration = speed;
        }
        pathTimeLeft -= Time.deltaTime;
    }
    
    /// <summary>
    /// Gets a VALID random point in the dungeon
    /// </summary>
    /// <returns>A random point</returns>
    public Vector2 GetRandomPoint()
    {
        floorTilemap.CompressBounds();
        List<Vector3Int> possiblePositions = new List<Vector3Int>();
        foreach(Vector3Int pos in floorTilemap.cellBounds.allPositionsWithin){
            if(AstarPath.active.data.gridGraph.GetNearest(pos).node.Walkable){possiblePositions.Add(pos);}
        }
    
        return (Vector3)possiblePositions[Random.Range(0, possiblePositions.Count)];
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
            
            Gizmos.color = col;
            Gizmos.matrix = mat;
        }
    }
#endif
}
