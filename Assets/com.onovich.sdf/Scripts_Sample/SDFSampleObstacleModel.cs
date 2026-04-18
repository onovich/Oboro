using System;
using UnityEngine;

namespace Onovich.SDF.Sample {

    [Serializable]
    public class SDFSampleObstacleModel {

        public float relativeX;
        public float relativeY;
        public float baseRadius;
        public float intensity;
        public float x;
        public float y;
        public float radius;

        public SDFSampleObstacleModel(float relativeX, float relativeY, float baseRadius, float intensity) {
            this.relativeX = relativeX;
            this.relativeY = relativeY;
            this.baseRadius = baseRadius;
            this.intensity = intensity;
        }

        public void UpdateLayout(float screenWidth, float screenHeight, float radiusScale) {
            x = screenWidth * relativeX;
            y = screenHeight * relativeY;
            radius = baseRadius * radiusScale;
        }

        public bool Contains(Vector2 point) {
            return Vector2.Distance(point, new Vector2(x, y)) < radius;
        }

    }

}
