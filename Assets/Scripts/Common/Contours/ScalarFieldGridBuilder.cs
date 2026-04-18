using System;

namespace SdfSample.Common.Contours;

public static class ScalarFieldGridBuilder
{
    public static void Build(float[] buffer, int columns, int rows, float cellSize, Func<float, float, float> evaluator)
    {
        for (var row = 0; row <= rows; row++)
        {
            var y = row * cellSize;
            var rowOffset = row * (columns + 1);
            for (var column = 0; column <= columns; column++)
            {
                var x = column * cellSize;
                buffer[rowOffset + column] = evaluator(x, y);
            }
        }
    }
}
