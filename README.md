# Oboro

Oboro（朧）是一个用于 Unity 的 SDF 等高线运行时与示例包。

“朧”意为月色朦胧、边界模糊，正好对应 SDF 在边缘融合、软过渡与轮廓漂移上的视觉气质。

当前仓库提供两部分内容：
- 一个可复用的运行时等高线库，用于标量场采样与 Marching Squares 轮廓提取。
- 一个可直接运行的 Sample，用于展示 Oboro 风格的动态 SDF 地形、轮廓层次与障碍拖拽交互。

## Readiness

Oboro 目前适合：
- 作为 Unity 中二维标量场 / SDF 等高线效果的基础运行时。
- 作为实验性视觉项目、交互草图与图形原型的轮廓生成模块。
- 作为独立 UPM 包接入其他 Unity 项目。

## Features

- Runtime / Sample 分层结构，便于复用与演示分离。
- 通用标量场网格构建流程。
- 基于 Marching Squares 的多级等高线提取。
- 可配置轮廓层级与颜色透明度。
- 示例内置软核心障碍场、动态背景扰动与鼠标拖拽交互。
- 采用接近 UPM 的包组织方式，便于集成到现有 Unity 工程。

## Sample

示例入口位于：
- [Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs](Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs)
- [Assets/com.onovich.oboro/Scripts_Sample/OboroSampleBootstrap.cs](Assets/com.onovich.oboro/Scripts_Sample/OboroSampleBootstrap.cs)

运行后可看到：
- 动态 SDF 等高线层次。
- 多个软融合障碍体形成的地形起伏。
- 鼠标拖拽障碍体时实时更新轮廓。

## UPM URL

可通过 Git URL 引入：

`ssh://git@github.com/onovich/SDFSample.git?path=/Assets/com.onovich.oboro#main`
