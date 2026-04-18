using System.Collections.Generic;
using UnityEngine;
using SdfSample.SdfTopography.Data;

namespace SdfSample.SdfTopography.Interaction
{
    public sealed class SdfObstacleInteractionController
    {
        private SdfObstacleData draggedObstacle;
        private Vector2 dragOffset;

        public void Handle(
            IReadOnlyList<SdfObstacleData> obstacles,
            Vector2 pointer,
            bool pointerDown,
            bool pointerHeld,
            bool pointerUp,
            float screenWidth,
            float screenHeight)
        {
            if (pointerDown)
            {
                for (var index = obstacles.Count - 1; index >= 0; index--)
                {
                    var obstacle = obstacles[index];
                    if (!obstacle.Contains(pointer))
                    {
                        continue;
                    }

                    draggedObstacle = obstacle;
                    dragOffset = pointer - new Vector2(obstacle.X, obstacle.Y);
                    break;
                }
            }

            if (draggedObstacle != null && pointerHeld)
            {
                draggedObstacle.X = pointer.x - dragOffset.x;
                draggedObstacle.Y = pointer.y - dragOffset.y;
                draggedObstacle.RelativeX = Mathf.Clamp01(draggedObstacle.X / screenWidth);
                draggedObstacle.RelativeY = Mathf.Clamp01(draggedObstacle.Y / screenHeight);
            }

            if (pointerUp)
            {
                draggedObstacle = null;
            }
        }
    }
}
