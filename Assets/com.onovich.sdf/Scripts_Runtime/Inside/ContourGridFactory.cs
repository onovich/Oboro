using System;

namespace Onovich.SDF.Inside {

    internal static class ContourGridFactory {

        internal static void Build(float[] buffer, int columns, int rows, float cellSize, Func<float, float, float> evaluator) {
            for (int row = 0; row <= rows; row++) {
                float y = row * cellSize;
                int rowOffset = row * (columns + 1);
                for (int column = 0; column <= columns; column++) {
                    float x = column * cellSize;
                    buffer[rowOffset + column] = evaluator(x, y);
                }
            }
        }

    }

}
