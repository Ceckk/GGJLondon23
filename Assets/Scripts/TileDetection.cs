using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDetection : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var tpos = _tilemap.WorldToCell(worldPoint);

            // Try to get a tile from cell position
            var tile = _tilemap.GetTile(tpos);
            var sprite = _tilemap.GetSprite(tpos);

            if (tile)
            {
                _tilemap.SetTile(tpos, _tile);
                Debug.Log(tile.name);
                
            }
            else
            {
                Debug.Log("No tile");
            }
        }
    }
}
