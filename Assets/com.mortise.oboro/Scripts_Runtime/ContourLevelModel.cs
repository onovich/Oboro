using UnityEngine;

namespace MortiseFrame.Oboro {

    public readonly struct ContourLevelModel {

        public readonly float value;
        public readonly Color color;

        public ContourLevelModel(float value, Color color) {
            this.value = value;
            this.color = color;
        }

    }

}
