using System;
using System.Collections.Generic;
using UnityEngine;

namespace Onovich.Oboro.Inside {

    internal static class ContourMarchingSquaresFactory {

        static readonly int[][][] cases = {
            Array.Empty<int[]>(), new[] { new[] { 3, 2 } }, new[] { new[] { 2, 1 } }, new[] { new[] { 3, 1 } },
            new[] { new[] { 1, 0 } }, new[] { new[] { 3, 0 }, new[] { 1, 2 } }, new[] { new[] { 2, 0 } }, new[] { new[] { 3, 0 } },
            new[] { new[] { 0, 3 } }, new[] { new[] { 0, 2 } }, new[] { new[] { 0, 1 }, new[] { 2, 3 } }, new[] { new[] { 0, 1 } },
            new[] { new[] { 1, 3 } }, new[] { new[] { 1, 2 } }, new[] { new[] { 2, 3 } }, Array.Empty<int[]>()
        };

        internal static void Emit(float[] grid, float[] xPositions, float[] yPositions, int columns, int rows, int rowStride, float cellSize, IReadOnlyList<ContourLevelModel> contourLevels, Action<ContourLevelModel, Vector2, Vector2> emitHandle) {
            int contourLevelCount = contourLevels.Count;
            for (int row = 0; row < rows; row++) {
                int rowOffset = row * rowStride;
                int nextRowOffset = (row + 1) * rowStride;
                float y = yPositions[row];

                for (int column = 0; column < columns; column++) {
                    float vTL = grid[rowOffset + column];
                    float vTR = grid[rowOffset + column + 1];
                    float vBL = grid[nextRowOffset + column];
                    float vBR = grid[nextRowOffset + column + 1];

                    float minV = Mathf.Min(vTL, vTR, vBL, vBR);
                    float maxV = Mathf.Max(vTL, vTR, vBL, vBR);
                    float x = xPositions[column];

                    for (int i = 0; i < contourLevelCount; i++) {
                        var level = contourLevels[i];
                        if (level.value <= minV || level.value > maxV) {
                            if (level.value > maxV) {
                                break;
                            }
                            continue;
                        }

                        int state = 0;
                        if (vTL > level.value) state |= 8;
                        if (vTR > level.value) state |= 4;
                        if (vBR > level.value) state |= 2;
                        if (vBL > level.value) state |= 1;

                        int[][] lineSegments = cases[state];
                        for (int segmentIndex = 0; segmentIndex < lineSegments.Length; segmentIndex++) {
                            int[] line = lineSegments[segmentIndex];
                            Vector2 start = GetEdgePosition(line[0], x, y, cellSize, vTL, vTR, vBR, vBL, level.value);
                            Vector2 end = GetEdgePosition(line[1], x, y, cellSize, vTL, vTR, vBR, vBL, level.value);
                            emitHandle(level, start, end);
                        }
                    }
                }
            }
        }

        static Vector2 GetEdgePosition(int edge, float x, float y, float size, float vTL, float vTR, float vBR, float vBL, float level) {
            float t = 0.5f;
            if (edge == 0 && !Mathf.Approximately(vTR, vTL)) t = (level - vTL) / (vTR - vTL);
            if (edge == 1 && !Mathf.Approximately(vBR, vTR)) t = (level - vTR) / (vBR - vTR);
            if (edge == 2 && !Mathf.Approximately(vBR, vBL)) t = (level - vBL) / (vBR - vBL);
            if (edge == 3 && !Mathf.Approximately(vBL, vTL)) t = (level - vTL) / (vBL - vTL);

            t = Mathf.Clamp01(t);

            switch (edge) {
                case 0:
                    return new Vector2(x + t * size, y);
                case 1:
                    return new Vector2(x + size, y + t * size);
                case 2:
                    return new Vector2(x + t * size, y + size);
                case 3:
                    return new Vector2(x, y + t * size);
                default:
                    return new Vector2(x, y);
            }
        }

    }

}
