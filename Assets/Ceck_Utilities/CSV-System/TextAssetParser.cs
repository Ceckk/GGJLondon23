using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextAssetParser
{
    public static List<Dictionary<string, string>> ReadAsCSV(TextAsset asset)
    {
        return CSVReader.Read(asset);
    }
}
