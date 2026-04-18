using UnityEngine;

namespace MortiseFrame.Oboro.Sample {

    [CreateAssetMenu(fileName = "OboroSampleContourData", menuName = "MortiseFrame/Oboro/Sample Contour Data")]
    public sealed class OboroSampleContourDataAsset : ScriptableObject {

        public OboroSampleContourData data = new OboroSampleContourData();

    }

}