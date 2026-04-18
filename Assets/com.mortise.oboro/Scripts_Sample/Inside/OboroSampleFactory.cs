using System.Collections.Generic;
using UnityEngine;
using MortiseFrame.Oboro;

namespace MortiseFrame.Oboro.Sample.Inside {

    internal static class OboroSampleFactory {

        internal const int GridResolution = 8;
        internal const float BackgroundTimeStep = 0.01f;
        internal static readonly Color BackgroundColor = new Color(11f / 255f, 9f / 255f, 6f / 255f, 1f);
        internal static readonly float[] ContourValues = {
            -2.0f, -1.2f, -0.5f, 0.0f, 0.4f, 0.8f, 1.3f, 1.9f, 2.6f,
            3.5f, 4.6f, 6.0f, 7.8f, 10.0f, 13.0f, 17.0f, 22.0f, 30.0f
        };
        internal const float DefaultCenterRelativeX = 0.5f;
        internal const float DefaultCenterRelativeY = 0.5f;
        internal const float DefaultCenterBaseRadius = 140f;
        internal const float DefaultCenterIntensity = 3.5f;

        internal static OboroSampleContourData CreateDefaultContourData() {
            var data = new OboroSampleContourData();
            data.contourObstacles = new List<OboroSampleObstacleModel>(CreateObstacles());
            return data;
        }

        internal static ContourLevelModel[] CreateContourLevels(Color startColor, Color endColor) {
            var levels = new ContourLevelModel[ContourValues.Length];
            int lastIndex = ContourValues.Length - 1;
            for (int i = 0; i < ContourValues.Length; i++) {
                float t = lastIndex > 0 ? i / (float)lastIndex : 0f;
                levels[i] = new ContourLevelModel(ContourValues[i], Color.Lerp(startColor, endColor, t));
            }

            return levels;
        }

        internal static OboroSampleObstacleModel[] CreateObstacles() {
            return new[] {
                new OboroSampleObstacleModel(0.15f, 0.25f, 90f, 1.8f),
                new OboroSampleObstacleModel(0.35f, 0.45f, 160f, 5.0f),
                new OboroSampleObstacleModel(0.65f, 0.35f, 130f, 3.2f),
                new OboroSampleObstacleModel(0.85f, 0.20f, 85f, 1.5f),
                new OboroSampleObstacleModel(0.50f, 0.70f, 180f, 6.5f),
                new OboroSampleObstacleModel(0.20f, 0.75f, 110f, 2.5f),
                new OboroSampleObstacleModel(0.80f, 0.70f, 140f, 4.0f),
                new OboroSampleObstacleModel(0.50f, 0.15f, 75f, 1.2f)
            };
        }

        internal static void CreateContourCenter(List<OboroSampleObstacleModel> obstacles, int ringCount, Vector2 perRingOffset, float ringScaleRatio) {
            if (obstacles == null) {
                return;
            }

            int safeRingCount = Mathf.Max(1, ringCount);
            float safeScale = Mathf.Max(0.01f, ringScaleRatio);
            Vector2 nextCenter = new Vector2(DefaultCenterRelativeX, DefaultCenterRelativeY);
            float nextRadius = DefaultCenterBaseRadius;

            for (int i = 0; i < safeRingCount; i++) {
                obstacles.Add(new OboroSampleObstacleModel(nextCenter.x, nextCenter.y, nextRadius, DefaultCenterIntensity));
                nextCenter += perRingOffset;
                nextRadius *= safeScale;
            }
        }

    }

}
