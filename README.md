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

示例运行组件位于：
- [Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs](Assets/com.onovich.oboro/Scripts_Sample/OboroSampleEntry.cs)

默认示例场景位于：
- [Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity](Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity)

当前约定是**手动在场景对象上挂载** `OboroSampleEntry`，不再通过自动 Bootstrap 脚本创建入口，以免干扰上层用户测试。

运行后可看到：
- 动态 SDF 等高线层次。
- 多个软融合障碍体形成的地形起伏。
- 鼠标拖拽障碍体时实时更新轮廓。

## Validation

### Headless Compile

可使用 Unity 批处理模式在项目根目录做无头编译校验：

运行前请先关闭已打开 `D:/UnityProjects/Oboro` 的所有 Unity Editor 实例，否则 Unity 会因为项目锁直接失败。

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod Onovich.Oboro.Editor.OboroBatchValidation.CompileProject -quit -logFile "D:/UnityProjects/Oboro/Temp/oboro-compile.log"`

### PlayMode Smoke Test

可使用同一个批处理入口启动 Sample 场景并运行一次无头 PlayMode smoke 测试：

运行前同样需要关闭已打开当前项目的 Unity 实例。

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod Onovich.Oboro.Editor.OboroBatchValidation.PlayModeSmokeSampleEntry -logFile "D:/UnityProjects/Oboro/Temp/oboro-smoke.log"`

该 smoke 测试会：
- 打开 `Assets/com.onovich.oboro/Resources_Sample/SampleEntry.unity`
- 进入 PlayMode
- 短时间运行并监听 Error / Exception / Assert
- 无报错则以成功退出

## UPM URL

可通过 Git URL 引入：

`ssh://git@github.com/onovich/Oboro.git?path=/Assets/com.onovich.oboro#main`
