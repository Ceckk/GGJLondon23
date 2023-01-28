using UnityEngine;
using DataSystem;
using System;

public class LogManager
{
    public static void LogWarning(object message = null)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(object message = null)
    {
        Debug.LogError(message);
    }

    public static void LogException(Exception e)
    {
        Debug.LogException(e);
    }

    public static void Log(object message = null, Color? color = null, int stackTraceIndex = 1)
    {
        var stacktrace = UnityEngine.StackTraceUtility.ExtractStackTrace();
        var stack = stacktrace.Split('\n');

        if (stack.Length > stackTraceIndex)
        {
            var s = stack[stackTraceIndex];

#if UNITY_EDITOR
            var index = s.LastIndexOf('(');
            // var location = s.Substring(index, s.Length - index);
            var classAndMethodString = s.Substring(0, index - 1);
#else
            var classAndMethodString = s;
#endif

            if (message != null)
            {
                if (color != null)
                {
                    Debug.Log(string.Format("<color=#{0}>{1}:{2}</color>",
                        ColorUtility.ToHtmlStringRGB((Color)color),
                        classAndMethodString,
                        message.ToString()));
                }
                else
                {
                    Debug.Log(string.Format("{0}:{1}",
                        classAndMethodString,
                        message.ToString()));
                }
            }
            else
            {
                if (color != null)
                {
                    Debug.Log(string.Format("<color=#{0}>{1}</color>",
                        ColorUtility.ToHtmlStringRGB((Color)color),
                        classAndMethodString));
                }
                else
                {
                    Debug.Log(classAndMethodString);
                }
            }
        }
        else
        {
            if (color != null)
            {
                Debug.Log(string.Format("<color=#{0}>{1}</color>",
                    ColorUtility.ToHtmlStringRGB((Color)color),
                    message));
            }
            else
            {
                Debug.Log(message.ToString());
            }
        }
    }
}
