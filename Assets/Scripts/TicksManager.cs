using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TicksManager : MonoSingleton<TicksManager>
{
    [SerializeField] private float _timePerSimpleTick = 1;
    [SerializeField] private float _timePerSpecialTick = 2;
    [SerializeField] private float _numberOfTicksBeforeSpecialTick = 4;

    [SerializeField] private AudioClip _simpleTickSound;
    [SerializeField] private AudioClip _specialTckSound;
    [SerializeField] private AudioSource _audioSource;

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

                _audioSource.clip = _simpleTickSound;
                _audioSource.Play();

                EventAggregator.Instance.Dispatch<OnSimpleTick>();

                if (i == 0)
                {
                    EnemyManager.Instance.SpawnEnemies();
                }

                yield return new WaitForSeconds(_timePerSimpleTick);
            }

            if (PlayerManager.Instance.IsDead)
                yield break;

            _audioSource.clip = _specialTckSound;
            _audioSource.Play();

            EventAggregator.Instance.Dispatch<OnSpecialTick>();

            HandlePlayerAttack();
            HandleEnemyBehaviour();

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

    private void HandleEnemyBehaviour()
    {
        EnemyManager.Instance.ResolveEnemyMovement();
    }
}
