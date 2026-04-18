using System;
using System.Collections.Generic;
using UnityEngine;
using Onovich.SDF.Inside;

namespace Onovich.SDF {

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

            int requiredSize = (ctx.columns + 1) * (ctx.rows + 1);
            if (ctx.fieldGrid == null || ctx.fieldGrid.Length != requiredSize) {
                ctx.fieldGrid = new float[requiredSize];
            }
        }

        public void BuildField(Func<float, float, float> evaluator) {
            ContourGridFactory.Build(ctx.fieldGrid, ctx.columns, ctx.rows, ctx.gridResolution, evaluator);
        }

        public void EmitContour(IReadOnlyList<ContourLevelModel> contourLevels, Action<ContourLevelModel, Vector2, Vector2> emitHandle) {
            ContourMarchingSquaresFactory.Emit(ctx.fieldGrid, ctx.columns, ctx.rows, ctx.gridResolution, contourLevels, emitHandle);
        }

    }

}
