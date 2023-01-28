using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoSingleton<QueueManager>
{
    private QueueController _controller;

    private QueueController Controller
    {
        get
        {
            if (_controller == null)
            {
                _controller = new QueueController();
            }

            return _controller;
        }
    }

    void OnDestroy()
    {
        ClearQueue();
    }

    public void ClearQueue()
    {
        StopAllCoroutines();

        if (_controller != null)
        {
            _controller.ClearQueue();
        }
    }

    public void Add(Action action, int priority = 50)
    {
        Add(null, action, priority);
    }

    public void Add(IEnumerator coroutine, Action endCoroutineCallback = null, int priority = 50)
    {
        if (Controller.IsRunning)
        {
            //_logger.LogError("Queue already running, cant add more");
            Debug.Log("Queue already running, cant add more");
        }
        else
        {
            Controller.Add(coroutine, endCoroutineCallback, priority);
        }
    }
}
