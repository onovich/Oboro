using UnityEngine;
using Onovich.Oboro;

namespace Onovich.Oboro.Sample.Inside {

    internal static class OboroSampleFactory {

        internal const int GridResolution = 8;
        internal const float BackgroundTimeStep = 0.02f;
        internal static readonly Color BackgroundColor = new Color(11f / 255f, 9f / 255f, 6f / 255f, 1f);

        internal static ContourLevelModel[] CreateContourLevels() {
            return new[] {
                new ContourLevelModel(-2.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.01f)),
                new ContourLevelModel(-1.2f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.02f)),
                new ContourLevelModel(-0.5f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.03f)),
                new ContourLevelModel(0.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.04f)),
                new ContourLevelModel(0.4f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.05f)),
                new ContourLevelModel(0.8f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.06f)),
                new ContourLevelModel(1.3f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.07f)),
                new ContourLevelModel(1.9f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.08f)),
                new ContourLevelModel(2.6f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.10f)),
                new ContourLevelModel(3.5f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.12f)),
                new ContourLevelModel(4.6f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.15f)),
                new ContourLevelModel(6.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.19f)),
                new ContourLevelModel(7.8f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.24f)),
                new ContourLevelModel(10.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.30f)),
                new ContourLevelModel(13.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.38f)),
                new ContourLevelModel(17.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.48f)),
                new ContourLevelModel(22.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.60f)),
                new ContourLevelModel(30.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.70f))
            };
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

    }

}
