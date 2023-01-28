using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPanelExample_RegisterActions : MonoBehaviour
{
    void Start()
    {
        CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString);

        CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString); CheatPanel.Instance.RegisterAction(DebugLogNoParameter);
        CheatPanel.Instance.RegisterAction<int>(DebugLogInt);
        CheatPanel.Instance.RegisterAction<string>(DebugLogString);
    }

    private void DebugLogNoParameter()
    {
        Debug.Log("No parameter");
    }

    private void DebugLogInt(int value)
    {
        Debug.Log(value);
    }

    private void DebugLogString(string value)
    {
        Debug.Log(value);
    }
}
