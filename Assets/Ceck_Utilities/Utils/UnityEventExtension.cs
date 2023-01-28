using System;
using UnityEngine.Events;

public static class UnityEventExtension
{
    private static LogManager _logger = new LogManager();

    public static void AddOneShotListener(this UnityEvent unityEvent, Action callback)
    {
        UnityAction listener = null;
        listener = () =>
        {
            callback();
            unityEvent.RemoveListener(listener);
        };
        unityEvent.AddListener(listener);
    }
}
