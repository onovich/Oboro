using UnityEngine;

namespace Onovich.SDF.Sample {

    public static class SDFSampleBootstrap {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Bootstrap() {
            if (Object.FindFirstObjectByType<SDFSampleEntry>() != null) {
                return;
            }

            Application.targetFrameRate = 60;

            var root = new GameObject("SDFSampleEntry");
            Object.DontDestroyOnLoad(root);
            root.AddComponent<SDFSampleEntry>();
        }

    }

}
