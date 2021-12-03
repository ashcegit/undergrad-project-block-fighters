using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_Instruction
{
    I_BE2_InstructionBase InstructionBase { get; }
    bool ExecuteInUpdate { get; }

    string Operation();
    void Function();
}
