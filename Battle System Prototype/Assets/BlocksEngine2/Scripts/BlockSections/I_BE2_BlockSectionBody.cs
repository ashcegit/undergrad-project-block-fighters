using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface I_BE2_BlockSectionBody
{
    RectTransform RectTransform { get; }
    Vector2 Size { get; }
    I_BE2_Block[] ChildBlocksArray { get; } 
    I_BE2_Spot Spot { get; set; }
    I_BE2_BlockSection BlockSection { get; }
    int ChildBlocksCount { get; }
    Shadow Shadow { get; }

    void UpdateChildBlocksList();
    void UpdateLayout();
}
