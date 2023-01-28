using System;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanelActionString : CheatPanelAction<String>
{
    [SerializeField] private Text _parameter = null;

    protected override string ConvertParameter()
    {
        return _parameter.text;
    }
}
