using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(CompositeShadowCaster2D))]
public class TilemapShadows : MonoBehaviour
{
    public float margin = 0.25f;
    public bool regenerateOnPlay = true;

    void Start()
    {
        if(regenerateOnPlay)
            Regenerate();
    }

    public void Regenerate()
    {
        if(TryGetComponent(out Tilemap tilemap))
            Regenerate(tilemap);
    }

    public void Clear()
    {
        var casters = transform.GetComponentsInChildren<ShadowCaster2D>();
        foreach (var caster in casters)
        {
            if (caster)
                DestroyImmediate(caster.gameObject);
        }
    }

    private void Regenerate(Tilemap tilemap)
    {
        // Delete old shadow casters
        Clear();

        if (tilemap == null) return;

        // Generate array of tiles that need shadow casters
        int width = tilemap.size.x;
        int height = tilemap.size.y;

        var mask = new bool[width * height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                mask[x + y * width] = tilemap.GetTile(tilemap.cellBounds.min + new Vector3Int(x, y, 0)) != null;
            }
        }

        // Don't shade the bottom wall tiles, since those are typically vertical surfaces that should be hit by light
        for (int y = height - 1; y >= 1; y--)
        {
            for (int x = 0; x < width; x++)
            {
                mask[x + y * width] &= mask[x + (y - 1) * width] || y == height - 1 || !mask[x + (y + 1) * width];
            }
        }

        // Add shadow casters
        foreach(var rect in GenerateCoverRects(mask, width, height, margin))
        {
            var obj = new GameObject($"Shadow {rect}", typeof(ShadowCaster2D));
            obj.GetComponent<ShadowCaster2D>().selfShadows = true;

            obj.transform.parent = transform;

            var scale = rect.size;
            scale.Scale(tilemap.cellSize);
            obj.transform.localScale = scale;

            var pos = rect.center;
            pos.Scale(tilemap.cellSize);
            pos += (Vector2)tilemap.localBounds.min;
            obj.transform.localPosition = pos;
        }
    }

    private static IEnumerable<Rect> GenerateCoverRects(bool[] tiles, int width, int height, float margin)
    {
        // Subdivide tiles into 9 sections:
        // 4 corners, 4 edges, 1 center
        int expWidth = width * 3;
        int expHeight = height * 3;
        bool[] expTiles = new bool[expWidth * expHeight];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                int src = x + y * width;
                int dst = 3 * x + 1 + (3 * y + 1) * expWidth;

                expTiles[dst] = tiles[src];

                if (expTiles[dst])
                {
                    // Cardinal connections
                    expTiles[dst - 1] = x > 0 && tiles[src - 1];
                    expTiles[dst + 1] = x < width - 1 && tiles[src + 1];
                    expTiles[dst - expWidth] = y > 0 && tiles[src - width];
                    expTiles[dst + expWidth] = y < height - 1 && tiles[src + width];

                    // Diagonal connections
                    expTiles[dst + 1 + expWidth] = expTiles[dst + 1] && expTiles[dst + expWidth] && tiles[src + 1 + width];
                    expTiles[dst - 1 + expWidth] = expTiles[dst - 1] && expTiles[dst + expWidth] && tiles[src - 1 + width];
                    expTiles[dst + 1 - expWidth] = expTiles[dst + 1] && expTiles[dst - expWidth] && tiles[src + 1 - width];
                    expTiles[dst - 1 - expWidth] = expTiles[dst - 1] && expTiles[dst - expWidth] && tiles[src - 1 - width];
                }
            }
        }

        // Generate rectangles
        for (int y = 0; y < expHeight; y++)
        {
            for (int x = 0; x < expWidth; x++)
            {
                if (!expTiles[x + y * expWidth]) continue;

                Vector2Int rectSize = GetMaximalRectSize(expTiles, x, y, expWidth, expHeight);

                yield return Rect.MinMaxRect(
                    xmin: GetCoord(x),
                    ymin: GetCoord(y),
                    xmax: GetCoord(x + rectSize.x),
                    ymax: GetCoord(y + rectSize.y)
                );

                // Mark rectangle as covered
                for (int oy = 0; oy < rectSize.y; oy++)
                {
                    for (int ox = 0; ox < rectSize.x; ox++)
                    {
                        expTiles[x + ox + (y + oy) * expWidth] = false;
                    }
                }
            }
        }

        float GetCoord(int x)
        {
            return (x % 3) switch
            {
                0 => x / 3,
                1 => x / 3 + margin,
                2 => x / 3 + 1f - margin,
                _ => throw new ArgumentException("x must be positive!")
            };
        }
    }

    private static Vector2Int GetMaximalRectSize(bool[] tiles, int startX, int startY, int width, int height)
    {
        int endX;
        int endY;

        // Find the maximum width this rectangle can occupy
        for (endX = startX; endX < width; endX++)
        {
            if (!tiles[endX + startY * width])
                break;
        }

        // Find the maximum height a rectangle of this width can occupy
        for (endY = startY; endY < height; endY++)
        {
            if (!RangeAllTrue(tiles, startX + endY * width, endX - startX))
                break;
        }

        return new Vector2Int(endX - startX, endY - startY);
    }

    private static bool RangeAllTrue(bool[] mask, int start, int length)
    {
        for(int i = start; i < start + length; i++)
        {
            if (!mask[i]) return false;
        }
        return true;
    }
}