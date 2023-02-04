using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private Enemy[] _enemyPrefabs;

    private List<Enemy> _spawnedEnemyList = new List<Enemy>();

    private int _spawnRound;
    private int _killCount;

    public int SpawnRound { get => _spawnRound; }
    public int KillCount { get => _killCount; }

    public void SpawnEnemies()
    {
        _spawnRound++;
        GameManager.Instance.UpdateHighScore();

        var spawnPositions = new List<Vector3>();

        var reservedPos = new List<Vector3>();
        foreach (var enemy in _spawnedEnemyList)
        {
            reservedPos.Add(enemy.transform.position);
        }

        reservedPos.Add(PlayerManager.Instance.transform.position);

        for (float x = TilemapManager.MIN_VALUE; x <= TilemapManager.MAX_VALUE; x += TilemapManager.Instance.CellSize)
        {
            var pos = new Vector3(x, TilemapManager.MIN_VALUE);
            if (!reservedPos.Contains(pos))
            {
                spawnPositions.Add(pos);
            }

            pos = new Vector3(x, TilemapManager.MAX_VALUE);
            if (!reservedPos.Contains(pos))
            {
                spawnPositions.Add(pos);
            }
        }

        for (float y = TilemapManager.MIN_VALUE + TilemapManager.Instance.CellSize; y <= TilemapManager.MAX_VALUE - TilemapManager.Instance.CellSize; y += TilemapManager.Instance.CellSize)
        {
            var pos = new Vector3(TilemapManager.MIN_VALUE, y);
            if (!reservedPos.Contains(pos))
            {
                spawnPositions.Add(pos);
            }

            pos = new Vector3(TilemapManager.MAX_VALUE, y);
            if (!reservedPos.Contains(pos))
            {
                spawnPositions.Add(pos);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            Enemy enemy = null;
            if (_spawnRound % 2 == 0)
            {
                enemy = Instantiate(_enemyPrefabs[UnityEngine.Random.Range(1, _enemyPrefabs.Length)], transform);
            }
            else
            {
                enemy = Instantiate(_enemyPrefabs[0], transform);
            }
            var spawnPos = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            spawnPositions.Remove(spawnPos);
            enemy.transform.position = spawnPos;
            _spawnedEnemyList.Add(enemy);
        }
    }

    public void ResolvePlayerAttack(Vector3 pos, string attackType)
    {
        foreach (var enemy in _spawnedEnemyList)
        {
            var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);
            var bounds = TilemapManager.Instance.MapBounds;
            var cellSize = TilemapManager.Instance.CellSize;

            switch (attackType)
            {
                case "Hay":
                    for (float x = bounds.min.x; x < bounds.max.x; x += cellSize)
                    {
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(x, pos.y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                        }
                    }
                    break;
                case "Flowers":
                    for (float y = bounds.min.y; y < bounds.max.y; y += cellSize)
                    {
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(pos.x, y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                        }
                    }
                    break;
                case "Water":
                    for (float x = -cellSize; x <= cellSize; x += cellSize)
                    {
                        for (float y = -cellSize; y <= cellSize; y += cellSize)
                        {
                            var tpos = TilemapManager.Instance.GetCellPosition(pos + new Vector3(x, y));
                            if (enemyCellPos == tpos)
                            {
                                Hit(enemy);
                            }
                        }
                    }
                    break;
            }
        }

        _spawnedEnemyList.RemoveAll(e => e.IsDead);
    }

    public bool CheckPlayerHit()
    {
        var playerCellPos = TilemapManager.Instance.GetCellPosition(PlayerManager.Instance.transform.position);
        foreach (var enemy in _spawnedEnemyList)
        {
            var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);

            if (enemyCellPos == playerCellPos)
            {
                GameManager.Instance.Restart();
                return true;
            }
        }

        return false;
    }

    public bool CheckPlayerHit(Enemy enemy)
    {
        var playerCellPos = TilemapManager.Instance.GetCellPosition(PlayerManager.Instance.transform.position);
        var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);

        if (enemyCellPos == playerCellPos)
        {
            GameManager.Instance.Restart();
            return true;
        }

        return false;
    }

    private void Hit(Enemy enemy)
    {
        enemy.Hp--;
        if (enemy.Hp <= 0)
        {
            enemy.Die();
            _killCount++;
            GameManager.Instance.UpdateHighScore();
        }
    }

    public void ResolveEnemyMovement()
    {
        var reservedPositions = new List<Vector3>();

        foreach (var enemy in _spawnedEnemyList)
        {
            reservedPositions.Add(enemy.transform.position);
        }

        foreach (var enemy in _spawnedEnemyList)
        {
            enemy.Move(ref reservedPositions);
        }
    }
}