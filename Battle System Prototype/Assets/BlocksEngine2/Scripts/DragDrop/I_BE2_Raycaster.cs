using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_Raycaster
{
    I_BE2_Drag GetDragAtPosition(Vector2 position);
    I_BE2_Spot GetSpotAtPosition(Vector2 position);
    I_BE2_Spot FindClosestSpotOfType<T>(I_BE2_Drag drag, float maxDistance);
    I_BE2_Spot FindClosestSpotForBlock(I_BE2_Drag drag, float maxDistance);
}
