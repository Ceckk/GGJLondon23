using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private float _movementAnimationSpeed = 0.25f;

    private Tweener _tween;

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
        
        _tween = transform.DOPunchScale(new Vector3(0, -1f, 0), TicksManager.Instance.TimePerSpecialTick * 0.5f);
    }

    private void OnTick(IEvent obj)
    {
        transform.DOPunchScale(new Vector3(0, -0.5f, 0), TicksManager.Instance.TimePerSimpleTick * 0.25f);
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
}
