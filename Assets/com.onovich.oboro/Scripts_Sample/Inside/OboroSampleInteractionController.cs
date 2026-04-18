using UnityEngine;

namespace Onovich.Oboro.Sample.Inside {

    internal class OboroSampleInteractionController {

        OboroSampleObstacleModel draggedObstacle;
        Vector2 dragOffset;

        internal void Tick(OboroSampleObstacleModel[] obstacles, Vector2 pointer, bool pointerDown, bool pointerHeld, bool pointerUp, int screenWidth, int screenHeight) {
            if (pointerDown) {
                for (int i = obstacles.Length - 1; i >= 0; i--) {
                    var obstacle = obstacles[i];
                    if (!obstacle.Contains(pointer)) {
                        continue;
                    }

                    draggedObstacle = obstacle;
                    dragOffset = pointer - new Vector2(obstacle.x, obstacle.y);
                    break;
                }
            }

            if (draggedObstacle != null && pointerHeld) {
                draggedObstacle.x = pointer.x - dragOffset.x;
                draggedObstacle.y = pointer.y - dragOffset.y;
                draggedObstacle.relativeX = Mathf.Clamp01(draggedObstacle.x / screenWidth);
                draggedObstacle.relativeY = Mathf.Clamp01(draggedObstacle.y / screenHeight);
            }

            if (pointerUp) {
                draggedObstacle = null;
            }
        }

    }

}
