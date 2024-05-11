using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;

[RequireComponent(typeof(EnemyPathfinder))]
public class ScentBeastChase : StateBaseClass
{
    //[Header("Path Variables")]
    //[SerializeField][Tooltip("The min and max radius to path around the player")] private float endPointRadius = 30f;
    //[SerializeField][Tooltip("The maximum amount of time to stay on a calculated path")] private float maxPathTime = 10f;
    //[Header("Turn on only during runtime")]
    [SerializeField] private bool enableGizmos = false;
    [SerializeField] private int ChaseSpeed;

    //[Header("READ ONLY PLZ")]
    [SerializeField] private Vector2 endpoint;
    [SerializeField] private GameObject player;
    [SerializeField] private int endPointRadius;

    EnemyPathfinder _pathfinder;


    override public void Init()
    {
        player = GameObject.FindWithTag("Player");
        _pathfinder = GetComponent<EnemyPathfinder>();
        _pathfinder.acceleration = ChaseSpeed;
    }

    override public void On_Update()
    {
        endpoint = player.transform.position;
        _pathfinder.SetTarget(endpoint);
    }


#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (enableGizmos)
        {
            var col = Gizmos.color;
            var mat = Gizmos.matrix;
            // Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endpoint, 0.5f);

            // Gizmos.color = Color.green;
            // Gizmos.DrawWireSphere(actualEndpoint, 0.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(player.transform.position, Vector3.one * 2);

            Gizmos.color = col;
            Gizmos.matrix = mat;
        }
    }
#endif

}
