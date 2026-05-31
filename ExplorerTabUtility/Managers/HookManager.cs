using System;
using ExplorerTabUtility.Hooks;

namespace ExplorerTabUtility.Managers;

public sealed class HookManager : IDisposable
{
    private readonly ExplorerWatcher _windowHook;

    public event Action? OnShellInitialized;

    public HookManager()
    {
        _windowHook = new ExplorerWatcher();
        _windowHook.OnShellInitialized += () => OnShellInitialized?.Invoke();
        System.Windows.Application.Current.SessionEnding += (_, _) => Dispose();
    }

    public void StartWindowHook() => ChangeHookStatus(_windowHook, true);
    public void StopWindowHook() => ChangeHookStatus(_windowHook, false);
    public void SetReuseTabs(bool value) => _windowHook.SetReuseTabs(value);

    private static void ChangeHookStatus(IHook hook, bool isActive)
    {
        if (hook.IsHookActive == isActive) return;

        if (isActive)
            hook.StartHook();
        else
            hook.StopHook();
    }

    public void Dispose()
    {
        _windowHook.Dispose();
    }
}
