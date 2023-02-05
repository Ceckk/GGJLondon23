using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private float _movementAnimationSpeed = 0.25f;
    [SerializeField] private GameObject[] _leftAttackObjs;
    [SerializeField] private GameObject[] _rightAttackObjs;
    [SerializeField] private GameObject[] _upAttackObjs;
    [SerializeField] private GameObject[] _downAttackObjs;

    private Tweener _tween;

    private List<GameObject> _attackObjects = new List<GameObject>();

    void Start()
    {
        EventAggregator.Instance.AddListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.AddListener<TicksManager.OnSpecialTick>(OnSpecialTick);
        TilemapManager.Instance.PlayerMoved(transform.position, transform.position);
    }

    void OnDestroy()
    {
        EventAggregator.Instance.RemoveListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.RemoveListener<TicksManager.OnSpecialTick>(OnSpecialTick);
    }

    private void OnSpecialTick(IEvent obj)
    {
        if (_tween.IsActive())
        {
            _tween.Complete();
        }

        _tween = transform.DOPunchScale(new Vector3(0, 0f, 0), TicksManager.Instance.TimePerSpecialTick * 0.5f);
    }

    private void OnTick(IEvent e)
    {
        foreach(var obj in _attackObjects)
        {
            Destroy(obj);
        }

        _attackObjects = new List<GameObject>();
        // transform.DOPunchScale(new Vector3(0, -0.5f, 0), TicksManager.Instance.TimePerSimpleTick * 0.25f);
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

            _tween.SetRelative().SetEase(Ease.InOutSine).OnComplete(() => EnemyManager.Instance.CheckPlayerHit());
        }
    }

    public void HorizontalAttack()
    {
        var bounds = TilemapManager.Instance.MapBounds;
        var cellSize = TilemapManager.Instance.CellSize;

        for (float x = TilemapManager.MIN_VALUE; x <= TilemapManager.MAX_VALUE; x += cellSize)
        {
            var pos = new Vector3(x, transform.position.y);
            if (pos.x > transform.position.x)
            {
                StartCoroutine(SpawnObject(x < TilemapManager.MAX_VALUE ? _rightAttackObjs[0] : _rightAttackObjs[1], pos, (pos.x - transform.position.x) / cellSize / 10f));
            }
            else if (pos.x < transform.position.x)
            {
                StartCoroutine(SpawnObject(x > TilemapManager.MIN_VALUE ? _leftAttackObjs[0] : _leftAttackObjs[1], pos, (transform.position.x - pos.x) / cellSize / 10f));
            }
        }
    }

    public void VerticalAttack()
    {
        // throw new NotImplementedException();
    }

    public void AroundAttack()
    {
        // throw new NotImplementedException();
    }

    private IEnumerator SpawnObject(GameObject obj, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackObjects.Add(Instantiate(obj, pos, Quaternion.identity));
    }
}
