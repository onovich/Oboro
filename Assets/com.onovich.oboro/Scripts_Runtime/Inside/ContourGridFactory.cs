using System;

namespace Onovich.Oboro.Inside {

    internal static class ContourGridFactory {

        internal static void Build(float[] buffer, float[] xPositions, float[] yPositions, int columns, int rows, int rowStride, Func<float, float, float> evaluator) {
            for (int row = 0; row <= rows; row++) {
                float y = yPositions[row];
                int rowOffset = row * rowStride;
                for (int column = 0; column <= columns; column++) {
                    buffer[rowOffset + column] = evaluator(xPositions[column], y);
                }
            }
        }

    }

}
