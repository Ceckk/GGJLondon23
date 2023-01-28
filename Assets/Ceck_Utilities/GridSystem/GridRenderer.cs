using UnityEngine;


public abstract class GridRenderer<T> : MonoBehaviour
{
    protected Grid<T> _grid;

    void OnDestroy()
    {
        if (_grid != null)
        {
            _grid.OnCellChanged -= OnCellChanged;
        }
    }

    public virtual void SetGrid(Grid<T> grid)
    {
        if (_grid != null)
        {
            _grid.OnCellChanged -= OnCellChanged;
        }

        _grid = grid;
        _grid.OnCellChanged += OnCellChanged;
        DebugDrawGrid(_grid, 100);
    }

    protected abstract void OnCellChanged(int x, int y);

    private void DebugDrawGrid(Grid<T> grid, float duration)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.black, duration);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.black, duration);
            }
        }

        Debug.DrawLine(grid.GetWorldPosition(0, grid.Height), grid.GetWorldPosition(grid.Width, grid.Height), Color.black, duration);
        Debug.DrawLine(grid.GetWorldPosition(grid.Width, 0), grid.GetWorldPosition(grid.Width, grid.Height), Color.black, duration);
    }
}
