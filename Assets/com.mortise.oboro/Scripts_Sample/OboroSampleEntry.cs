using UnityEngine;
using System.Collections.Generic;
using MortiseFrame.Oboro;
using MortiseFrame.Oboro.Sample.Inside;

namespace MortiseFrame.Oboro.Sample {

    [DefaultExecutionOrder(-1000)]
    public class OboroSampleEntry : MonoBehaviour {

        [SerializeField] bool preferGpuRenderer = true;
        [SerializeField] OboroSampleContourData contourData = OboroSampleFactory.CreateDefaultContourData();
        [SerializeField] OboroSampleContourDataAsset bakedContourData;
        [SerializeField] string runtimeSaveKey = "oboro.sample.contour-data";

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
            contourData ??= OboroSampleFactory.CreateDefaultContourData();
            fieldCore = new OboroSampleFieldCore(contourData);
            gpuRendererCore = new OboroSampleGpuRendererCore();
            interactionController = new OboroSampleInteractionController();

            fieldCore.SyncContourLevels();
            SyncDisturbanceCurveSamples();
            EnsureCamera();
            RefreshLayout(true);
        }

        void OnEnable() {
            contourData ??= OboroSampleFactory.CreateDefaultContourData();
            fieldCore?.SyncContourLevels();
            SyncDisturbanceCurveSamples();
            EnsureCamera();
            RefreshLayout(true);
        }

        void Update() {
            SyncDisturbanceCurveSamples();
            fieldCore.SyncContourLevels();
            elapsedTime += fieldCore.BackgroundTimeStep * contourData.disturbanceTimeScale;
            RefreshLayout(true);

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
                gpuRendererCore.Render(fieldCore, elapsedTime, contourCore.ScreenWidth, contourCore.ScreenHeight, contourData.disturbanceIntensity, contourData.disturbancePhase, contourData.lineThickness, disturbanceCurveSamples);
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

            bool screenChanged = force || screenWidth != contourCore.ScreenWidth || screenHeight != contourCore.ScreenHeight;
            if (screenChanged) {
                contourCore.Resize(screenWidth, screenHeight, fieldCore.GridResolution);
            }

            fieldCore.Resize(screenWidth, screenHeight);
        }

        Vector2 GetPointerPosition() {
            var mousePosition = Input.mousePosition;
            return new Vector2(mousePosition.x, contourCore.ScreenHeight - mousePosition.y);
        }

        float EvaluateField(float x, float y) {
            return fieldCore.Evaluate(x, y, elapsedTime, contourData.disturbanceIntensity, contourData.disturbancePhase, disturbanceCurveSamples);
        }

        void SyncDisturbanceCurveSamples() {
            OboroSampleFieldCore.SampleDisturbanceCurve(contourData.disturbanceCurve, disturbanceCurveSamples);
        }

        public void CreateContourCenter(int ringCount, Vector2 perRingOffset, float ringScaleRatio) {
            OboroSampleFactory.CreateContourCenter(contourData.contourObstacles, ringCount, perRingOffset, ringScaleRatio);
            fieldCore.MarkObstacleStateDirty();
        }

        public OboroSampleContourData CaptureContourData() {
            return contourData.Clone();
        }

        public void ApplyContourData(OboroSampleContourData data) {
            if (data == null) {
                return;
            }

            contourData = data.Clone();
            fieldCore = new OboroSampleFieldCore(contourData);
            fieldCore.SyncContourLevels();
            SyncDisturbanceCurveSamples();
            RefreshLayout(true);
        }

        public void ApplyBakedContourData() {
            if (bakedContourData == null || bakedContourData.data == null) {
                return;
            }

            ApplyContourData(bakedContourData.data);
        }

        public void SaveContourData() {
            string json = JsonUtility.ToJson(contourData.Clone());
            PlayerPrefs.SetString(runtimeSaveKey, json);
            PlayerPrefs.Save();
        }

        public bool LoadContourData() {
            if (!PlayerPrefs.HasKey(runtimeSaveKey)) {
                return false;
            }

            string json = PlayerPrefs.GetString(runtimeSaveKey);
            var loaded = JsonUtility.FromJson<OboroSampleContourData>(json);
            if (loaded == null) {
                return false;
            }

            ApplyContourData(loaded);
            return true;
        }

        void EmitContour(ContourLevelModel level, Vector2 start, Vector2 end) {
            contourRendererCore.DrawLine(level, start, end);
        }

    }

}
