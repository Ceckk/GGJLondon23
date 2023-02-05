using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private Enemy[] _enemyPrefabs;

    private List<Enemy> _spawnedEnemyList = new List<Enemy>();

    private int _spawnRound;
    private int _killCount;
    private List<Vector3> _reservedPositions = new List<Vector3>();

    public int SpawnRound { get => _spawnRound; }
    public int KillCount { get => _killCount; }

    public void SpawnEnemies()
    {
        _spawnRound++;
        GameManager.Instance.UpdateHighScore();

        var spawnPositions = new List<Vector3>();

        _reservedPositions.Add(PlayerManager.Instance.transform.position);
        _reservedPositions.Add(PlayerManager.Instance.transform.position + Vector3.up * TilemapManager.Instance.CellSize);
        _reservedPositions.Add(PlayerManager.Instance.transform.position + Vector3.down * TilemapManager.Instance.CellSize);
        _reservedPositions.Add(PlayerManager.Instance.transform.position + Vector3.left * TilemapManager.Instance.CellSize);
        _reservedPositions.Add(PlayerManager.Instance.transform.position + Vector3.right * TilemapManager.Instance.CellSize);

        for (float x = TilemapManager.MIN_VALUE; x <= TilemapManager.MAX_VALUE; x += TilemapManager.Instance.CellSize)
        {
            var pos = new Vector3(x, TilemapManager.MIN_VALUE);
            if (!_reservedPositions.Contains(pos))
            {
                spawnPositions.Add(pos);
            }

            pos = new Vector3(x, TilemapManager.MAX_VALUE);
            if (!_reservedPositions.Contains(pos))
            {
                spawnPositions.Add(pos);
            }
        }

        for (float y = TilemapManager.MIN_VALUE + TilemapManager.Instance.CellSize; y <= TilemapManager.MAX_VALUE - TilemapManager.Instance.CellSize; y += TilemapManager.Instance.CellSize)
        {
            var pos = new Vector3(TilemapManager.MIN_VALUE, y);
            if (!_reservedPositions.Contains(pos))
            {
                spawnPositions.Add(pos);
            }

            pos = new Vector3(TilemapManager.MAX_VALUE, y);
            if (!_reservedPositions.Contains(pos))
            {
                spawnPositions.Add(pos);
            }
        }


        if (_spawnRound % 2 == 0)
        {
            var enemy = Instantiate(_enemyPrefabs[UnityEngine.Random.Range(1, _enemyPrefabs.Length)], transform);
            var spawnPos = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            enemy.transform.position = spawnPos;
            _spawnedEnemyList.Add(enemy);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                var enemy = Instantiate(_enemyPrefabs[0], transform);
                var spawnPos = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
                spawnPositions.Remove(spawnPos);
                enemy.transform.position = spawnPos;
                _spawnedEnemyList.Add(enemy);
            }
        }
    }

    public void ResolvePlayerAttack(Vector3 pos, string attackType)
    {
        var cellSize = TilemapManager.Instance.CellSize;

        switch (attackType)
        {
            case "Hay":

                PlayerManager.Instance.HorizontalAttack();

                for (float x = TilemapManager.MIN_VALUE; x <= TilemapManager.MAX_VALUE; x += cellSize)
                {
                    foreach (var enemy in _spawnedEnemyList)
                    {
                        var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(x, pos.y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                            break;
                        }
                    }
                }
                break;
            case "Flowers":
                PlayerManager.Instance.VerticalAttack();
                for (float y = TilemapManager.MIN_VALUE; y <= TilemapManager.MAX_VALUE; y += cellSize)
                {
                    foreach (var enemy in _spawnedEnemyList)
                    {
                        var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(pos.x, y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                            break;
                        }
                    }
                }
                break;
            case "Water":
                PlayerManager.Instance.AroundAttack();
                for (float x = -cellSize; x <= cellSize; x += cellSize)
                {
                    for (float y = -cellSize; y <= cellSize; y += cellSize)
                    {
                        foreach (var enemy in _spawnedEnemyList)
                        {
                            var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);
                            var tpos = TilemapManager.Instance.GetCellPosition(pos + new Vector3(x, y));
                            if (enemyCellPos == tpos)
                            {
                                Hit(enemy);
                                break;
                            }
                        }
                    }
                }
                break;
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
                PlayerManager.Instance.Die();
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
            PlayerManager.Instance.Die();
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
        _reservedPositions = new List<Vector3>();

        foreach (var enemy in _spawnedEnemyList)
        {
            _reservedPositions.Add(enemy.transform.position);
        }

        foreach (var enemy in _spawnedEnemyList)
        {
            enemy.Move(ref _reservedPositions);
        }
    }
}