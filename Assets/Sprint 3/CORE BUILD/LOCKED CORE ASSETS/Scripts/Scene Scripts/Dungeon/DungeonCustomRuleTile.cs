using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class DungeonCustomRuleTile : RuleTile<DungeonCustomRuleTile.Neighbor>
{
    public TileBase wall;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Wall = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.Wall: return CheckWall(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    public bool CheckWall(TileBase tile)
    {
        return tile == wall || tile == null;
    }
}