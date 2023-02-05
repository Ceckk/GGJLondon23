using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private float _movementAnimationSpeed = 0.25f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private GameObject[] _leftAttackObjs;
    [SerializeField] private GameObject[] _rightAttackObjs;
    [SerializeField] private GameObject[] _upAttackObjs;
    [SerializeField] private GameObject[] _downAttackObjs;
    [SerializeField] private GameObject _aroundAttackObj;

    private Tweener _tween;

    private List<GameObject> _attackObjects = new List<GameObject>();
    private bool _isDead = false;

    public bool IsDead { get => _isDead; }

    void Start()
    {
        EventAggregator.Instance.AddListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.AddListener<TicksManager.OnSpecialTick>(OnSpecialTick);
    }

    void OnDestroy()
    {
        EventAggregator.Instance.RemoveListener<TicksManager.OnSimpleTick>(OnTick);
        EventAggregator.Instance.RemoveListener<TicksManager.OnSpecialTick>(OnSpecialTick);
    }

    private void OnSpecialTick(IEvent obj)
    {
        _animator.SetBool("IsAttacking", true);
        _shadow.gameObject.SetActive(false);

        if (_tween.IsActive())
        {
            _tween.Complete();
        }

        _tween = transform.DOPunchScale(new Vector3(0, 0f, 0), TicksManager.Instance.TimePerSpecialTick * 0.5f);
    }

    private void OnTick(IEvent e)
    {
        _animator.SetBool("IsAttacking", false);
        _shadow.gameObject.SetActive(true);

        foreach(var obj in _attackObjects)
        {
            Destroy(obj);
        }

        _attackObjects = new List<GameObject>();
    }

    void Update()
    {
        if (_isDead)
            return;

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
                _sprite.flipX = false;
                _tween = transform.DOLocalMoveX(-movement, _movementAnimationSpeed);
            }
            else if (oldPos.y > TilemapManager.MIN_VALUE && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                _tween = transform.DOLocalMoveY(-movement, _movementAnimationSpeed);
            }
            else if (oldPos.x < TilemapManager.MAX_VALUE && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                _sprite.flipX = true;
                _tween = transform.DOLocalMoveX(movement, _movementAnimationSpeed);
            }

            _tween.SetRelative().SetEase(Ease.InOutSine).OnComplete(() => EnemyManager.Instance.CheckPlayerHit());

            if (_tween.IsActive())
            {
                _animator.SetBool("IsAttacking", false);
                _shadow.gameObject.SetActive(true);

                foreach (var obj in _attackObjects)
                {
                    Destroy(obj);
                }

                _attackObjects = new List<GameObject>();
            }
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
        var bounds = TilemapManager.Instance.MapBounds;
        var cellSize = TilemapManager.Instance.CellSize;

        for (float y = TilemapManager.MIN_VALUE; y <= TilemapManager.MAX_VALUE; y += cellSize)
        {
            var pos = new Vector3(transform.position.x, y);
            if (pos.y > transform.position.y)
            {
                StartCoroutine(SpawnObject(y < TilemapManager.MAX_VALUE ? _upAttackObjs[0] : _upAttackObjs[1], pos, (pos.y - transform.position.y) / cellSize / 10f));
            }
            else if (pos.y < transform.position.y)
            {
                StartCoroutine(SpawnObject(y > TilemapManager.MIN_VALUE ? _downAttackObjs[0] : _downAttackObjs[1], pos, (transform.position.y - pos.y) / cellSize / 10f));
            }
        }
    }

    public void AroundAttack()
    {
        _attackObjects.Add(Instantiate(_aroundAttackObj, transform.position, Quaternion.identity));
    }

    private IEnumerator SpawnObject(GameObject obj, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackObjects.Add(Instantiate(obj, pos, Quaternion.identity));
    }

    public void Die()
    {
        _isDead = true;
        _animator.SetBool("IsDead", true);
        _shadow.gameObject.SetActive(false);

        foreach (var obj in _attackObjects)
        {
            Destroy(obj);
        }

        Debug.Log("GAME OVER");

        StartCoroutine(EndScreen());
    }

    private IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.End();
    }
}
