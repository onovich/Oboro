using System;
using System.Collections.Generic;
using UnityEngine;

namespace MortiseFrame.Oboro.Sample {

    [Serializable]
    public class OboroSampleContourData {

        public float disturbanceIntensity = 1f;
        public float disturbanceTimeScale = 1f;
        public float disturbancePhase;
        public AnimationCurve disturbanceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float lineThickness = 0.85f;
        public Color contourGradientStart = new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.01f);
        public Color contourGradientEnd = new Color(229f / 255f, 204f / 255f, 170f / 255f, 0.70f);
        public List<OboroSampleObstacleModel> contourObstacles = new List<OboroSampleObstacleModel>();

        public OboroSampleContourData Clone() {
            var clone = new OboroSampleContourData {
                disturbanceIntensity = disturbanceIntensity,
                disturbanceTimeScale = disturbanceTimeScale,
                disturbancePhase = disturbancePhase,
                disturbanceCurve = new AnimationCurve(disturbanceCurve.keys),
                lineThickness = lineThickness,
                contourGradientStart = contourGradientStart,
                contourGradientEnd = contourGradientEnd,
                contourObstacles = new List<OboroSampleObstacleModel>(contourObstacles.Count)
            };

            clone.disturbanceCurve.preWrapMode = disturbanceCurve.preWrapMode;
            clone.disturbanceCurve.postWrapMode = disturbanceCurve.postWrapMode;

            for (int i = 0; i < contourObstacles.Count; i++) {
                clone.contourObstacles.Add(contourObstacles[i].Clone());
            }

            return clone;
        }

    }

}