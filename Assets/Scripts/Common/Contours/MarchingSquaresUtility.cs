using System;
using System.Collections.Generic;
using UnityEngine;

namespace SdfSample.Common.Contours
{
    public static class MarchingSquaresUtility
    {
        private static readonly int[][][] Cases =
        {
            Array.Empty<int[]>(), new[] { new[] { 3, 2 } }, new[] { new[] { 2, 1 } }, new[] { new[] { 3, 1 } },
            new[] { new[] { 1, 0 } }, new[] { new[] { 3, 0 }, new[] { 1, 2 } }, new[] { new[] { 2, 0 } }, new[] { new[] { 3, 0 } },
            new[] { new[] { 0, 3 } }, new[] { new[] { 0, 2 } }, new[] { new[] { 0, 1 }, new[] { 2, 3 } }, new[] { new[] { 0, 1 } },
            new[] { new[] { 1, 3 } }, new[] { new[] { 1, 2 } }, new[] { new[] { 2, 3 } }, Array.Empty<int[]>()
        };

        public static void EmitContourSegments(
            float[] grid,
            int columns,
            int rows,
            float cellSize,
            IReadOnlyList<ContourLevel> levels,
            Action<ContourLevel, Vector2, Vector2> emitSegment)
        {
            for (var row = 0; row < rows; row++)
            {
                var rowOffset = row * (columns + 1);
                var nextRowOffset = (row + 1) * (columns + 1);

                for (var column = 0; column < columns; column++)
                {
                    var valueTopLeft = grid[rowOffset + column];
                    var valueTopRight = grid[rowOffset + column + 1];
                    var valueBottomLeft = grid[nextRowOffset + column];
                    var valueBottomRight = grid[nextRowOffset + column + 1];

                    var minValue = Mathf.Min(valueTopLeft, valueTopRight, valueBottomLeft, valueBottomRight);
                    var maxValue = Mathf.Max(valueTopLeft, valueTopRight, valueBottomLeft, valueBottomRight);
                    var x = column * cellSize;
                    var y = row * cellSize;

                    for (var levelIndex = 0; levelIndex < levels.Count; levelIndex++)
                    {
                        var level = levels[levelIndex];
                        if (level.Value <= minValue || level.Value > maxValue)
                        {
                            continue;
                        }

                        var state = 0;
                        if (valueTopLeft > level.Value) state |= 8;
                        if (valueTopRight > level.Value) state |= 4;
                        if (valueBottomRight > level.Value) state |= 2;
                        if (valueBottomLeft > level.Value) state |= 1;

                        var lineSegments = Cases[state];
                        for (var segmentIndex = 0; segmentIndex < lineSegments.Length; segmentIndex++)
                        {
                            var line = lineSegments[segmentIndex];
                            var start = GetEdgePosition(line[0], x, y, cellSize, valueTopLeft, valueTopRight, valueBottomRight, valueBottomLeft, level.Value);
                            var end = GetEdgePosition(line[1], x, y, cellSize, valueTopLeft, valueTopRight, valueBottomRight, valueBottomLeft, level.Value);
                            emitSegment(level, start, end);
                        }
                    }
                }
            }
        }

        private static Vector2 GetEdgePosition(int edge, float x, float y, float size, float valueTopLeft, float valueTopRight, float valueBottomRight, float valueBottomLeft, float level)
        {
            var interpolation = 0.5f;
            if (edge == 0 && !Mathf.Approximately(valueTopRight, valueTopLeft)) interpolation = (level - valueTopLeft) / (valueTopRight - valueTopLeft);
            if (edge == 1 && !Mathf.Approximately(valueBottomRight, valueTopRight)) interpolation = (level - valueTopRight) / (valueBottomRight - valueTopRight);
            if (edge == 2 && !Mathf.Approximately(valueBottomRight, valueBottomLeft)) interpolation = (level - valueBottomLeft) / (valueBottomRight - valueBottomLeft);
            if (edge == 3 && !Mathf.Approximately(valueBottomLeft, valueTopLeft)) interpolation = (level - valueTopLeft) / (valueBottomLeft - valueTopLeft);

            interpolation = Mathf.Clamp01(interpolation);

            switch (edge)
            {
                case 0:
                    return new Vector2(x + interpolation * size, y);
                case 1:
                    return new Vector2(x + size, y + interpolation * size);
                case 2:
                    return new Vector2(x + interpolation * size, y + size);
                case 3:
                    return new Vector2(x, y + interpolation * size);
                default:
                    return new Vector2(x, y);
            }
        }
    }
}
