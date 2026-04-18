using UnityEngine;
using Onovich.SDF.Sample.Inside;

namespace Onovich.SDF.Sample {

    [DefaultExecutionOrder(-1000)]
    public class SDFSampleEntry : MonoBehaviour {

        ContourCore contourCore;
        ContourRendererCore contourRendererCore;
        SDFSampleFieldCore fieldCore;
        SDFSampleInteractionController interactionController;
        Camera targetCamera;
        float elapsedTime;

        void Awake() {
            contourCore = new ContourCore();
            contourRendererCore = new ContourRendererCore();
            fieldCore = new SDFSampleFieldCore();
            interactionController = new SDFSampleInteractionController();

            EnsureCamera();
            RefreshLayout(true);
        }

        void OnEnable() {
            EnsureCamera();
            RefreshLayout(true);
        }

        void Update() {
            elapsedTime += fieldCore.BackgroundTimeStep;
            RefreshLayout(false);

            interactionController.Tick(
                fieldCore.Obstacles,
                GetPointerPosition(),
                Input.GetMouseButtonDown(0),
                Input.GetMouseButton(0),
                Input.GetMouseButtonUp(0),
                contourCore.ScreenWidth,
                contourCore.ScreenHeight);
        }

        void OnRenderObject() {
            if (!enabled) {
                return;
            }
            if (targetCamera == null || Camera.current != targetCamera) {
                return;
            }
            if (!contourRendererCore.Prepare()) {
                return;
            }

            contourRendererCore.Begin(contourCore.ScreenWidth, contourCore.ScreenHeight);
            contourCore.BuildField(delegate(float x, float y) { return fieldCore.Evaluate(x, y, elapsedTime); });
            contourCore.EmitContour(fieldCore.ContourLevels, EmitContour);
            contourRendererCore.End();
        }

        void OnDestroy() {
            contourRendererCore.TearDown();
        }

        void EnsureCamera() {
            targetCamera = Camera.main;
            if (targetCamera == null) {
                targetCamera = FindFirstObjectByType<Camera>();
            }

            if (targetCamera == null) {
                var cameraObject = new GameObject("SDF Sample Camera");
                targetCamera = cameraObject.AddComponent<Camera>();
                targetCamera.tag = "MainCamera";
            }

            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = fieldCore != null ? fieldCore.BackgroundColor : SDFSampleFactory.BackgroundColor;
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

        void EmitContour(ContourLevelModel level, Vector2 start, Vector2 end) {
            contourRendererCore.DrawLine(level, start, end);
        }

    }

}
