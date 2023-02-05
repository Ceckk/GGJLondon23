using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TicksManager : MonoSingleton<TicksManager>
{
    [SerializeField] private float _timePerSimpleTick = 1;
    [SerializeField] private float _timePerSpecialTick = 2;
    [SerializeField] private int _numberOfTicksBeforeSpecialTick = 3;

    [SerializeField] private AudioClip _simpleTickSound;
    [SerializeField] private AudioClip _specialTckSound;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private Image _beatImage;
    [SerializeField] private Sprite[] _beatsSprites;

    public class OnSimpleTick : IEvent { }
    public class OnSpecialTick : IEvent { }

    private int _index = 0;

    public float TimePerSpecialTick { get => _timePerSpecialTick; }
    public float TimePerSimpleTick { get => _timePerSimpleTick; }

    void Start()
    {
        StartCoroutine(TicksUpdate());
    }

    private IEnumerator TicksUpdate()
    {
        while (!PlayerManager.Instance.IsDead)
        {
            for (int i = 0; i < _numberOfTicksBeforeSpecialTick; i++)
            {
                if (PlayerManager.Instance.IsDead)
                    yield break;

                _beatImage.sprite = _beatsSprites[i];

                _audioSource.clip = _simpleTickSound;
                _audioSource.Play();

                EventAggregator.Instance.Dispatch<OnSimpleTick>();

                if (i == 0)
                {
                    EnemyManager.Instance.ResolveEnemyMovement();
                    EnemyManager.Instance.SpawnEnemies();
                }

                yield return new WaitForSeconds(_timePerSimpleTick);
            }

            if (PlayerManager.Instance.IsDead)
                yield break;

            _beatImage.sprite = _beatsSprites[_numberOfTicksBeforeSpecialTick];

            _audioSource.clip = _specialTckSound;
            _audioSource.Play();

            EventAggregator.Instance.Dispatch<OnSpecialTick>();

            HandlePlayerAttack();

            yield return new WaitForSeconds(_timePerSpecialTick);
        }
    }

    private void HandlePlayerAttack()
    {
        var pos = PlayerManager.Instance.transform.position;
        var tile = TilemapManager.Instance.GetTile(pos);

        if (tile != null && !tile.name.StartsWith("Dirt"))
        {
            TilemapManager.Instance.Attack(pos);
            EnemyManager.Instance.ResolvePlayerAttack(pos, tile.name);
        }
    }
}
