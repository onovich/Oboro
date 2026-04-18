using UnityEngine;

namespace MortiseFrame.Oboro {

    public class ContourRendererCore {

        Material lineMaterial;

        public bool Prepare() {
            if (lineMaterial != null) {
                return true;
            }

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            if (shader == null) {
                return false;
            }

            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
            return true;
        }

        public void Begin(int screenWidth, int screenHeight) {
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0f, screenWidth, screenHeight, 0f);
            GL.Begin(GL.LINES);
        }

        public void DrawLine(ContourLevelModel level, Vector2 start, Vector2 end) {
            GL.Color(level.color);
            GL.Vertex3(start.x, start.y, 0f);
            GL.Vertex3(end.x, end.y, 0f);
        }

        public void End() {
            GL.End();
            GL.PopMatrix();
        }

        public void TearDown() {
            if (lineMaterial != null) {
                Object.Destroy(lineMaterial);
                lineMaterial = null;
            }
        }

    }

}
