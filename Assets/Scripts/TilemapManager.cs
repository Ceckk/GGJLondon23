using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoSingleton<TilemapManager>
{
    private const int COOLDOWN_MAX = 36;
    private const int COOLDOWN_YELLOW = 24;

    public const float MIN_VALUE = -6;
    public const float MAX_VALUE = 4;

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

    public Bounds MapBounds
    {
        get
        {
            return _tilemap.localBounds;
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

        _tilemap.RefreshAllTiles();
    }

    public void Attack(Vector3 pos)
    {
        Debug.Log("Attack");

        var tpos = _tilemap.WorldToCell(pos);
        _tilesDictionary[tpos] = UnityEngine.Random.Range(0, _tiles.Length);
        if (ChangeTile(tpos, _coolDownTiles[0]))
        {
            _cooldownTilesDictionary[tpos] = COOLDOWN_MAX;
        }
    }

    public Vector3Int GetCellPosition(Vector3 pos)
    {
        return _tilemap.WorldToCell(pos);
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