using UnityEngine;
using MortiseFrame.Oboro;

namespace MortiseFrame.Oboro.Sample.Inside {

    internal class OboroSampleFieldCore {

        internal const int DisturbanceCurveSampleCount = 32;
        const float DisturbanceHalfRange = 4.5f;
        const float DisturbanceFullRange = DisturbanceHalfRange * 2f;

        readonly OboroSampleObstacleModel[] obstacles;
        readonly ContourLevelModel[] contourLevels;
        bool obstacleStateDirty;

        internal OboroSampleFieldCore() {
            obstacles = OboroSampleFactory.CreateObstacles();
            contourLevels = OboroSampleFactory.CreateContourLevels();
            obstacleStateDirty = true;
        }

        internal OboroSampleObstacleModel[] Obstacles => obstacles;
        internal ContourLevelModel[] ContourLevels => contourLevels;
        internal Color BackgroundColor => OboroSampleFactory.BackgroundColor;
        internal int GridResolution => OboroSampleFactory.GridResolution;
        internal float BackgroundTimeStep => OboroSampleFactory.BackgroundTimeStep;

        internal void Resize(int screenWidth, int screenHeight) {
            float radiusScale = Mathf.Max(0.5f, Mathf.Min(screenWidth, screenHeight) / 1000f);
            for (int i = 0; i < obstacles.Length; i++) {
                obstacles[i].UpdateLayout(screenWidth, screenHeight, radiusScale);
            }

            obstacleStateDirty = true;
        }

        internal float Evaluate(float x, float y, float time, float disturbanceIntensity, float[] disturbanceCurveSamples) {
            float value = EvaluateDisturbance(x, y, time, disturbanceIntensity, disturbanceCurveSamples);

            for (int i = 0; i < obstacles.Length; i++) {
                var obstacle = obstacles[i];
                float dx = x - obstacle.x;
                float dy = y - obstacle.y;
                float distSq = dx * dx + dy * dy + obstacle.softCoreSquared;
                value += obstacle.radiusSquared / distSq * obstacle.intensity;
            }

            return value;
        }

        internal static void SampleDisturbanceCurve(AnimationCurve curve, float[] targetSamples) {
            AnimationCurve sourceCurve = curve ?? AnimationCurve.Linear(0f, 0f, 1f, 1f);
            int lastIndex = targetSamples.Length - 1;

            for (int i = 0; i < targetSamples.Length; i++) {
                float normalized = lastIndex > 0 ? i / (float)lastIndex : 0f;
                targetSamples[i] = sourceCurve.Evaluate(normalized);
            }
        }

        internal static float EvaluateDisturbance(float x, float y, float time, float disturbanceIntensity, float[] disturbanceCurveSamples) {
            if (disturbanceIntensity <= 0f) {
                return 0f;
            }

            float rawValue =
                (Mathf.Sin(x * 0.002f + time * 0.4f) +
                 Mathf.Cos(y * 0.0025f - time * 0.3f) +
                 Mathf.Sin((x + y) * 0.0015f + time * 0.2f)) * 1.5f;

            float normalized = Mathf.Clamp01(rawValue / DisturbanceFullRange + 0.5f);
            float curved = SampleDisturbanceCurveLut(normalized, disturbanceCurveSamples);
            return (curved - 0.5f) * DisturbanceFullRange * disturbanceIntensity;
        }

        static float SampleDisturbanceCurveLut(float normalized, float[] disturbanceCurveSamples) {
            if (disturbanceCurveSamples == null || disturbanceCurveSamples.Length == 0) {
                return normalized;
            }

            if (disturbanceCurveSamples.Length == 1) {
                return disturbanceCurveSamples[0];
            }

            float scaled = Mathf.Clamp01(normalized) * (disturbanceCurveSamples.Length - 1);
            int lowerIndex = Mathf.FloorToInt(scaled);
            int upperIndex = Mathf.Min(lowerIndex + 1, disturbanceCurveSamples.Length - 1);
            float blend = scaled - lowerIndex;
            return Mathf.Lerp(disturbanceCurveSamples[lowerIndex], disturbanceCurveSamples[upperIndex], blend);
        }

        internal void MarkObstacleStateDirty() {
            obstacleStateDirty = true;
        }

        internal bool ConsumeObstacleStateDirty() {
            bool wasDirty = obstacleStateDirty;
            obstacleStateDirty = false;
            return wasDirty;
        }

    }

}
