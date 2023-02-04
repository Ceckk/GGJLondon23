using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private float _movementAnimationSpeed = 0.25f;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    void Start()
    {
        TilemapManager.Instance.PlayerMoved(transform.position, transform.position);
    }

    void Update()
    {
        if (!_tween.IsActive())
        {
            var movement = TilemapManager.Instance.CellSize;
            var oldPos = transform.position;

            if (oldPos.y < TilemapManager.MAX_VALUE && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                _tween = transform.DOLocalMoveY(movement, _movementAnimationSpeed);
            }
            else if (oldPos.x > TilemapManager.MIN_VALUE && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                _tween = transform.DOLocalMoveX(-movement, _movementAnimationSpeed);
            }
            else if (oldPos.y > TilemapManager.MIN_VALUE && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                _tween = transform.DOLocalMoveY(-movement, _movementAnimationSpeed);
            }
            else if (oldPos.x < TilemapManager.MAX_VALUE && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                _tween = transform.DOLocalMoveX(movement, _movementAnimationSpeed);
            }

            _tween.SetRelative().SetEase(Ease.InOutSine).OnComplete(EnemyManager.Instance.CheckPlayerHit);
        }
    }
}
