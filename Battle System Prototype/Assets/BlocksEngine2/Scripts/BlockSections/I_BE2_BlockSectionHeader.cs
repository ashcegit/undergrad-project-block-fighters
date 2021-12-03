using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface I_BE2_BlockSectionHeader
{
    I_BE2_BlockSectionHeaderItem[] ItemsArray { get; }
    I_BE2_BlockSectionHeaderInput[] InputsArray { get; }
    Vector2 Size { get; }
    Shadow Shadow { get; }

    void UpdateLayout();
    void UpdateItemsArray();
    void UpdateInputsArray();
}
