# Explorer Tab Lite

> Lightweight tool that forces new File Explorer windows to open as tabs in Windows 11.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Windows 11](https://img.shields.io/badge/Windows%2011-22H2+-blue.svg)](https://www.microsoft.com/windows/windows-11)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download)

> **Requires**: Windows 11 22H2 (Build 22621) or later with File Explorer Tabs enabled.

## What it does

When you open a new folder (from desktop, another app, or "Show in folder"), instead of spawning a separate window, it automatically becomes a **new tab** in your existing Explorer window.

## Features

- **Window to Tab** - New Explorer windows are automatically converted to tabs
- **File Selection Preserved** - "Show in folder" / "Open file location" keeps the file highlighted after converting to tab
- **Tab Reuse** - If the path is already open in a tab, switches to it instead of creating a duplicate
- **Ctrl+Shift Bypass** - Hold Ctrl+Shift when opening a folder to force a new window (skip conversion)
- **Explorer Crash Recovery** - Automatically reconnects when explorer.exe restarts
- **System Tray** - Runs silently with a right-click menu to toggle features

## How to Use

### Install

1. Download `ExplorerTabLite.exe` from [Releases](https://github.com/raiseyang/ExplorerTabLite/releases)
2. Put it anywhere you like (e.g. `C:\Tools\`)
3. Run it - a system tray icon appears

### Or build from source

```powershell
git clone https://github.com/raiseyang/ExplorerTabLite.git
cd ExplorerTabLite

# Generate COM interop DLLs (one-time, requires Windows SDK)
tlbimp "C:\Windows\System32\ieframe.dll" /out:"ExplorerTabUtility\lib\Interop.SHDocVw.dll" /namespace:SHDocVw /machine:Agnostic
tlbimp "C:\Windows\System32\shell32.dll" /out:"ExplorerTabUtility\lib\Interop.Shell32.dll" /namespace:Shell32 /machine:Agnostic

# Build
dotnet build ExplorerTabUtility\ExplorerTabUtility.csproj -c Release

# Or publish as single-file exe
dotnet publish ExplorerTabUtility\ExplorerTabUtility.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true -o publish
```

### System Tray Menu (right-click the icon)

| Option | Description |
|--------|-------------|
| **Window Hook** | Toggle auto window-to-tab conversion (on by default) |
| **Reuse Tabs** | Toggle tab reuse for duplicate paths (on by default) |
| **Add to startup** | Auto-start with Windows |
| **I have theme issues** | Use alternative window hiding method for custom Explorer themes |
| **Exit** | Quit the application |

### Daily Usage

Once running, just use your computer normally:

- **Open a folder** from Desktop/other apps -> automatically becomes a tab
- **"Show in folder"** from Chrome/other apps -> opens as tab with file selected
- **Same folder again** -> switches to existing tab (if Reuse Tabs is on)
- **Want a separate window?** -> Hold `Ctrl+Shift` while opening

### Auto-Start on Boot

Right-click tray icon -> check "Add to startup". Done.

### Uninstall

1. Right-click tray icon -> Exit
2. Delete the exe file
3. (Optional) Remove registry key: `HKCU\Software\Microsoft\Windows\CurrentVersion\Run\ExplorerTabLite`
4. (Optional) Delete settings: `%APPDATA%\ExplorerTabLite\`

## How it Works

1. Monitors `ShellWindows` COM events to detect new Explorer windows
2. Uses `WinEvent Hook` (EVENT_OBJECT_SHOW) as a safety net to catch windows before they render
3. Hides the new window instantly (transparent or off-screen)
4. Creates a new tab in the existing Explorer window via undocumented `WM_COMMAND` (0xA21B)
5. Navigates the new tab to the target path
6. Restores file selection if applicable
7. Closes the hidden original window

  ---
  .NET 运行时说明

  本程序依赖 .NET 9.0 Desktop Runtime。如果双击 exe
  无法启动或报错提示缺少运行时，请通过以下命令安装：

  winget install Microsoft.DotNet.DesktopRuntime.9

  注意：安装的是 Desktop Runtime（而非 SDK），安装后 dotnet 命令不会出现在 PATH
  中，这是正常现象。只要 winget install 提示"已安装"或安装成功，即可直接运行
  exe，无需额外配置环境变量。

  ---

## Credits

Trimmed from [ExplorerTabUtility](https://github.com/w4po/ExplorerTabUtility) by [w4po](https://github.com/w4po). Original project has many more features (hotkeys, tab search, duplicate/reopen tabs, etc). This version keeps only the core window-to-tab conversion for minimal footprint.

## License

[MIT](LICENSE)
