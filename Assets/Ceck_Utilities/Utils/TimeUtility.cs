using System;

public static class TimeUtility
{
    public static int timeOffset = 0;

    public static string ConvertTime(this TimeSpan entry)
    {
        entry = entry.Add(new TimeSpan(0, 0, 1));
        var hours = string.Format(entry.Hours > 0 ? "{0}h" : "", entry.Hours);
        var minutes = string.Format(entry.Minutes > 0 ? "{0}m" : "", entry.Minutes);
        var seconds = string.Format(entry.Seconds > 0 ? "{0}s" : "00s", entry.Seconds);

        return hours + minutes + seconds;
    }

    public static DateTime GetTime()
    {
        return DateTime.Now.AddMinutes(timeOffset);
    }
}
