using System;
using UnityEngine;

public class Grid<T>
{
    private T[,] _cells;
    private float _cellSize;
    private Vector3 _originPosition;

    public Action<int, int> OnCellChanged;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> createCell)
    {
        _cells = new T[width, height];
        _cellSize = cellSize;
        _originPosition = originPosition;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _cells[x, y] = createCell(this, x, y);
            }
        }
    }

    public int Width
    {
        get => _cells.GetLength(0);
    }

    public int Height
    {
        get => _cells.GetLength(1);
    }

    public float CellSize
    {
        get => _cellSize;
    }

    public Vector3 OriginPosition
    {
        get => _originPosition;
    }

    public T GetCell(Vector3 pos)
    {
        GetXY(pos, out int x, out int y);
        return GetCell(x, y);
    }

    public T GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return _cells[x, y];

        return default;
    }

    public void SetCell(T cell, Vector3 pos)
    {
        GetXY(pos, out int x, out int y);
        SetCell(cell, x, y);

        TriggerCellChanged(x, y);
    }

    public void TriggerCellChanged(int x, int y)
    {
        if (OnCellChanged != null)
            OnCellChanged(x, y);
    }

    public void SetCell(T cell, int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            _cells[x, y] = cell;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }
}