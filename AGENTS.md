# AGENTS.md

## 沟通

- 与用户对话时始终使用 zh-hans（简体中文）。

## 项目

WPF 课堂点名小工具，仅面向 Windows。当前处于**初始模板阶段**：`MainWindow` 为空壳，尚未实现名单读取、悬浮球、配置持久化等任何功能。

## 构建与运行

```powershell
dotnet build Choose-a-Student.sln
dotnet run --project .\Choose-a-Student\Choose-a-Student.csproj
```

- 目标框架 `net9.0-windows`（WPF），与本机安装的 .NET 9 SDK 一致。
- 没有测试项目、没有 lint / 格式化配置；改完代码用 `dotnet build` 验证即可。

## 命名陷阱（易错）

- 目录 / 项目 / sln 名用连字符：`Choose-a-Student`。
- 但根命名空间是下划线形式：`Choose_a_Student`（见 `.csproj` 的 `RootNamespace`）。
- 新建 `.cs` / `.xaml` 时命名空间必须写 `Choose_a_Student`，`x:Class` 同理，否则编译失败。

## 目标架构（用户约定，尚未实现）

- 主窗口：标题、名单文件选择、悬浮球开关控制台。
- 悬浮球：矩形、无边框置顶窗口；**左半区**响应点击触发选人，**右半区**响应拖拽移动窗口。
- 名单：从用户指定路径读取并保存到配置文件，供后续复用。
- 语言：以 zh-hans 为首要语言（新 UI 文案用简体中文；涉及资源/区域性时显式设置 zh-Hans）。

## Git

- 仓库已初始化（分支 `main`），`.gitignore` 已忽略 `bin/`、`obj/`、`.vs/`、`*.user`。
- commit 前缀：`feat: ` `fix: ` `refactor: ` `style: ` `docs: ` `chore: ` `test: `
- message 尽量简短；每完成一个小功能立即提交一次。
