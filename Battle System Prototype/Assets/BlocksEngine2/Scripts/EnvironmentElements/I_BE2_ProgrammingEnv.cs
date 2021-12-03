using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_ProgrammingEnv
{
    Transform Transform { get; }
    List<I_BE2_Block> BlocksList { get; }
    I_BE2_TargetObject TargetObject { get; }
    void UpdateBlocksList();
    void ClearBlocks();
}
