using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoSingleton<TilemapManager>
{
    private const int COOLDOWN_MAX = 36;
    private const int COOLDOWN_YELLOW = 24;

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase[] _tiles;
    [SerializeField] private TileBase[] _coolDownTiles;

    private Dictionary<Vector3Int, int> _tilesDictionary = new Dictionary<Vector3Int, int>();
    private Dictionary<Vector3Int, int> _cooldownTilesDictionary = new Dictionary<Vector3Int, int>();

    public float CellSize
    {
        get
        {
            return _tilemap.layoutGrid.cellSize.x;
        }
    }

    void Start()
    {
        EventAggregator.Instance.AddListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.AddListener<TicksManager.OnSpecialTick>(OnTick);
        GenerateRandomMap();
    }

    void OnDestroy()
    {
        EventAggregator.Instance.RemoveListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.RemoveListener<TicksManager.OnSpecialTick>(OnTick);
    }

    private void OnTick(IEvent obj)
    {
        var keys = _cooldownTilesDictionary.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            _cooldownTilesDictionary[key]--;
            if (_cooldownTilesDictionary[key] <= 0)
            {
                ChangeTile(key, _tiles[_tilesDictionary[key]]);
                _cooldownTilesDictionary.Remove(key);
            }
            else if (_cooldownTilesDictionary[key] <= COOLDOWN_YELLOW)
            {
                ChangeTile(key, _coolDownTiles[1]);
            }
        }
    }

    private void GenerateRandomMap()
    {
        var bounds = _tilemap.localBounds;
        for (float x = bounds.min.x; x < bounds.max.x; x += CellSize)
        {
            for (float y = bounds.min.y; y < bounds.max.y; y += CellSize)
            {
                var tpos = _tilemap.WorldToCell(new Vector3(x, y));
                var tile = _tilemap.GetTile(tpos);
                if (tile != null)
                {
                    _tilesDictionary[tpos] = UnityEngine.Random.Range(0, _tiles.Length);
                    var newTile = _tiles[_tilesDictionary[tpos]];
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

    public void AroundAttack(Vector3 pos)
    {
        Debug.Log("AroundAttack");

        _tilesDictionary[_tilemap.WorldToCell(pos)] = UnityEngine.Random.Range(0, _tiles.Length);

        for (float x = -CellSize; x <= CellSize; x += CellSize)
        {
            for (float y = -CellSize; y <= CellSize; y += CellSize)
            {
                var tpos = _tilemap.WorldToCell(pos + new Vector3(x, y));
                if (ChangeTile(tpos, _coolDownTiles[0]))
                {
                    _cooldownTilesDictionary[tpos] = COOLDOWN_MAX;
                }
            }
        }
    }

    public void HorizontalAttack(Vector3 pos)
    {
        Debug.Log("HorizontalAttack");

        _tilesDictionary[_tilemap.WorldToCell(pos)] = UnityEngine.Random.Range(0, _tiles.Length);

        var bounds = _tilemap.localBounds;
        for (float x = bounds.min.x; x < bounds.max.x; x += CellSize)
        {
            var tpos = _tilemap.WorldToCell(new Vector3(x, pos.y));
            if (ChangeTile(tpos, _coolDownTiles[0]))
            {
                _cooldownTilesDictionary[tpos] = COOLDOWN_MAX;
            }
        }
    }

    public void VerticalAttack(Vector3 pos)
    {
        Debug.Log("VerticalAttack");

        _tilesDictionary[_tilemap.WorldToCell(pos)] = UnityEngine.Random.Range(0, _tiles.Length);

        var bounds = _tilemap.localBounds;
        for (float y = bounds.min.y; y < bounds.max.y; y += CellSize)
        {
            var tpos = _tilemap.WorldToCell(new Vector3(pos.x, y));
            if (ChangeTile(tpos, _coolDownTiles[0]))
            {
                _cooldownTilesDictionary[tpos] = COOLDOWN_MAX;
            }
        }
    }

    public TileBase GetTile(Vector3 pos)
    {
        var tpos = _tilemap.WorldToCell(pos);
        return _tilemap.GetTile(tpos);
    }

    private bool ChangeTile(Vector3Int tpos, TileBase newTile)
    {
        var tile = _tilemap.GetTile(tpos);
        if (tile != null)
        {
            _tilemap.SetTile(tpos, newTile);
            return true;
        }

        return false;
    }
}