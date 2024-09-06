using Common.Data;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTestManager : MonoSingleton<NpcTestManager>
{
    protected override void OnStart()
    {
        NpcManager.Instance.RegisterNpcAction(NpcFunction.InvokeShop, OpenTestUI);
    }

    private bool OpenTestUI(NpcDefine define)
    {
        UITest ui = UIManager.Instance.Show<UITest>();
        ui.SetTitle(define.Name+"╣дил╣Й");
        return true;
    }
}
