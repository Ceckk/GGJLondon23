using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConversionUtility
{
    public static int ConvertToInt(this string entry, int defaultValue = 0)
    {
        if (int.TryParse(entry, out int value))
        {
            return value;
        }
        else
        {
            LogManager.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }

    public static float ConvertToFloat(this string entry, float defaultValue = 0)
    {
        if (float.TryParse(entry, out float value))
        {
            return value;
        }
        else
        {
            LogManager.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }

    public static bool ConvertToBool(this string entry, bool defaultValue = false)
    {
        if (bool.TryParse(entry, out bool value))
        {
            return value;
        }
        else
        {
            LogManager.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }
}
