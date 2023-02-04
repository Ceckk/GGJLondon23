using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _hp = 1;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _movementAnimationSpeed = 0.25f;
    
    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    public int Hp { get => _hp; set => _hp = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    void Start()
    {
        // TODO spawn animation
    }

    public virtual void Move()
    {
        if (!_tween.IsActive())
        {
            var playerPos = PlayerManager.Instance.transform.position;
            var movement = TilemapManager.Instance.CellSize;

            var differenceX = Mathf.Abs(playerPos.x - transform.position.x);
            var differenceY = Mathf.Abs(playerPos.y - transform.position.y);

            if (differenceX < differenceY)
            {
                _tween = transform.DOLocalMoveY(playerPos.y > transform.position.y ? movement : -movement, _movementAnimationSpeed);
            }
            else
            {
                _tween = transform.DOLocalMoveX(playerPos.x > transform.position.x ? movement : -movement, _movementAnimationSpeed);
            }

            _tween.SetRelative().SetEase(Ease.InOutSine);
        }
    }

    public void Die()
    {
        _isDead = true;

        // TODO death animation -> Destroy
        Destroy(gameObject);
    }
}
