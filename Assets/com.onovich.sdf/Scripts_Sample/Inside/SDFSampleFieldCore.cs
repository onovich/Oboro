using UnityEngine;
using Onovich.SDF;

namespace Onovich.SDF.Sample.Inside {

    internal class SDFSampleFieldCore {

        readonly SDFSampleObstacleModel[] obstacles;
        readonly ContourLevelModel[] contourLevels;

        internal SDFSampleFieldCore() {
            obstacles = SDFSampleFactory.CreateObstacles();
            contourLevels = SDFSampleFactory.CreateContourLevels();
        }

        internal SDFSampleObstacleModel[] Obstacles => obstacles;
        internal ContourLevelModel[] ContourLevels => contourLevels;
        internal Color BackgroundColor => SDFSampleFactory.BackgroundColor;
        internal int GridResolution => SDFSampleFactory.GridResolution;
        internal float BackgroundTimeStep => SDFSampleFactory.BackgroundTimeStep;

        internal void Resize(int screenWidth, int screenHeight) {
            float radiusScale = Mathf.Max(0.5f, Mathf.Min(screenWidth, screenHeight) / 1000f);
            for (int i = 0; i < obstacles.Length; i++) {
                obstacles[i].UpdateLayout(screenWidth, screenHeight, radiusScale);
            }
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
                float softCore = obstacle.radius * 0.22f;
                float distSq = dx * dx + dy * dy + softCore * softCore;
                value += (obstacle.radius * obstacle.radius) / distSq * obstacle.intensity;
            }

            return value;
        }

    }

}
