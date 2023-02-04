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

    public virtual void Move(ref List<Vector3> reservedPos)
    {
        if (!_tween.IsActive())
        {
            var playerPos = PlayerManager.Instance.transform.position;
            var movement = TilemapManager.Instance.CellSize;

            var differenceX = Mathf.Abs(playerPos.x - transform.position.x);
            var differenceY = Mathf.Abs(playerPos.y - transform.position.y);

            if (differenceX < differenceY)
            {
                if (playerPos.y > transform.position.y)
                {
                    var targetPos = transform.position + Vector3.up * movement;
                    if (!reservedPos.Contains(targetPos))
                    {
                        _tween = transform.DOLocalMoveY(movement, _movementAnimationSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(OnComplete);
                        reservedPos.Add(targetPos);
                        return;
                    }
                }
                else
                {
                    var targetPos = transform.position + Vector3.down * movement;
                    if (!reservedPos.Contains(targetPos))
                    {
                        _tween = transform.DOLocalMoveY(-movement, _movementAnimationSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(OnComplete);
                        reservedPos.Add(targetPos);
                        return;
                    }
                }
            }
            else
            {
                if (playerPos.x > transform.position.x)
                {
                    var targetPos = transform.position + Vector3.right * movement;
                    if (!reservedPos.Contains(targetPos))
                    {
                        _tween = transform.DOLocalMoveX(movement, _movementAnimationSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(OnComplete);
                        reservedPos.Add(targetPos);
                        return;
                    }
                }
                else
                {
                    var targetPos = transform.position + Vector3.left * movement;
                    if (!reservedPos.Contains(targetPos))
                    {
                        _tween = transform.DOLocalMoveX(-movement, _movementAnimationSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(OnComplete);
                        reservedPos.Add(targetPos);
                        return;
                    }
                }
            }
        }
    }

    protected virtual void OnComplete()
    {
        EnemyManager.Instance.CheckPlayerHit(this);
    }

    public void Die()
    {
        _isDead = true;

        // TODO death animation -> Destroy
        Destroy(gameObject);
    }
}
