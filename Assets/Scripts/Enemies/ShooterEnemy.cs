using System;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] private Projectile _projectilePrefab;

    protected override void OnComplete()
    {
        if (!EnemyManager.Instance.CheckPlayerHit(this))
        {
            Shoot(PlayerManager.Instance.transform.position);
        }
    }

    private void Shoot(Vector3 position)
    {
        _animator.SetTrigger("Shoot");
        Instantiate(_projectilePrefab, transform.position, Quaternion.LookRotation(position - transform.position));
    }
}
