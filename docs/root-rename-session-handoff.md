# Oboro 本地根目录改名与会话恢复指南

## 目的

当前 Unity 项目的仓库内品牌、包名、程序集名、README 与远端仓库地址都已经完成 Oboro / 朧 的重命名。

当前唯一尚未变更的是 **本地工程根目录名**：
- 当前路径：`D:/UnityProjects/SDFSample`
- 目标路径：`D:/UnityProjects/Oboro`

这一步是**文件系统级别的目录重命名**，不是仓库内容层面的重构，因此最大的风险不是代码，而是：
- 当前 VS Code / Copilot session 绑定旧路径，重命名后会话上下文可能中断。
- Unity Hub、VS Code 最近项目记录、终端当前工作目录等会指向旧路径。

## 当前项目状态

在执行根目录改名前，项目已经处于以下状态：
- Unity 项目 `productName` 已改为 `Oboro`
- 包目录已改为 `Assets/com.onovich.oboro`
- 运行时程序集已改为 `Onovich.Oboro`
- Sample 程序集已改为 `Onovich.Oboro.Sample`
- 示例入口组件为 `OboroSampleEntry`
- 示例入口场景为 `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`
- 根 README 已改为 Oboro 文案
- Git 远端已改为：`git@github.com:onovich/Oboro.git`
- 本地已经成功推送到远端 `origin/main`

最近两次关键提交：
- `20e79b5`：完成 Oboro 重命名、README、编译验证
- `4da06e9`：更新 README 与 package.json 中的仓库链接到 `Oboro.git`

## 为什么这一步建议手动执行

不建议在当前 Copilot 会话中直接由代理去改 workspace 根目录名，原因是：
- 当前 workspace 就绑定在该路径上
- 改名后编辑器上下文会被切断
- 会影响当前终端、打开文件、索引与 Copilot 会话状态

因此，最稳妥的方式是：
1. 先记录当前上下文
2. 由你手动重命名根目录
3. 重新打开新路径
4. 在新 session 中使用本文末尾的恢复提示词，让 Copilot 快速接上上下文

## 执行前检查

在改名前，先确认以下事项：
- `git status` 工作区干净
- 当前分支已同步到远端
- Unity Editor 已关闭
- VS Code 当前窗口准备关闭
- 没有外部程序锁住该目录

建议你额外记录以下信息：
- 当前本地路径：`D:/UnityProjects/SDFSample`
- 目标路径：`D:/UnityProjects/Oboro`
- Git 远端：`git@github.com:onovich/Oboro.git`
- 当前主分支：`main`

## 推荐的改名步骤

### 1. 关闭相关程序

完全关闭以下程序：
- Unity Editor
- 当前 VS Code 窗口
- 可能占用该目录的资源管理器预览、终端、同步工具

Unity Hub 不一定必须关闭，但如果它正在扫描项目，最好让它处于空闲状态。

### 2. 手动重命名根目录

在文件资源管理器中，将目录：
- `D:/UnityProjects/SDFSample`

重命名为：
- `D:/UnityProjects/Oboro`

### 3. 从新路径重新打开项目

使用 Unity Hub 或 Unity Editor 打开：
- `D:/UnityProjects/Oboro`

然后使用 VS Code 打开：
- `D:/UnityProjects/Oboro`

### 4. 等待 Unity 完成刷新

首次从新路径打开时，Unity 可能会重新生成或刷新：
- `.sln`
- `.csproj`
- 局部缓存索引
- 编辑器外部工具工程引用

通常不需要删除 `Library`，除非 Unity 明确报出路径异常且无法自愈。

### 5. 做最小验证

在新路径下验证以下内容：
- Unity 项目能正常打开
- 控制台没有新的脚本编译错误
- 示例能正常启动
- `git status` 仍然干净
- `git remote -v` 仍然指向 `git@github.com:onovich/Oboro.git`

## 改名后建议检查的遗留项

以下问题通常不在 Git 内，而是在本机工具层：
- Unity Hub 项目列表仍显示旧路径
- VS Code 最近打开项目仍显示旧路径
- 终端默认路径或脚本里仍写着 `D:/UnityProjects/SDFSample`
- 你自己的快捷方式、批处理、截图输出目录、笔记链接仍指向旧路径

仓库内容本身通常不会因为根目录改名而产生 Git diff，因为 Git 不跟踪仓库外层目录名。

## 关于 Sample 的两个脚本

当前 Sample 的入口约定已经收敛为**场景显式挂载**：

### `OboroSampleEntry`

这是**真正的示例运行组件**，负责：
- 初始化 `ContourCore`
- 初始化 `ContourRendererCore`
- 初始化 SDF 场与交互控制器
- 驱动 `Update()` 中的时间与拖拽逻辑
- 在 `OnRenderObject()` 中实际绘制轮廓
- 兜底创建或配置相机

因此：
- 真正的业务入口只有一个：`OboroSampleEntry`
- 推荐从 `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity` 进入示例
- 不再依赖自动 Bootstrap，以免干扰上层用户测试

## 新 session 恢复提示词

下面这段可以直接复制到新的 Copilot 会话中：

---

我刚把本地 Unity 工程根目录从 `D:/UnityProjects/SDFSample` 手动改名为 `D:/UnityProjects/Oboro`，请基于以下既有上下文继续工作，不要重复做已完成事项：

- 这是一个 Unity 2023.2.22f1 项目。
- 项目原名是 SDFSample，现已重命名为 Oboro，中文名为朧。
- 仓库远端已改为：`git@github.com:onovich/Oboro.git`。
- 包目录已改为：`Assets/com.onovich.oboro`。
- Runtime 程序集为：`Onovich.Oboro`。
- Sample 程序集为：`Onovich.Oboro.Sample`。
- 项目 README 已按 Choir 风格重写。
- `OboroSampleEntry` 是实际示例组件，默认入口场景是 `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`。
- 之前已经完成 Oboro 命名替换、README 更新、Unity 批处理编译验证，以及推送到远端。
- 关键提交包括：`20e79b5` 与 `4da06e9`。

请先帮我做以下检查：
1. 搜索仓库中是否还有旧的本地路径或旧项目名残留。
2. 验证 Unity 项目在新路径下是否仍能正常编译。
3. 检查是否有必要把 Sample 的 `Bootstrap` 机制进一步重构为 Scene/Preset 方式。

---

## 新 session 首轮建议动作

建议新 session 首先执行：
- 搜索 `SDFSample`
- 搜索 `D:/UnityProjects/SDFSample`
- 搜索 `com.onovich.sdf`
- 运行 Unity 批处理编译验证
- 查看 `git status`

如果这些都正常，就说明本地根目录改名已经完全落稳。
