using UnityEngine;
using MortiseFrame.Oboro;
using MortiseFrame.Oboro.Sample.Inside;

namespace MortiseFrame.Oboro.Sample {

    [DefaultExecutionOrder(-1000)]
    public class OboroSampleEntry : MonoBehaviour {

        [SerializeField] bool preferGpuRenderer = true;
        [Header("Disturbance")]
        [SerializeField] [Min(0f)] float disturbanceIntensity = 1f;
        [SerializeField] [Min(0f)] float disturbanceTimeScale = 1f;
        [SerializeField] AnimationCurve disturbanceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        ContourCore contourCore;
        ContourRendererCore contourRendererCore;
        OboroSampleFieldCore fieldCore;
        OboroSampleGpuRendererCore gpuRendererCore;
        OboroSampleInteractionController interactionController;
        readonly System.Action<ContourLevelModel, Vector2, Vector2> emitContourHandler;
        readonly System.Func<float, float, float> fieldEvaluator;
        readonly float[] disturbanceCurveSamples;
        Camera targetCamera;
        float elapsedTime;

        public OboroSampleEntry() {
            emitContourHandler = EmitContour;
            fieldEvaluator = EvaluateField;
            disturbanceCurveSamples = new float[OboroSampleFieldCore.DisturbanceCurveSampleCount];
        }

        void Awake() {
            contourCore = new ContourCore();
            contourRendererCore = new ContourRendererCore();
            fieldCore = new OboroSampleFieldCore();
            gpuRendererCore = new OboroSampleGpuRendererCore();
            interactionController = new OboroSampleInteractionController();

            SyncDisturbanceCurveSamples();
            EnsureCamera();
            RefreshLayout(true);
        }

        void OnEnable() {
            SyncDisturbanceCurveSamples();
            EnsureCamera();
            RefreshLayout(true);
        }

        void Update() {
            SyncDisturbanceCurveSamples();
            elapsedTime += fieldCore.BackgroundTimeStep * disturbanceTimeScale;
            RefreshLayout(false);

            bool obstacleStateChanged = interactionController.Tick(
                fieldCore.Obstacles,
                GetPointerPosition(),
                Input.GetMouseButtonDown(0),
                Input.GetMouseButton(0),
                Input.GetMouseButtonUp(0),
                contourCore.ScreenWidth,
                contourCore.ScreenHeight);

            if (obstacleStateChanged) {
                fieldCore.MarkObstacleStateDirty();
            }
        }

        void OnRenderObject() {
            if (!enabled) {
                return;
            }
            if (targetCamera == null || Camera.current != targetCamera) {
                return;
            }

            if (preferGpuRenderer && gpuRendererCore.Prepare(fieldCore)) {
                gpuRendererCore.Render(fieldCore, elapsedTime, contourCore.ScreenWidth, contourCore.ScreenHeight, disturbanceIntensity, disturbanceCurveSamples);
                return;
            }

            if (!contourRendererCore.Prepare()) {
                return;
            }

            contourRendererCore.Begin(contourCore.ScreenWidth, contourCore.ScreenHeight);
            contourCore.BuildField(fieldEvaluator);
            contourCore.EmitContour(fieldCore.ContourLevels, emitContourHandler);
            contourRendererCore.End();
        }

        void OnDestroy() {
            contourRendererCore.TearDown();
            gpuRendererCore?.TearDown();
        }

        void EnsureCamera() {
            targetCamera = Camera.main;
            if (targetCamera == null) {
                targetCamera = FindFirstObjectByType<Camera>();
            }

            if (targetCamera == null) {
                var cameraObject = new GameObject("Oboro Sample Camera");
                targetCamera = cameraObject.AddComponent<Camera>();
                targetCamera.tag = "MainCamera";
            }

            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = fieldCore != null ? fieldCore.BackgroundColor : OboroSampleFactory.BackgroundColor;
            targetCamera.orthographic = true;
            targetCamera.nearClipPlane = -10f;
            targetCamera.farClipPlane = 10f;
            targetCamera.transform.position = new Vector3(0f, 0f, -5f);
        }

        void RefreshLayout(bool force) {
            int screenWidth = Mathf.Max(1, Screen.width);
            int screenHeight = Mathf.Max(1, Screen.height);

            if (!force && screenWidth == contourCore.ScreenWidth && screenHeight == contourCore.ScreenHeight) {
                return;
            }

            contourCore.Resize(screenWidth, screenHeight, fieldCore.GridResolution);
            fieldCore.Resize(screenWidth, screenHeight);
        }

        Vector2 GetPointerPosition() {
            var mousePosition = Input.mousePosition;
            return new Vector2(mousePosition.x, contourCore.ScreenHeight - mousePosition.y);
        }

        float EvaluateField(float x, float y) {
            return fieldCore.Evaluate(x, y, elapsedTime, disturbanceIntensity, disturbanceCurveSamples);
        }

        void SyncDisturbanceCurveSamples() {
            OboroSampleFieldCore.SampleDisturbanceCurve(disturbanceCurve, disturbanceCurveSamples);
        }

        void EmitContour(ContourLevelModel level, Vector2 start, Vector2 end) {
            contourRendererCore.DrawLine(level, start, end);
        }

    }

}
