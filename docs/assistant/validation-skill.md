# Oboro Validation Skill

> 用途：给未来会话或本地验证提供统一、低歧义的无头编译与 smoke 测试方法。

## 适用前提

- Unity 版本：`2023.2.22f1`
- 项目根目录：`D:/UnityProjects/Oboro`
- Sample 场景：`Assets/com.mortise.oboro/Resources_Sample/SampleEntry.unity`
- 批处理验证入口：`MortiseFrame.Oboro.Editor.OboroBatchValidation`
- 运行前必须关闭已打开当前项目的所有 Unity Editor 实例，否则批处理会因 project lock 失败
- `CompileProject` 与 `PlayModeSmokeSampleEntry` 必须串行执行，不能并行运行两个 Unity batchmode 进程

## 1. 无头编译

用途：验证项目在当前路径下能完成导入与脚本编译。

命令：

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod MortiseFrame.Oboro.Editor.OboroBatchValidation.CompileProject -quit -logFile "D:/UnityProjects/Oboro/Temp/oboro-compile.log"`

通过标准：

- 进程返回码为 `0`
- 日志中没有脚本编译错误
- 如果当前机器上的 Unity batchmode 返回码异常，以日志中的 `Compile passed under batchmode.` 为最终判定依据

## 2. PlayMode Smoke Test

用途：打开 Sample 场景并以无头模式进入 PlayMode，确认短时间运行无 `Error` / `Exception` / `Assert`。

命令：

`"D:/UnityEditors/Unity 2023.2.22f1/Editor/Unity.exe" -batchmode -nographics -projectPath "D:/UnityProjects/Oboro" -executeMethod MortiseFrame.Oboro.Editor.OboroBatchValidation.PlayModeSmokeSampleEntry -logFile "D:/UnityProjects/Oboro/Temp/oboro-smoke.log"`

该 smoke 测试会：

1. 打开 `Assets/com.mortise.oboro/Resources_Sample/SampleEntry.unity`
2. 进入 PlayMode
3. 短时间运行示例
4. 监听 `Error` / `Exception` / `Assert`
5. 无报错则退出并返回 `0`

通过标准：

- 进程返回码为 `0`
- 日志包含 smoke 启动与成功结束标记
- 未出现 `Smoke test captured log error` 或未处理异常
- 如果当前机器上的 Unity batchmode 返回码异常，以日志中的 `PlayMode smoke passed for SampleEntry scene.` 为最终判定依据

## 3. 使用建议

- 改动 Runtime 算法、Sample 场逻辑、场景入口或渲染流程后，优先跑 compile + smoke。
- 只改文档时通常不需要跑 smoke。
- 如果只是 Unity 本地缓存或布局残留，不要把这类问题误判成源码错误。
- 推荐顺序固定为：先 `CompileProject`，确认结束后再跑 `PlayModeSmokeSampleEntry`。
- 不要并行起两个 Unity 验证命令，否则很容易因为 project lock 造成假失败。

## 4. 常见结论模板

- `Compile passed under batchmode.`
- `PlayMode smoke passed for SampleEntry scene.`
- `Failure is likely from scene/runtime logic, not local path rename.`