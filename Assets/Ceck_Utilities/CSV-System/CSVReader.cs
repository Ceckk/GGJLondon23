using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Based on https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/

public class CSVReader
{
    private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private static char[] TRIM_CHARS = { '\"' };

    private static string START_READING = "_start";
    private static string STOP_READING = "_stop";

    public static List<Dictionary<string, string>> Read(TextAsset data)
    {
        var list = new List<Dictionary<string, string>>();

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        var read = false;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length > 0 && (read || values[0] == START_READING))
            {
                read = true;

                var entry = new Dictionary<string, string>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    entry[header[j]] = value;
                }
                list.Add(entry);

                if (values[0] == STOP_READING)
                {
                    read = false;
                }
            }
        }
        return list;
    }
}
