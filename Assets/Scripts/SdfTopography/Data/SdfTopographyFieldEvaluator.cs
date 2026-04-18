using System.Collections.Generic;
using UnityEngine;

namespace SdfSample.SdfTopography.Data
{
    public sealed class SdfTopographyFieldEvaluator
    {
        public float Evaluate(float x, float y, float timeValue, IReadOnlyList<SdfObstacleData> obstacles)
        {
            var value =
                (Mathf.Sin(x * 0.002f + timeValue * 0.4f) +
                 Mathf.Cos(y * 0.0025f - timeValue * 0.3f) +
                 Mathf.Sin((x + y) * 0.0015f + timeValue * 0.2f)) * 1.5f;

            for (var index = 0; index < obstacles.Count; index++)
            {
                var obstacle = obstacles[index];
                var deltaX = x - obstacle.X;
                var deltaY = y - obstacle.Y;
                var softCore = obstacle.Radius * 0.22f;
                var distanceSquared = deltaX * deltaX + deltaY * deltaY + softCore * softCore;
                value += (obstacle.Radius * obstacle.Radius) / distanceSquared * obstacle.Intensity;
            }

            return value;
        }
    }
}
