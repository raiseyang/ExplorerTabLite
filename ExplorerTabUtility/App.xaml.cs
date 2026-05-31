using System.Windows;
using System.Threading;
using ExplorerTabUtility.UI.Views;
using ExplorerTabUtility.Helpers;

namespace ExplorerTabUtility;

public partial class App : Application
{
    private Mutex? _mutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new Mutex(true, Constants.MutexId, out var createdNew);

        if (createdNew)
        {
            base.OnStartup(e);
            _ = new MainWindow();
            return;
        }

        MessageBox.Show("Another instance is already running.\nCheck in System Tray Icons.",
            Constants.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mutex?.Dispose();
        base.OnExit(e);
    }
}
