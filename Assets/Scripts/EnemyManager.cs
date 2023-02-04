using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private Enemy[] _enemyPrefabs;

    private List<Enemy> _spawnedEnemyList = new List<Enemy>();

    public void SpawnEnemies(int amount)
    {
        var spawnPositions = new List<Vector3>
        {
            new Vector3(TilemapManager.MAX_VALUE, TilemapManager.MAX_VALUE),
            new Vector3(TilemapManager.MIN_VALUE, TilemapManager.MIN_VALUE),
            new Vector3(TilemapManager.MIN_VALUE, TilemapManager.MAX_VALUE),
            new Vector3(TilemapManager.MAX_VALUE, TilemapManager.MIN_VALUE),
        };

        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(_enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)], transform);
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
                case "Horizontal":
                    for (float x = bounds.min.x; x < bounds.max.x; x += cellSize)
                    {
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(x, pos.y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                        }
                    }
                    break;
                case "Vertical":
                    for (float y = bounds.min.y; y < bounds.max.y; y += cellSize)
                    {
                        var tpos = TilemapManager.Instance.GetCellPosition(new Vector3(pos.x, y));
                        if (enemyCellPos == tpos)
                        {
                            Hit(enemy);
                        }
                    }
                    break;
                case "Around":
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

    public void CheckPlayerHit()
    {
        var playerCellPos = TilemapManager.Instance.GetCellPosition(PlayerManager.Instance.transform.position);
        foreach(var enemy in _spawnedEnemyList)
        {
            var enemyCellPos = TilemapManager.Instance.GetCellPosition(enemy.transform.position);

            if (enemyCellPos == playerCellPos)
            {
                GameManager.Instance.Restart();
            }
        }
    }

    private void Hit(Enemy enemy)
    {
        enemy.Hp--;
        if (enemy.Hp <= 0)
        {
            enemy.Die();
        }
    }

    public void ResolveEnemyMovement()
    {
        foreach (var enemy in _spawnedEnemyList)
        {
            enemy.Move();
        }
    }
}