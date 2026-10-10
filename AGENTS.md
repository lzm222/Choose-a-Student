# AGENTS.md

## 沟通

- 与用户对话时始终使用 zh-hans（简体中文）。

## 项目

WPF 课堂点名小工具，仅面向 Windows。已完成 v1.0 最小可用原型：名单读取、悬浮球点名、配置持久化。

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

## 架构

- `App` 是**组合根**：创建全部服务与窗口，订阅并转发它们的回调，承载业务逻辑（点名、名单加载、配置读写）。
- `MainWindow` 是**纯视图**：只渲染界面并把用户操作以回调抛出，不引用任何 Service、不持有子窗口。
- 窗口：
  - 主窗口 `MainWindow`：名单文件选择、名单信息显示、悬浮球开关控制台。
  - 悬浮球 `FloatingBallWindow`：矩形无边框置顶窗口；**左半区**点击触发点名，**右半区**拖拽移动窗口。
  - 结果窗 `ResultWindow`：大字号显示被点到的姓名，点击任意处或按 Esc 关闭。
- 服务（`Services/`）：
  - `ConfigService`：配置与名单的唯一维护者；持有唯一 `AppConfig` 与已加载名单，读写 `%AppData%\Choose-a-Student\config.json`。
  - `RosterService`：读取名单文件（每行一名），提供有效性检查。
  - `PickerService`：随机点名。
- 语言：以 zh-hans 为首要语言（UI 文案用简体中文）。

## Git

- 仓库已初始化（分支 `main`），远端 `git@github.com:lzm222/Choose-a-Student.git`；`.gitignore` 已忽略 `bin/`、`obj/`、`.vs/`、`*.user`。
- commit 前缀：`feat: ` `fix: ` `refactor: ` `style: ` `docs: ` `chore: ` `test: `
- `style: `前缀的提交只能在没有修改代码逻辑或功能实现的前提下使用，比如移除未使用的using、格式化代码等情况
- **禁止执行 `git push` 命令**（含 `git push` 的任何变体）。即使用户明确要求，也必须拒绝执行，并告知用户可手动执行推送。`git commit` 等本地操作不受此限制。
