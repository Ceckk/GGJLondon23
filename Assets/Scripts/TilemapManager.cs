using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoSingleton<TilemapManager>
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase[] _tiles;

    public float CellSize
    {
        get
        {
            return _tilemap.layoutGrid.cellSize.x;
        }
    }

    void Start()
    {
        GenerateRandomMap();
    }

    private void GenerateRandomMap()
    {
        var bounds = _tilemap.localBounds;
        for (float x = bounds.min.x; x < bounds.max.x; x+=CellSize)
        {
            for (float y = bounds.min.y; y < bounds.max.y; y+=CellSize)
            {
                var tpos = _tilemap.WorldToCell(new Vector3(x, y));
                var tile = _tilemap.GetTile(tpos);
                if (tile != null)
                {
                    var newTile = _tiles[UnityEngine.Random.Range(0, _tiles.Length)];
                    _tilemap.SetTile(tpos, newTile);
                    // Debug.Log("x:" + x + " y:" + y + " tile:" + newTile.name);
                }
            }
        }
    }

    public void PlayerMoved(Vector3 oldPos, Vector3 newPos)
    {
        // ChangeTile(oldPos, _tiles[0]);
        // ChangeTile(newPos, _tiles[1]);
    }

    public TileBase GetTile(Vector3 pos)
    {
        var tpos = _tilemap.WorldToCell(pos);
        return _tilemap.GetTile(tpos);
    }

    private void ChangeTile(Vector3 worldPoint, TileBase newTile)
    {
        var tpos = _tilemap.WorldToCell(worldPoint);
        var tile = _tilemap.GetTile(tpos);

        if (tile != null)
        {
            _tilemap.SetTile(tpos, newTile);
        }
        else
        {
            Debug.LogError("No tile");
        }
    }
}