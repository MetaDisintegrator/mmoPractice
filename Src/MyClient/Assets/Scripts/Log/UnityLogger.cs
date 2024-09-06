﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using log4net;

/// <summary>
/// <para>用来将Unity日志也输出到log4net中</para>
/// <para>使用Init()方法来开启功能</para>
/// </summary>
public static class UnityLogger
{

    public static void Init()
    {
        Application.logMessageReceived += onLogMessageReceived;
        Common.Log.Init("Unity");
    }

    private static ILog log = LogManager.GetLogger("Unity");

    private static void onLogMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
    {
        switch(type)
        {
            case LogType.Error:
                log.ErrorFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Assert:
                log.DebugFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Exception:
                log.FatalFormat("{0\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Warning:
                log.WarnFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            default:
                log.Info(condition);
                break;
        }
    }
}