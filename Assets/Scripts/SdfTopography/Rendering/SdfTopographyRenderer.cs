using System;
using UnityEngine;
using SdfSample.Common.Contours;
using SdfSample.SdfTopography.Data;
using SdfSample.SdfTopography.Interaction;

namespace SdfSample.SdfTopography.Rendering
{
    [DefaultExecutionOrder(-1000)]
    public sealed class SdfTopographyRenderer : MonoBehaviour
    {
        private readonly SdfTopographyFieldEvaluator fieldEvaluator = new SdfTopographyFieldEvaluator();
        private readonly SdfObstacleInteractionController interactionController = new SdfObstacleInteractionController();

        private Material lineMaterial;
        private float[] fieldGrid = Array.Empty<float>();
        private SdfObstacleData[] obstacles = Array.Empty<SdfObstacleData>();
        private ContourLevel[] contourLevels = Array.Empty<ContourLevel>();
        private int columns;
        private int rows;
        private int screenWidth;
        private int screenHeight;
        private float elapsedTime;

        private void Awake()
        {
            contourLevels = SdfTopographyDefaults.CreateContourLevels();
            obstacles = SdfTopographyDefaults.CreateObstacles();
            RebuildLayout(true);
        }

        private void OnEnable()
        {
            EnsureCamera();
            EnsureMaterial();
            RebuildLayout(true);
        }

        private void Update()
        {
            elapsedTime += SdfTopographyDefaults.BackgroundTimeStep;

            RebuildLayout(false);
            interactionController.Handle(
                obstacles,
                GetPointerPosition(),
                Input.GetMouseButtonDown(0),
                Input.GetMouseButton(0),
                Input.GetMouseButtonUp(0),
                screenWidth,
                screenHeight);
        }

        private void OnRenderObject()
        {
            if (!enabled || Camera.current == null)
            {
                return;
            }

            if (lineMaterial == null)
            {
                EnsureMaterial();
                if (lineMaterial == null)
                {
                    return;
                }
            }

            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0f, screenWidth, screenHeight, 0f);
            GL.Begin(GL.LINES);

            ScalarFieldGridBuilder.Build(
                fieldGrid,
                columns,
                rows,
                SdfTopographyDefaults.GridResolution,
                delegate(float x, float y) { return fieldEvaluator.Evaluate(x, y, elapsedTime, obstacles); });

            MarchingSquaresUtility.EmitContourSegments(
                fieldGrid,
                columns,
                rows,
                SdfTopographyDefaults.GridResolution,
                contourLevels,
                EmitSegment);

            GL.End();
            GL.PopMatrix();
        }

        private void OnDestroy()
        {
            if (lineMaterial != null)
            {
                Destroy(lineMaterial);
            }
        }

        private void EnsureCamera()
        {
            var targetCamera = Camera.main;
            if (targetCamera == null)
            {
                targetCamera = FindFirstObjectByType<Camera>();
            }

            if (targetCamera == null)
            {
                var cameraObject = new GameObject("SDF Topography Camera");
                targetCamera = cameraObject.AddComponent<Camera>();
                targetCamera.tag = "MainCamera";
            }

            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = SdfTopographyDefaults.BackgroundColor;
            targetCamera.orthographic = true;
            targetCamera.nearClipPlane = -10f;
            targetCamera.farClipPlane = 10f;
            targetCamera.transform.position = new Vector3(0f, 0f, -5f);
        }

        private void EnsureMaterial()
        {
            if (lineMaterial != null)
            {
                return;
            }

            var shader = Shader.Find("Hidden/Internal-Colored");
            if (shader == null)
            {
                return;
            }

            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }

        private void RebuildLayout(bool force)
        {
            var currentWidth = Mathf.Max(1, Screen.width);
            var currentHeight = Mathf.Max(1, Screen.height);
            if (!force && currentWidth == screenWidth && currentHeight == screenHeight)
            {
                return;
            }

            screenWidth = currentWidth;
            screenHeight = currentHeight;
            columns = Mathf.CeilToInt(screenWidth / (float)SdfTopographyDefaults.GridResolution);
            rows = Mathf.CeilToInt(screenHeight / (float)SdfTopographyDefaults.GridResolution);

            var requiredSize = (columns + 1) * (rows + 1);
            if (fieldGrid.Length != requiredSize)
            {
                fieldGrid = new float[requiredSize];
            }

            var radiusScale = Mathf.Max(0.5f, Mathf.Min(screenWidth, screenHeight) / 1000f);
            for (var index = 0; index < obstacles.Length; index++)
            {
                obstacles[index].UpdateLayout(screenWidth, screenHeight, radiusScale);
            }
        }

        private Vector2 GetPointerPosition()
        {
            var mousePosition = Input.mousePosition;
            return new Vector2(mousePosition.x, screenHeight - mousePosition.y);
        }

        private static void EmitSegment(ContourLevel level, Vector2 start, Vector2 end)
        {
            GL.Color(level.Color);
            GL.Vertex3(start.x, start.y, 0f);
            GL.Vertex3(end.x, end.y, 0f);
        }
    }
}
