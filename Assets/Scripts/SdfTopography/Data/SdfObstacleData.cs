using System;
using UnityEngine;

namespace SdfSample.SdfTopography.Data;

[Serializable]
public sealed class SdfObstacleData
{
    public float RelativeX;
    public float RelativeY;
    public float BaseRadius;
    public float Intensity;
    public float X;
    public float Y;
    public float Radius;

    public SdfObstacleData(float relativeX, float relativeY, float baseRadius, float intensity)
    {
        RelativeX = relativeX;
        RelativeY = relativeY;
        BaseRadius = baseRadius;
        Intensity = intensity;
    }

    public void UpdateLayout(float screenWidth, float screenHeight, float radiusScale)
    {
        X = screenWidth * RelativeX;
        Y = screenHeight * RelativeY;
        Radius = BaseRadius * radiusScale;
    }

    public bool Contains(Vector2 point)
    {
        return Vector2.Distance(point, new Vector2(X, Y)) < Radius;
    }
}
