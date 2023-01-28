using System;
using UnityEngine;

public class CheatPanel : MonoSingleton<CheatPanel>
{
    [SerializeField] private GameObject _mainPanel = null;
    [SerializeField] private GameObject _dynamicPanel = null;

    [Space, Header("Cheat Action prefabs")]
    [SerializeField] private CheatPanelAction _cheatActionPrefab = null;
    [SerializeField] private CheatPanelActionInt _cheatActionIntPrefab = null;
    [SerializeField] private CheatPanelActionString _cheatActionStringPrefab = null;

    [Space, Header("CheatButton click settings")]
    [SerializeField] private int _buttonClicksRequired = 3;
    [SerializeField] private float _buttonClickTimeLimit = 0.5f;
    private int _buttonClickedCount = 0;
    private float _buttonTimer = 0;

    void Start()
    {
        _mainPanel.SetActive(false);
    }

    void Update()
    {
        if (!_mainPanel.activeSelf)
        {
            if (_buttonClickedCount > 0)
            {
                if (_buttonClickedCount >= _buttonClicksRequired || _buttonTimer >= _buttonClickTimeLimit)
                {
                    if (_buttonClickedCount >= _buttonClicksRequired)
                    {
                        _mainPanel.SetActive(true);
                    }

                    _buttonClickedCount = 0;
                    _buttonTimer = 0;
                }
                else
                {
                    _buttonTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _mainPanel.SetActive(false);
        }
    }

    public void CheatButton_OnClick()
    {
        _buttonClickedCount++;
    }

    public void RegisterAction(Action action)
    {
        var panelAction = GameObject.Instantiate<CheatPanelAction>(_cheatActionPrefab, _dynamicPanel.transform);
        panelAction.SetAction(action);
    }

    public void RegisterAction<T>(Action<T> action)
    {
        switch (action)
        {
            case Action<int> a:
                var panelActionInt = GameObject.Instantiate<CheatPanelActionInt>(_cheatActionIntPrefab, _dynamicPanel.transform);
                panelActionInt.SetAction(a);
                break;
            case Action<string> a:
                var panelActionString = GameObject.Instantiate<CheatPanelActionString>(_cheatActionStringPrefab, _dynamicPanel.transform);
                panelActionString.SetAction(a);
                break;
            default:
                Debug.LogWarning("CheatAction Type not yet implemented");
                break;
        }
    }
}