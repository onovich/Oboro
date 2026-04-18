using UnityEngine;

namespace SdfSample.Common.Contours
{
    public readonly struct ContourLevel
    {
        public readonly float Value;
        public readonly Color Color;

        public ContourLevel(float value, Color color)
        {
            Value = value;
            Color = color;
        }
    }
}
