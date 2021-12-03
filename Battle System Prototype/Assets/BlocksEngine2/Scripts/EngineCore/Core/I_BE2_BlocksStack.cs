using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_BlocksStack
{
    I_BE2_TargetObject TargetObject { get; set; }
    bool IsActive { get; set; }
    I_BE2_Instruction TriggerInstruction { get; }
    bool MarkToAdd { get; set; }
    I_BE2_Instruction[] InstructionsArray { get; set; }
    int OverflowGuard { get; set; }

    int Pointer { get; set; }

    void Execute();
    void PopulateStack();
}
