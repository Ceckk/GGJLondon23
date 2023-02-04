using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TicksManager : MonoBehaviour
{
    [SerializeField] private float _timePerSimpleTick = 1;
    [SerializeField] private float _timePerSpecialTick = 2;
    [SerializeField] private float _numberOfTicksBeforeSpecialTick = 3;

    [SerializeField] private AudioClip _simpleTickSound;
    [SerializeField] private AudioClip _specialTckSound;
    [SerializeField] private AudioSource _audioSource;

    public class OnSimpleTick : IEvent {}
    public class OnSpecialTick : IEvent {}

    private int _index = 0;

    void Start()
    {
        StartCoroutine(TicksUpdate());
    }

    private IEnumerator TicksUpdate()
    {
        while(true)
        {
            for (int i = 0; i < _numberOfTicksBeforeSpecialTick; i++)
            {
                _audioSource.clip = _simpleTickSound;
                _audioSource.Play();
                EventAggregator.Instance.Dispatch<OnSimpleTick>();
                yield return new WaitForSeconds(_timePerSimpleTick);
            }

            _audioSource.clip = _specialTckSound;
            _audioSource.Play();
            EventAggregator.Instance.Dispatch<OnSpecialTick>();
            yield return new WaitForSeconds(_timePerSpecialTick);
        }
    }
}
