using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_Drag
{
    void OnPointerDown();
    void OnRightPointerDownOrHold();
    void OnDrag();
    void OnPointerUp();
    Transform Transform { get; }
    Vector2 RayPoint { get; }
    I_BE2_Block Block { get; }
    List<I_BE2_Block> ChildBlocks { get; }
}
