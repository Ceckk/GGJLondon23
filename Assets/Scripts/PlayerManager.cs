using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private float _movementSpeed = 0.25f;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    private float minValue = -6;
    private float maxValue = 4;

    void Update()
    {
        if (!_tween.IsActive())
        {
            var movement = TilemapManager.Instance.GridSize;
            var oldPos = transform.position;
            
            if (oldPos.y < maxValue && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                _tween = transform.DOLocalMoveY(movement, _movementSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(() => TilemapManager.Instance.PlayerMoved(oldPos, transform.position));
            }
            else if (oldPos.x > minValue && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                _tween = transform.DOLocalMoveX(-movement, _movementSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(() => TilemapManager.Instance.PlayerMoved(oldPos, transform.position));
            }
            else if (oldPos.y > minValue && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                _tween = transform.DOLocalMoveY(-movement, _movementSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(() => TilemapManager.Instance.PlayerMoved(oldPos, transform.position));
            }
            else if (oldPos.x < maxValue && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                _tween = transform.DOLocalMoveX(movement, _movementSpeed).SetRelative().SetEase(Ease.InOutSine).OnComplete(() => TilemapManager.Instance.PlayerMoved(oldPos, transform.position));
            }
        }
    }
}
