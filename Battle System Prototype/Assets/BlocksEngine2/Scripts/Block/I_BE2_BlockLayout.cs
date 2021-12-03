using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_BlockLayout
{
    RectTransform RectTransform { get; set; }
    I_BE2_BlockSection[] SectionsArray { get; }
    Color Color { get; set; }
    Vector2 Size { get; }

    void UpdateLayout();
}
