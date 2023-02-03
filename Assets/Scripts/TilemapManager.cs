using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoSingleton<TilemapManager>
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase[] _tiles;

    public float GridSize
    {
        get
        {
            return _tilemap.layoutGrid.cellSize.x;
        }
    }

    public void PlayerMoved(Vector3 oldPos, Vector3 newPos)
    {
        ChangeTile(oldPos, _tiles[0]);
        ChangeTile(newPos, _tiles[1]);
    }

    private void ChangeTile(Vector3 worldPoint, TileBase newTile)
    {
        var tpos = _tilemap.WorldToCell(worldPoint);
        var tile = _tilemap.GetTile(tpos);
        var sprite = _tilemap.GetSprite(tpos);

        if (tile)
        {
            _tilemap.SetTile(tpos, newTile);
        }
        else
        {
            Debug.LogError("No tile");
        }
    }

}