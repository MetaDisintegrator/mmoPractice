using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public enum UIResult
    {
        None = 0,
        Yes = 1,
        No = 2,
    }

    public delegate void CloseHandler(UIWindow sender, UIResult result);
    public event CloseHandler closeHandler;

    public Type Type => this.GetType();

    private void Close(UIResult result = UIResult.None)
    { 
        UIManager.Instance.Close(Type);
        closeHandler?.Invoke(this, result);
        closeHandler = null;
    }

    public void OnClose() => Close();

    public void OnYesClicked() => Close(UIResult.Yes);
}
