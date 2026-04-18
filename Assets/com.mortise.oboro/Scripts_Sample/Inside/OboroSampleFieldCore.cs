using UnityEngine;
using MortiseFrame.Oboro;

namespace MortiseFrame.Oboro.Sample.Inside {

    internal class OboroSampleFieldCore {

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

        internal float Evaluate(float x, float y, float time) {
            float value =
                (Mathf.Sin(x * 0.002f + time * 0.4f) +
                 Mathf.Cos(y * 0.0025f - time * 0.3f) +
                 Mathf.Sin((x + y) * 0.0015f + time * 0.2f)) * 1.5f;

            for (int i = 0; i < obstacles.Length; i++) {
                var obstacle = obstacles[i];
                float dx = x - obstacle.x;
                float dy = y - obstacle.y;
                float distSq = dx * dx + dy * dy + obstacle.softCoreSquared;
                value += obstacle.radiusSquared / distSq * obstacle.intensity;
            }

            return value;
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
