using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueController
{
    private QueueRunner _runner;

    private bool _flagDirty = false;
    private bool _isRunning = false;
    private bool _autoStart = true;
    
    private Queue<QueueItem> _queue = new Queue<QueueItem>();
    public bool IsRunning { get => _isRunning; }
    public QueueRunner Runner
    {
        get
        {
            if (_runner == null)
            {
                _runner = new GameObject("QueueRunner").AddComponent<QueueRunner>();
            }
            return _runner;
        }

        set => _runner = value;
    }

    public bool AutoStart { get => _autoStart; set => _autoStart = value; }

    public void ClearQueue()
    {
        if (_runner != null)
        {
            _runner.StopAllCoroutines();
        }
        
        _queue = new Queue<QueueItem>();
        _flagDirty = false;
        _isRunning = false;
    }

    public void Add(IEnumerator coroutine, Action endCoroutineCallback = null, int priority = 50)
    {
        if (!_isRunning)
        {
            var item = new QueueItem(coroutine, endCoroutineCallback, priority);
            _queue.Enqueue(item);
            _queue = new Queue<QueueItem>(_queue.OrderByDescending((a) => a.Priority));

            if (!_flagDirty && _autoStart)
            {
                StartRunner(true);
            }
        }
    }

    public void StartRunner(bool waitForNextFrame)
    {
        _flagDirty = true;
        Runner.StartCoroutine(ExecuteQueuedItems(waitForNextFrame));
    }

    private IEnumerator ExecuteQueuedItems(bool waitForNextFrame)
    {
        if (waitForNextFrame)
        {
            yield return null;
        }

        _isRunning = true;

        while (_queue != null && _queue.Count > 0)
        {
            var item = _queue.Dequeue();
            if (item != null)
            {
                if (item.Coroutine != null)
                {
                    yield return Runner.StartCoroutine(item.Coroutine);
                }

                if (item.Action != null)
                {
                    item.Action.Invoke();
                }
            }
        }

        _isRunning = false;
        _flagDirty = false;
    }
}
