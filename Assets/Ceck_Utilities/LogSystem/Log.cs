using UnityEngine;

public static class Log
{
    public static string Method
    {
        get
        {
            return GetInfo(null, null, 2);
        }
    }

    // I can use Debug.unityLogger.logEnabled = false; to set log enabled/disabled or even filter them out with Debug.unityLogger.filterLogType = LogType.Error;
    public static string GetInfo(object message = null, Color? color = null, int stackTraceIndex = 1)
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
                    return string.Format("<color=#{0}>{1}:{2}</color>",
                        ColorUtility.ToHtmlStringRGB((Color)color),
                        classAndMethodString,
                        message.ToString());
                }
                else
                {
                    return string.Format("{0}:{1}",
                        classAndMethodString,
                        message.ToString());
                }
            }
            else
            {
                if (color != null)
                {
                    return string.Format("<color=#{0}>{1}</color>",
                        ColorUtility.ToHtmlStringRGB((Color)color),
                        classAndMethodString);
                }
                else
                {
                    return classAndMethodString;
                }
            }
        }
        else
        {
            if (color != null)
            {
                return string.Format("<color=#{0}>{1}</color>",
                    ColorUtility.ToHtmlStringRGB((Color)color),
                    message);
            }
            else
            {
                return message.ToString();
            }
        }
    }
}
