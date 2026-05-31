using System;
using System.Windows;
using System.Windows.Controls;
using ExplorerTabUtility.Helpers;
using ExplorerTabUtility.Managers;

namespace ExplorerTabUtility.UI.Views.Controls;

public partial class SystemTrayIcon : UserControl, IDisposable
{
    private readonly HookManager _hookManager;

    public SystemTrayIcon(HookManager hookManager)
    {
        InitializeComponent();

        TrayIcon.Icon = Helper.GetIcon();
        TrayIcon.ToolTipText = Constants.NotifyIconText;

        _hookManager = hookManager;
        _hookManager.OnShellInitialized += HookManager_OnShellInitialized;

        WindowHook.Click += (_, _) => ToggleWindowHook();
        ReuseTabs.Click += (_, _) => ToggleReuseTabs();
        AddToStartup.Click += (_, _) => ToggleStartup();
        ThemeIssue.Click += (_, _) => ToggleThemeIssue();
        ExitApplication.Click += (_, _) => Application.Current.Shutdown();
    }

    private void HookManager_OnShellInitialized()
    {
        if (TrayIcon.Visibility == Visibility.Visible) return;
        Helper.DoUntilTimeEnd(HideTrayIcon, 7000, 1000);
        return;

        void HideTrayIcon()
        {
            TrayIcon.Dispatcher.BeginInvoke(() =>
            {
                if (TrayIcon.Visibility == Visibility.Visible) return;
                TrayIcon.Visibility = Visibility.Hidden;
                TrayIcon.Visibility = Visibility.Collapsed;
            });
        }
    }

    private void ToggleWindowHook()
    {
        SettingsManager.IsWindowHookActive = WindowHook.IsChecked;

        if (WindowHook.IsChecked)
            _hookManager.StartWindowHook();
        else
            _hookManager.StopWindowHook();

        if (!WindowHook.IsChecked && ReuseTabs.IsChecked)
        {
            ReuseTabs.IsChecked = false;
            SettingsManager.ReuseTabs = false;
            _hookManager.SetReuseTabs(false);
        }
    }

    private void ToggleReuseTabs()
    {
        SettingsManager.ReuseTabs = ReuseTabs.IsChecked;
        _hookManager.SetReuseTabs(ReuseTabs.IsChecked);

        if (ReuseTabs.IsChecked && !WindowHook.IsChecked)
        {
            WindowHook.IsChecked = true;
            SettingsManager.IsWindowHookActive = true;
            _hookManager.StartWindowHook();
        }
    }

    private void ToggleStartup()
    {
        RegistryManager.ToggleStartup();
        AddToStartup.IsChecked = RegistryManager.IsStartupEnabled;
    }

    private void ToggleThemeIssue()
    {
        SettingsManager.HaveThemeIssue = ThemeIssue.IsChecked;
    }

    public void Dispose()
    {
        TrayIcon.Dispose();
        GC.SuppressFinalize(this);
    }
}
