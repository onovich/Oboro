# Oboro Performance Testing Skill

> 用途：在继续做 CPU / GPU / DrawCall / GC 优化前，先建立可复用、可对比的性能测试口径。

## 适用范围

- Unity 版本：`2023.2.22f1`
- 示例入口场景：`Assets/com.mortise.oboro/Resources_Sample/SampleEntry.unity`
- 当前默认渲染路径：Sample 优先使用 GPU 全屏 pass，CPU contour 路径保留为 fallback

## 1. 测试前先固定变量

为了让优化前后数据可比，先尽量固定这些条件：

- 使用同一个场景：`SampleEntry.unity`
- 使用同一分辨率与 Game 视图缩放
- 测试时关闭额外编辑器窗口，避免额外波动
- 不要同时开 Deep Profile
- 同一轮对比里，不要一会儿测 Editor，一会儿测 Player
- 记录当前 commit hash，避免后面对不上代码版本

推荐至少固定两种观察状态：

1. **Idle**：进入 PlayMode 后不拖拽，等待 5~10 秒稳定后记录
2. **Drag**：持续拖拽一个或多个 obstacle 5~10 秒后记录

## 2. 最快的第一层：Game 视图 Stats

用途：快速看渲染层面的粗指标。

步骤：

1. 打开 `SampleEntry.unity`
2. 进入 PlayMode
3. 打开 Game 视图右上角 `Stats`
4. 分别在 **Idle** 和 **Drag** 状态记录：
   - FPS
   - Frame time
   - Batches
   - SetPass calls
   - Tris / Verts

适合回答的问题：

- DrawCall 是否明显下降
- GPU 全屏 pass 是否把批次数压到预期范围
- 拖拽时帧时间是否明显抖动

注意：

- Game 视图 Stats 很快，但它不是最精确的 CPU/GC 来源分析工具。
- 如果 VSync 锁帧，FPS 可能不敏感，此时优先看 `Frame time`。

## 3. 第二层：Profiler 看 CPU / GC / GPU

用途：确认瓶颈到底在 CPU、GC、Rendering 还是 GPU。

推荐模块：

- `CPU Usage`
- `GPU Usage`
- `Rendering`
- `Memory`

步骤：

1. 打开 `Window > Analysis > Profiler`
2. 关闭 `Deep Profile`
3. 进入 PlayMode
4. 先记录 **Idle** 5~10 秒
5. 再记录 **Drag** 5~10 秒
6. 在 CPU Usage 中重点看：
   - `PlayerLoop`
   - `BehaviourUpdate`
   - `Camera.Render`
   - `GC.Alloc`
7. 在 Rendering / GPU Usage 中重点看：
   - 每帧渲染时间
   - Draw Calls / Batches
   - 是否存在拖拽时的尖峰

建议重点记录：

- CPU Frame Time（平均值 / 峰值）
- GPU Frame Time（平均值 / 峰值）
- GC Alloc / frame
- Batches / SetPass Calls
- Drag 时是否有 spike

## 4. 第三层：Frame Debugger 看 DrawCall 结构

用途：确认每一帧到底画了几次、每次在画什么。

步骤：

1. 打开 `Window > Analysis > Frame Debugger`
2. 在 PlayMode 中启用 Frame Debugger
3. 查看 Sample 绘制阶段的事件数
4. 对比优化前后：
   - 是否仍只有一次主 contour pass
   - 是否出现额外材质切换
   - 是否有意外的重复绘制

适合回答的问题：

- GPU 方案是否真的把 draw call 压低了
- 有没有被无意中拆成多次 pass
- 当前瓶颈是否来自 draw call，而不是 shader 本身

## 5. 第四层：Memory Profiler / GC 观察

用途：确认是否还有持续分配或拖拽时瞬时分配。

最低限度先用普通 Profiler 即可：

- 看 `GC.Alloc` 是否长期为 `0 B/frame`
- 在拖拽时观察是否出现周期性分配峰值

如果后续需要更细：

- 再考虑使用 Memory Profiler 包抓快照
- 但对当前样例，先把 `GC.Alloc / frame` 压稳，比抓大快照更重要

## 6. 推荐对比表

每次优化前后，至少记录这一张表：

| Build / Commit | Mode | Resolution | FPS / Frame ms | CPU ms | GPU ms | GC.Alloc/frame | Batches | SetPass |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| before | Idle | 1920x1080 |  |  |  |  |  |  |
| before | Drag | 1920x1080 |  |  |  |  |  |  |
| after | Idle | 1920x1080 |  |  |  |  |  |  |
| after | Drag | 1920x1080 |  |  |  |  |  |  |

如果只想先看最核心的三项，至少记：

- `Frame time`
- `GC.Alloc/frame`
- `Batches / SetPass`

## 7. 当前项目最值得先观察什么

对于 Oboro 当前这套实现，建议按这个优先级看：

1. **Drag 时 GC.Alloc 是否仍为 0**
2. **GPU pass 是否真的维持低 Batches / SetPass**
3. **分辨率升高后 GPU Frame Time 是否明显上升**
4. **CPU fallback 路径与 GPU 路径的差距有多大**

因为当前主要优化目标本来就是：

- 降低 DrawCall
- 降低 GC
- 把更多压力转移给 GPU

## 8. Editor 与 Player 的区别

- **Editor Profiler**：最快，适合频繁迭代与相对比较
- **Standalone Player**：更接近真实表现，适合做最终结论

建议流程：

1. 先在 Editor 中快速筛选方向
2. 方向确定后，再做一次 Standalone Player 验证

如果你只是比较“这个提交比上个提交是否更好”，Editor 口径通常已经足够。

## 9. 一次最小可执行测试流程

如果你现在就想开始测，按下面做即可：

1. 打开 `Assets/com.mortise.oboro/Resources_Sample/SampleEntry.unity`
2. 进入 PlayMode
3. 在 Game 视图打开 `Stats`
4. 记录 **Idle** 5 秒时的 `Frame time / Batches / SetPass`
5. 打开 Profiler，记录 **Drag** 5 秒时的 `CPU Usage / GPU Usage / GC.Alloc`
6. 打开 Frame Debugger，确认主 contour 绘制是不是单次 pass
7. 把结果按 commit hash 记到一张对比表中

这样下一轮无论做 shader 优化、buffer 化，还是 CPU fallback 优化，都能清楚看见收益。
