using System;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanelAction : MonoBehaviour
{
    [SerializeField] protected Text _label;
    [SerializeField] protected Button _button;

    public void SetAction(Action action)
    {
        _label.text = action.Method.Name;
        _button.onClick.AddListener(() => action());
    }
}

public abstract class CheatPanelAction<T> : CheatPanelAction
{
    protected Action<T> _action;

    public void SetAction(Action<T> action)
    {
        _label.text = action.Method.Name;
        _action = action;
        _button.onClick.AddListener(OnClick);
    }

    protected abstract T ConvertParameter();

    protected void OnClick()
    {
        var parameter = ConvertParameter();
        _action(parameter);
    }
}
