using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _seconds = 0;

    public float TimeLeft { get => _seconds; set => _seconds = value; }

    void Update()
    {
        _seconds -= Time.deltaTime;
        if (_seconds <= 0)
        {
            Destroy(gameObject);
        }
    }

    public static Timer NewTimer(float seconds, string timerName = "timer")
    {
        var timer = new GameObject("_" + timerName + "_" + seconds).AddComponent<Timer>();
        timer.TimeLeft = seconds;
        return timer;
    }
}
