using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class QueueItem
{
    private int _priority;
    private IEnumerator _coroutine;
    private Action _action;

    public int Priority { get => _priority; }
    public IEnumerator Coroutine { get => _coroutine; }
    public Action Action { get => _action; }

    public QueueItem(IEnumerator coroutine, Action action, int priority)
    {
        _coroutine = coroutine;
        _action = action;
        _priority = priority;
    }
}
