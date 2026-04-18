# Oboro Project Context

> 目标：为未来重启会话提供低负担、低 token 的项目恢复材料。先读根目录 `copilot-instructions.md`；只有在需要更多上下文时再读本文。

## 1. 项目定位

- 项目名：`Oboro`，中文名：`朧`
- 历史名：`SDFSample`
- Unity 版本：`2023.2.22f1`
- Git 远端：`git@github.com:onovich/Oboro.git`
- 包目录：`Assets/com.mortise.oboro`
- README 已按 Choir 风格重写，项目当前对外定位是：
  - 一个可复用的 Unity SDF / 标量场等高线运行时
  - 一个可直接运行的动态视觉 Sample

## 2. 文档架构

建议未来会话按下面顺序读取：

1. `copilot-instructions.md`
2. 本文（仅在需要更多背景时）
3. `docs/assistant/validation-skill.md`（仅在需要批处理验证命令或 smoke 测试时）
4. `docs/root-rename-session-handoff.md`（仅在排查本地路径/改名遗留时）

这样可以避免每次都把历史、架构、路径迁移信息一次性读完。

## 3. 业务与产品语义

### 3.1 Oboro 想表达什么

- “朧”对应边界模糊、月光朦胧、轮廓融合的视觉气质。
- 项目核心不是复杂编辑器系统，而是运行时轮廓生成与一个能快速体现气质的演示样例。
- 当前 Sample 主要展示：
  - 动态背景扰动
  - 多个软障碍体叠加形成的标量场
  - 多级等高线提取与绘制
  - 鼠标拖拽障碍体时的实时反馈

### 3.2 当前产品阶段

- 更偏“可复用 runtime + 演示 sample”的轻量包。
- 还没有进入“重内容化的样例工程”阶段。
- 因此很多设计决策优先级是：
  - 显式场景入口
  - 结构清楚
  - 易于继续迭代
  - 少配置、少样板

## 4. 架构风格

### 4.1 总体分层

- `MortiseFrame.Oboro`
  - 可复用运行时
  - 负责标量场网格、轮廓提取、轮廓绘制等通用能力
- `MortiseFrame.Oboro.Sample`
  - 演示层
  - 负责参数、障碍布局、互动逻辑、自动启动体验
- `*.Inside`
  - 内部实现细节
  - 不是对外 API 扩展点

### 4.2 Runtime 角色划分

- `ContourCore`
  - 维护屏幕尺寸、网格尺寸、field grid
  - 组织 BuildField / EmitContour 流程
- `ContourRendererCore`
  - 负责 GL 线段绘制与材质生命周期
- `ContourCoreContext`
  - 保存运行时内部状态
- `ContourGridFactory`
  - 根据 evaluator 生成标量场采样网格
- `ContourMarchingSquaresFactory`
  - 根据标量场与 level 集合发射轮廓线段
- `ContourLevelModel`
  - 封装 contour level 与颜色

### 4.3 Sample 角色划分

- `OboroSampleEntry`
  - 真正的示例运行入口
  - 串联 runtime / gpu sample renderer、field、interaction、camera 与 render 生命周期
- `OboroSampleFieldCore`
  - 管理障碍体与 contour level 配置
  - 定义 sample 的标量场求值逻辑
- `OboroSampleInteractionController`
  - 处理拖拽交互
- `OboroSampleFactory`
  - 集中放置 sample 常量、默认 obstacle 和 contour 配置
- `OboroSampleObstacleModel`
  - 障碍体数据模型与基本布局/命中逻辑

### 4.4 当前不建议做的架构动作

- 不要为了“看起来更完整”就引入复杂配置系统。
- 不要把 runtime 与 sample 的概念边界混在一起。
- 不要重新引入自动 Bootstrap 来隐式创建入口对象。
  - 当前约定是由场景显式挂载 `OboroSampleEntry`
  - 这样更利于上层用户测试、场景控制与 smoke 验证
- Sample 当前默认优先使用 GPU 全屏 pass，以降低 DrawCall 之外的 CPU 轮廓提取开销。
  - CPU contour 路径仍保留，作为 Runtime 能力与 fallback

## 5. 代码编码风格

### 5.1 总体风格

- 偏简洁、直接、低抽象负担。
- 以“小类 + 清晰职责 + 顺序流程”为主。
- 优先可读性，而不是模式化或框架化设计。
- 新代码应尽量和现有文件保持相似密度与语气。

### 5.2 命名与结构习惯

- 类型、方法：`PascalCase`
- 字段、局部变量：`camelCase`
- Namespace：按边界分为
  - `MortiseFrame.Oboro`
  - `MortiseFrame.Oboro.Sample`
  - `MortiseFrame.Oboro.*.Inside`
- 示例相关命名统一以 `OboroSample*` 开头，避免污染 runtime 概念。
- 内部辅助类偏向 `internal` 或放在 `Inside` 命名空间中。

### 5.3 实现偏好

- 方法通常较短，职责集中。
- 倾向显式流程控制，如早返回、清晰分支、有限状态变量。
- 常量和默认参数集中在 factory/配置角色里，而不是分散硬编码到多个类。
- 不为小问题过度抽象接口或引入依赖注入。
- 如果只是 Sample 使用的逻辑，不要提前提升为 Runtime API。
- 对测试与验证入口，优先提供可脚本化、批处理友好的方案。

### 5.4 修改代码时的判断规则

优先问自己：

1. 这是 runtime 责任还是 sample 责任？
2. 这是通用能力还是仅演示需要？
3. 当前改动会不会破坏“开箱即跑”？
4. 有没有比新增抽象更简单的实现方式？

## 6. Git 约定

- 默认远端：`git@github.com:onovich/Oboro.git`
- 主分支：`main`
- 提交信息是强约束，不再只是建议。
- 必须使用以下格式之一：
  - `<type>: <summary>`
  - `<type>(<scope>): <summary>`
- 合法 `type`：`feat`、`fix`、`refactor`、`perf`、`docs`、`test`、`chore`、`ci`、`revert`
- `scope` 可选，但如果写了，必须使用小写单词或短横线，例如：`sample`、`runtime`、`validation`、`render`
- `summary` 要求：
  - 使用英文短句
  - 首字母小写
  - 聚焦单一结果
  - 通常不超过 72 个字符
- 禁止：
  - `wip`
  - `update stuff`
  - `misc fixes`
  - 一个 message 同时概括多个互不相关改动
- 例如：
  - `feat: add batch smoke validation`
  - `refactor(sample): remove bootstrap entry`
  - `fix(render): align gpu sample hit area`
- 不使用本地 git hook 强制提交格式；这里更偏向由 AI agent 在提交前做工作流自检。
- 提交前流程固定为：
  1. 先看 `git status --short`
  2. 确认 staged diff 只对应一个主题
  3. 由 agent 先确定一个符合规则的 commit message
  4. 如果变更仍混杂多个主题，先拆分而不是硬写一个笼统 message
  5. 如需验证 Unity，先 compile，再 smoke，且串行执行

## 7. 验证工作流约定

- Unity 无头验证必须串行执行，不能并行跑多个 batchmode 进程。
- 推荐顺序：先 compile，再 smoke。
- 如果当前项目已在 Unity Editor 中打开，优先关闭 Editor 后再进行 batchmode 验证。

## 8. 当前已确认状态（2026-04-18）

- Oboro 命名替换已经完成。
- 根目录已从 `SDFSample` 改为 `D:/UnityProjects/Oboro`。
- 已复验 Unity 批处理编译通过。
- 已跟踪源码内未发现需要继续处理的旧项目名/旧路径残留。
- 旧路径文本主要存在于 Unity 再生成缓存、布局和历史日志中，通常不是源码层问题。
- Sample 入口已改为显式场景挂载，不再使用 `OboroSampleBootstrap`。

## 9. 后续会话接手建议

如果未来会话要继续推进，优先考虑以下方向：

- 完善 Sample 的正式演示形态（例如 `Samples~`、示例场景、标准化分发）
- 在不破坏现有轻量结构的前提下补文档或用例
- 增强 Runtime 的可复用性时，保持与 Sample 解耦

如果只是普通修复或小功能，通常无需再读完整历史；读完 `copilot-instructions.md` 即可开始工作。
