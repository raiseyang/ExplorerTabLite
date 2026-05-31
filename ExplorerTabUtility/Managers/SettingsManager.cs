using System;
using System.IO;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExplorerTabUtility.Helpers;

namespace ExplorerTabUtility.Managers;

public static class SettingsManager
{
    private static readonly AppSettings Settings;
    public static event EventHandler<PropertyChangedEventArgs>? StaticPropertyChanged;

    private static readonly string SettingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        Constants.AppName,
        Constants.SettingsFileName);

    static SettingsManager()
    {
        var directory = Path.GetDirectoryName(SettingsFilePath);
        Directory.CreateDirectory(directory!);

        if (!File.Exists(SettingsFilePath))
        {
            Settings = new AppSettings();
            return;
        }

        try
        {
            var json = File.ReadAllText(SettingsFilePath);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch (Exception)
        {
            Settings = new AppSettings();
        }
    }

    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "")
    {
        StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }

    public static bool IsWindowHookActive
    {
        get => Settings.WindowHook;
        set
        {
            Settings.WindowHook = value;
            SaveSettings();
            NotifyStaticPropertyChanged();
        }
    }

    public static bool ReuseTabs
    {
        get => Settings.ReuseTabs;
        set
        {
            Settings.ReuseTabs = value;
            SaveSettings();
            NotifyStaticPropertyChanged();
        }
    }

    public static bool HaveThemeIssue
    {
        get => Settings.HaveThemeIssue;
        set
        {
            Settings.HaveThemeIssue = value;
            SaveSettings();
        }
    }

    public static void SaveSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(Settings);
            File.WriteAllText(SettingsFilePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
        }
    }
}

internal class AppSettings
{
    public bool WindowHook { get; set; } = true;
    public bool ReuseTabs { get; set; } = true;
    public bool HaveThemeIssue { get; set; }
}
