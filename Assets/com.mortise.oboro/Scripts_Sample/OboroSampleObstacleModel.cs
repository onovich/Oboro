using System;
using UnityEngine;

namespace MortiseFrame.Oboro.Sample {

    [Serializable]
    public class OboroSampleObstacleModel {

        public float relativeX;
        public float relativeY;
        public float baseRadius;
        public float intensity;
        public float x;
        public float y;
        public float radius;
        public float radiusSquared;
        public float softCoreSquared;

        public OboroSampleObstacleModel(float relativeX, float relativeY, float baseRadius, float intensity) {
            this.relativeX = relativeX;
            this.relativeY = relativeY;
            this.baseRadius = baseRadius;
            this.intensity = intensity;
        }

        public void UpdateLayout(float screenWidth, float screenHeight, float radiusScale) {
            x = screenWidth * relativeX;
            y = screenHeight * relativeY;
            radius = baseRadius * radiusScale;
            radiusSquared = radius * radius;
            float softCore = radius * 0.22f;
            softCoreSquared = softCore * softCore;
        }

        public bool Contains(float pointX, float pointY) {
            float dx = pointX - x;
            float dy = pointY - y;
            return dx * dx + dy * dy < radiusSquared;
        }

    }

}
