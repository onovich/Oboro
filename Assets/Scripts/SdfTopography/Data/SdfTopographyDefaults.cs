using UnityEngine;
using SdfSample.Common.Contours;

namespace SdfSample.SdfTopography.Data
{
    public static class SdfTopographyDefaults
    {
        public const int GridResolution = 8;
        public const float BackgroundTimeStep = 0.02f;
        public static readonly Color BackgroundColor = new Color(11f / 255f, 9f / 255f, 6f / 255f, 1f);

        public static ContourLevel[] CreateContourLevels()
        {
            return new[]
            {
                new ContourLevel(-2.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.01f)),
                new ContourLevel(-1.2f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.02f)),
                new ContourLevel(-0.5f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.03f)),
                new ContourLevel(0.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.04f)),
                new ContourLevel(0.4f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.05f)),
                new ContourLevel(0.8f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.06f)),
                new ContourLevel(1.3f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.07f)),
                new ContourLevel(1.9f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.08f)),
                new ContourLevel(2.6f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.10f)),
                new ContourLevel(3.5f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.12f)),
                new ContourLevel(4.6f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.15f)),
                new ContourLevel(6.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.19f)),
                new ContourLevel(7.8f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.24f)),
                new ContourLevel(10.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.30f)),
                new ContourLevel(13.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.38f)),
                new ContourLevel(17.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.48f)),
                new ContourLevel(22.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.60f)),
                new ContourLevel(30.0f, new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.70f))
            };
        }

        public static SdfObstacleData[] CreateObstacles()
        {
            return new[]
            {
                new SdfObstacleData(0.15f, 0.25f, 90f, 1.8f),
                new SdfObstacleData(0.35f, 0.45f, 160f, 5.0f),
                new SdfObstacleData(0.65f, 0.35f, 130f, 3.2f),
                new SdfObstacleData(0.85f, 0.20f, 85f, 1.5f),
                new SdfObstacleData(0.50f, 0.70f, 180f, 6.5f),
                new SdfObstacleData(0.20f, 0.75f, 110f, 2.5f),
                new SdfObstacleData(0.80f, 0.70f, 140f, 4.0f),
                new SdfObstacleData(0.50f, 0.15f, 75f, 1.2f)
            };
        }
    }
}
