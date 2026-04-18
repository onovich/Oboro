using System;
using System.Collections.Generic;
using UnityEngine;
using MortiseFrame.Oboro.Inside;

namespace MortiseFrame.Oboro {

    public class ContourCore {

        ContourCoreContext ctx = new ContourCoreContext();

        public int ScreenWidth => ctx.screenWidth;
        public int ScreenHeight => ctx.screenHeight;
        public int Columns => ctx.columns;
        public int Rows => ctx.rows;
        public int GridResolution => ctx.gridResolution;

        public void Resize(int screenWidth, int screenHeight, int gridResolution) {
            ctx.screenWidth = Mathf.Max(1, screenWidth);
            ctx.screenHeight = Mathf.Max(1, screenHeight);
            ctx.gridResolution = Mathf.Max(1, gridResolution);
            ctx.columns = Mathf.CeilToInt(ctx.screenWidth / (float)ctx.gridResolution);
            ctx.rows = Mathf.CeilToInt(ctx.screenHeight / (float)ctx.gridResolution);
            ctx.rowStride = ctx.columns + 1;

            int requiredSize = ctx.rowStride * (ctx.rows + 1);
            if (ctx.fieldGrid == null || ctx.fieldGrid.Length != requiredSize) {
                ctx.fieldGrid = new float[requiredSize];
            }

            if (ctx.xPositions == null || ctx.xPositions.Length != ctx.rowStride) {
                ctx.xPositions = new float[ctx.rowStride];
            }

            int yCount = ctx.rows + 1;
            if (ctx.yPositions == null || ctx.yPositions.Length != yCount) {
                ctx.yPositions = new float[yCount];
            }

            for (int column = 0; column < ctx.rowStride; column++) {
                ctx.xPositions[column] = column * ctx.gridResolution;
            }

            for (int row = 0; row < yCount; row++) {
                ctx.yPositions[row] = row * ctx.gridResolution;
            }
        }

        public void BuildField(Func<float, float, float> evaluator) {
            ContourGridFactory.Build(ctx.fieldGrid, ctx.xPositions, ctx.yPositions, ctx.columns, ctx.rows, ctx.rowStride, evaluator);
        }

        public void EmitContour(IReadOnlyList<ContourLevelModel> contourLevels, Action<ContourLevelModel, Vector2, Vector2> emitHandle) {
            ContourMarchingSquaresFactory.Emit(ctx.fieldGrid, ctx.xPositions, ctx.yPositions, ctx.columns, ctx.rows, ctx.rowStride, ctx.gridResolution, contourLevels, emitHandle);
        }

    }

}
