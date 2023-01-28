using UnityEngine;
using UnityEngine.UI;

public class CheatPanelActionInt : CheatPanelAction<int>
{
    [SerializeField] private Text _parameter = null;

    protected override int ConvertParameter()
    {
        return int.Parse(_parameter.text);
    }
}
