using System;
using System.Windows;
using ExplorerTabUtility.Managers;
using ExplorerTabUtility.UI.Views.Controls;

namespace ExplorerTabUtility.UI.Views;

public partial class MainWindow : Window
{
    private readonly HookManager _hookManager;
    private readonly SystemTrayIcon _trayIcon;

    public MainWindow()
    {
        InitializeComponent();

        _hookManager = new HookManager();
        _trayIcon = new SystemTrayIcon(_hookManager);

        StartHooks();

        Application.Current.Exit += OnApplicationExit;
    }

    private void StartHooks()
    {
        if (SettingsManager.IsWindowHookActive) _hookManager.StartWindowHook();
        _hookManager.SetReuseTabs(SettingsManager.ReuseTabs);
    }

    private void OnApplicationExit(object sender, ExitEventArgs e)
    {
        _trayIcon.Dispose();
        _hookManager.Dispose();
    }
}
