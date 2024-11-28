using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action OnSceneChangeEvent;
    public static event Action AfterRegisterUIManager;
    public static event Action<string> AfterRecvLLMData;

    public static void CallAfterRegisterUIManager()
    {
        AfterRegisterUIManager?.Invoke();
    }

    public static void CallAfterRecvLLMData(string recv)
    {
        AfterRecvLLMData?.Invoke(recv);
    }
}
