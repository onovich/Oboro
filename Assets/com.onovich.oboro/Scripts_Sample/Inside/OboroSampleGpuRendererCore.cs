using UnityEngine;

namespace Onovich.Oboro.Sample.Inside {

    internal sealed class OboroSampleGpuRendererCore {

        const string ShaderName = "Hidden/Onovich/OboroSampleContour";
        const float LineThickness = 0.045f;
        const float LineFeather = 0.03f;

        static readonly int ScreenSizeId = Shader.PropertyToID("_ScreenSize");
        static readonly int TimeValueId = Shader.PropertyToID("_TimeValue");
        static readonly int ObstacleCountId = Shader.PropertyToID("_ObstacleCount");
        static readonly int ObstacleDataId = Shader.PropertyToID("_ObstacleData");
        static readonly int ContourCountId = Shader.PropertyToID("_ContourCount");
        static readonly int ContourValuesId = Shader.PropertyToID("_ContourValues");
        static readonly int ContourColorsId = Shader.PropertyToID("_ContourColors");
        static readonly int LineThicknessId = Shader.PropertyToID("_LineThickness");
        static readonly int LineFeatherId = Shader.PropertyToID("_LineFeather");

        Material material;
        Mesh fullscreenMesh;
        Vector4[] obstacleData;
        float[] contourValues;
        Vector4[] contourColors;
        bool contourDataUploaded;

        internal bool Prepare(OboroSampleFieldCore fieldCore) {
            if (material == null) {
                Shader shader = Shader.Find(ShaderName);
                if (shader == null) {
                    return false;
                }

                material = new Material(shader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }

            EnsureBuffers(fieldCore);
            EnsureFullscreenMesh();
            return true;
        }

        internal void Render(OboroSampleFieldCore fieldCore, float elapsedTime, int screenWidth, int screenHeight) {
            if (fieldCore.ConsumeObstacleStateDirty()) {
                UploadObstacleData(fieldCore.Obstacles);
            }

            material.SetVector(ScreenSizeId, new Vector4(screenWidth, screenHeight, 0f, 0f));
            material.SetFloat(TimeValueId, elapsedTime);
            material.SetFloat(LineThicknessId, LineThickness);
            material.SetFloat(LineFeatherId, LineFeather);

            material.SetPass(0);
            Graphics.DrawMeshNow(fullscreenMesh, Matrix4x4.identity);
        }

        internal void TearDown() {
            if (material != null) {
                Object.Destroy(material);
                material = null;
            }

            if (fullscreenMesh != null) {
                Object.Destroy(fullscreenMesh);
                fullscreenMesh = null;
            }
        }

        void EnsureBuffers(OboroSampleFieldCore fieldCore) {
            OboroSampleObstacleModel[] obstacles = fieldCore.Obstacles;
            if (obstacleData == null || obstacleData.Length != obstacles.Length) {
                obstacleData = new Vector4[obstacles.Length];
            }

            ContourLevelModel[] levels = fieldCore.ContourLevels;
            bool contourBufferMatches = contourValues != null && contourValues.Length == levels.Length && contourColors != null && contourColors.Length == levels.Length;
            if (!contourBufferMatches) {
                contourValues = new float[levels.Length];
                contourColors = new Vector4[levels.Length];
                contourDataUploaded = false;
            }

            if (!contourDataUploaded) {
                for (int i = 0; i < levels.Length; i++) {
                    ContourLevelModel level = levels[i];
                    contourValues[i] = level.value;
                    contourColors[i] = level.color;
                }

                material.SetInt(ContourCountId, contourValues.Length);
                material.SetFloatArray(ContourValuesId, contourValues);
                material.SetVectorArray(ContourColorsId, contourColors);
                contourDataUploaded = true;
            }
        }

        void UploadObstacleData(OboroSampleObstacleModel[] obstacles) {
            for (int i = 0; i < obstacles.Length; i++) {
                var obstacle = obstacles[i];
                obstacleData[i] = new Vector4(obstacle.x, obstacle.y, obstacle.radiusSquared * obstacle.intensity, obstacle.softCoreSquared);
            }

            material.SetInt(ObstacleCountId, obstacles.Length);
            material.SetVectorArray(ObstacleDataId, obstacleData);
        }

        void EnsureFullscreenMesh() {
            if (fullscreenMesh != null) {
                return;
            }

            fullscreenMesh = new Mesh {
                name = "Oboro Sample Fullscreen Quad"
            };
            fullscreenMesh.hideFlags = HideFlags.HideAndDontSave;
            fullscreenMesh.vertices = new[] {
                new Vector3(-1f, -1f, 0f),
                new Vector3(1f, -1f, 0f),
                new Vector3(1f, 1f, 0f),
                new Vector3(-1f, 1f, 0f)
            };
            fullscreenMesh.uv = new[] {
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(1f, 1f),
                new Vector2(0f, 1f)
            };
            fullscreenMesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };
            fullscreenMesh.UploadMeshData(true);
        }

    }

}