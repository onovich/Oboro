# Oboro
Oboro, a lightweight SDF contour runtime and sample package for Unity, named after 「朧」.<br/>
**Oboro，一个用于 Unity 的轻量级 SDF 等高线运行时与示例包，取名自「朧」。**

Oboro focuses on blurred boundaries, fused shapes, and moonlight-like contour layering.<br/>
**Oboro 聚焦于边界模糊、形体融合，以及类似月色朦胧的轮廓层次表现。**

This repository currently contains two parts:<br/>
**当前仓库主要包含两部分：**
* A reusable runtime for scalar field sampling and contour extraction.<br/>
	**一个可复用的运行时，用于标量场采样与轮廓提取；**
* A runnable sample for dynamic terrain-like contour visuals and obstacle dragging interaction.<br/>
	**一个可直接运行的示例，用于展示动态地形式轮廓视觉与障碍拖拽交互。**

# Readiness
Suitable for 2D scalar field / SDF contour effects, experimental visual sketches, rendering prototypes, and UPM-style reuse in other Unity projects.<br/>
**适用于二维标量场 / SDF 等高线效果、实验性视觉草图、渲染原型，以及以 UPM 方式复用于其他 Unity 项目。**

# Features
## Implemented
* Runtime / Sample separation for clearer reuse and demonstration boundaries.<br/>
	**Runtime / Sample 分层，便于复用与演示分离；**
* Generic scalar field grid construction and contour extraction flow.<br/>
	**通用的标量场网格构建与轮廓提取流程；**
* Multi-level contour rendering based on Marching Squares in the runtime path.<br/>
	**Runtime 路径中基于 Marching Squares 的多级等高线提取；**
* Sample-side GPU fullscreen contour rendering path to minimize CPU / GC overhead.<br/>
	**Sample 侧默认提供 GPU 全屏轮廓渲染路径，以尽量降低 CPU / GC 开销；**
* CPU contour path remains available as fallback and runtime reference implementation.<br/>
	**CPU 轮廓路径仍保留，作为 fallback 与 Runtime 参考实现；**
* Soft-core obstacle field, animated background disturbance, and mouse dragging interaction in the sample.<br/>
	**示例内置软核心障碍场、动态背景扰动与鼠标拖拽交互。**

## Planned
* Additional sample presets / scenes if the package grows toward more curated presentation content.<br/>
	**如果包后续朝更完整的展示内容发展，可继续补充更多预设 / 示例场景；**
* More profiler-driven optimization work for larger field sizes or denser contour content.<br/>
	**如果后续要支持更大场尺寸或更密集轮廓内容，可继续做更偏 Profiler 驱动的性能优化。**

## Not In Plan Yet
* Replacing the reusable runtime API with a GPU-only pipeline.<br/>
	**暂不计划用纯 GPU 管线替代当前可复用的 Runtime API；**
* Heavy editor tooling or configuration frameworks.<br/>
	**暂不计划引入较重的编辑器工具链或配置框架。**

# Sample
The sample entry component is:<br/>
**示例入口组件位于：**
* [Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs](Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs)

The default sample scene is:<br/>
**默认示例场景位于：**
* [Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity](Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity)

The current convention is to attach `OboroSampleEntry` explicitly in scene objects rather than auto-creating it through bootstrap logic.<br/>
**当前约定是显式在场景对象上挂载 `OboroSampleEntry`，而不是通过自动 Bootstrap 逻辑隐式创建。**

The sample currently shows:<br/>
**当前示例主要展示：**
* Dynamic contour layers generated from an animated scalar field.<br/>
	**由动态标量场生成的轮廓层次；**
* Softly fused obstacle masses forming terrain-like contour structures.<br/>
	**由软融合障碍体构成的地形式轮廓结构；**
* Real-time contour updates while dragging obstacles.<br/>
	**拖拽障碍体时的实时轮廓更新。**

# Validation
## Headless Compile
Use Unity batchmode to validate import and script compilation.<br/>
**可使用 Unity 批处理模式验证导入与脚本编译。**

Please close all opened Unity Editor instances for `D:/UnityProjects/Oboro` before running batch validation.<br/>
**运行前请先关闭所有已打开 `D:/UnityProjects/Oboro` 的 Unity Editor 实例。**

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod Onovich.Oboro.Editor.OboroBatchValidation.CompileProject -quit -logFile "D:/UnityProjects/Oboro/Temp/oboro-compile.log"`

## PlayMode Smoke Test
Use the same batch validation entry to open the sample scene and run a headless PlayMode smoke test.<br/>
**可使用同一个批处理验证入口打开示例场景，并执行无头 PlayMode smoke 测试。**

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod Onovich.Oboro.Editor.OboroBatchValidation.PlayModeSmokeSampleEntry -logFile "D:/UnityProjects/Oboro/Temp/oboro-smoke.log"`

This smoke test will:<br/>
**该 smoke 测试会：**
* Open `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`.<br/>
	**打开 `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`；**
* Enter PlayMode and run briefly.<br/>
	**进入 PlayMode 并短时间运行；**
* Monitor `Error` / `Exception` / `Assert` logs.<br/>
	**监听 `Error` / `Exception` / `Assert` 日志；**
* Exit successfully when no runtime error is detected.<br/>
	**在未发现运行时错误时成功退出。**

# UPM URL
ssh://git@github.com/onovich/Oboro.git?path=/Assets/com.onovich.oboro#main
