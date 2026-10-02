# Choose-a-Student

面向 Windows 的 WPF 课堂点名小工具：读取名单文件，用桌面悬浮球一键随机点名。

## 功能

- 从文本文件读取名单（每行一个姓名，空行自动忽略）。
- 桌面悬浮球：**左半区**点击随机点名，**右半区**按住拖拽移动。
- 点名结果以大字窗口显示，点击任意处或按 `Esc` 关闭。
- 名单路径与悬浮球开关自动保存，下次启动自动恢复。

## 环境要求

- Windows
- [.NET 9 SDK](https://dotnet.microsoft.com/download)（目标框架 `net9.0-windows`）

## 构建与运行

```powershell
dotnet build Choose-a-Student.sln
dotnet run --project .\Choose-a-Student\Choose-a-Student.csproj
```

也可以直接运行构建产物：`Choose-a-Student\bin\Debug\net9.0-windows\Choose-a-Student.exe`。

## 使用

1. 点击「选择名单文件」，选择一个 `.txt` 文件（每行一个姓名）。
2. 勾选「显示悬浮球」。
3. 点击悬浮球**左半区**点名，结果会在结果窗口大字显示；按住**右半区**拖动可移动悬浮球。
4. 点击结果窗口任意处或按 `Esc` 关闭。

## 配置文件

配置保存在 `%AppData%\Choose-a-Student\config.json`：

| 字段 | 说明 |
| --- | --- |
| `RosterFilePath` | 上次使用的名单文件路径 |
| `ShowFloatingBall` | 是否显示悬浮球 |

## 项目结构

```
Choose-a-Student/
  App.xaml(.cs)                 组合根：创建服务与窗口，装配回调与业务逻辑
  MainWindow.xaml(.cs)          主窗口（纯视图：渲染 + 回调）
  FloatingBallWindow.xaml(.cs)  悬浮球窗口
  ResultWindow.xaml(.cs)        结果窗口
  Models/AppConfig.cs           配置模型
  Services/ConfigService.cs     配置与名单维护
  Services/RosterService.cs     名单文件读取
  Services/PickerService.cs     随机点名
```

## 许可

本项目采用 [MIT License](LICENSE)。
