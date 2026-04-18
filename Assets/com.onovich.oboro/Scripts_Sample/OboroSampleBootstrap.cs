using UnityEngine;

namespace Onovich.Oboro.Sample {

    public static class OboroSampleBootstrap {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Bootstrap() {
            if (Object.FindFirstObjectByType<OboroSampleEntry>() != null) {
                return;
            }

            Application.targetFrameRate = 60;

            var root = new GameObject("OboroSampleEntry");
            Object.DontDestroyOnLoad(root);
            root.AddComponent<OboroSampleEntry>();
        }

    }

}
