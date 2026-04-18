using UnityEngine;

namespace MortiseFrame.Oboro.Sample.Inside {

    internal class OboroSampleInteractionController {

        OboroSampleObstacleModel draggedObstacle;
        Vector2 dragOffset;

        internal bool Tick(OboroSampleObstacleModel[] obstacles, Vector2 pointer, bool pointerDown, bool pointerHeld, bool pointerUp, int screenWidth, int screenHeight) {
            float pointerX = pointer.x;
            float pointerY = pointer.y;
            bool obstacleStateChanged = false;

            if (pointerDown) {
                for (int i = obstacles.Length - 1; i >= 0; i--) {
                    var obstacle = obstacles[i];
                    if (!obstacle.Contains(pointerX, pointerY)) {
                        continue;
                    }

                    draggedObstacle = obstacle;
                    dragOffset = new Vector2(pointerX - obstacle.x, pointerY - obstacle.y);
                    break;
                }
            }

            if (draggedObstacle != null && pointerHeld) {
                float nextX = pointerX - dragOffset.x;
                float nextY = pointerY - dragOffset.y;

                if (!Mathf.Approximately(draggedObstacle.x, nextX) || !Mathf.Approximately(draggedObstacle.y, nextY)) {
                    draggedObstacle.x = nextX;
                    draggedObstacle.y = nextY;
                    draggedObstacle.relativeX = Mathf.Clamp01(draggedObstacle.x / screenWidth);
                    draggedObstacle.relativeY = Mathf.Clamp01(draggedObstacle.y / screenHeight);
                    obstacleStateChanged = true;
                }
            }

            if (pointerUp) {
                draggedObstacle = null;
            }

            return obstacleStateChanged;
        }

    }

}
