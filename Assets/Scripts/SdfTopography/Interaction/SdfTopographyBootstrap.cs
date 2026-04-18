using UnityEngine;
using SdfSample.SdfTopography.Rendering;

namespace SdfSample.SdfTopography.Interaction
{
    public static class SdfTopographyBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Object.FindFirstObjectByType<SdfTopographyRenderer>() != null)
            {
                return;
            }

            Application.targetFrameRate = 60;

            var root = new GameObject("SDF Topography Renderer");
            Object.DontDestroyOnLoad(root);
            root.AddComponent<SdfTopographyRenderer>();
        }
    }
}
