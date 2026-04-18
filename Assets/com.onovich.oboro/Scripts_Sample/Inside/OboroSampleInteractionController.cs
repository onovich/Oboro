using UnityEngine;

namespace Onovich.Oboro.Sample.Inside {

    internal class OboroSampleInteractionController {

        OboroSampleObstacleModel draggedObstacle;
        Vector2 dragOffset;

        internal void Tick(OboroSampleObstacleModel[] obstacles, Vector2 pointer, bool pointerDown, bool pointerHeld, bool pointerUp, int screenWidth, int screenHeight) {
            float pointerX = pointer.x;
            float pointerY = pointer.y;

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
                draggedObstacle.x = pointerX - dragOffset.x;
                draggedObstacle.y = pointerY - dragOffset.y;
                draggedObstacle.relativeX = Mathf.Clamp01(draggedObstacle.x / screenWidth);
                draggedObstacle.relativeY = Mathf.Clamp01(draggedObstacle.y / screenHeight);
            }

            if (pointerUp) {
                draggedObstacle = null;
            }
        }

    }

}
