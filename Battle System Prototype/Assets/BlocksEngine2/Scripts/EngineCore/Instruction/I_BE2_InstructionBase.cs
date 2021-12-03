using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BE2_InstructionBase
{
    I_BE2_Instruction Instruction { get; }
    int[] LocationsArray { get; set; }
    I_BE2_Block Block { get; }
    I_BE2_BlocksStack BlocksStack { get; set; }
    I_BE2_TargetObject TargetObject { get; set; }

    I_BE2_BlockSectionHeaderInput[] GetSectionInputs(int sectionIndex);
    I_BE2_BlockSectionHeaderInput[] Section0Inputs{ get; }
    void ExecuteSection(int sectionIndex);
    void ExecuteNextInstruction();

    void OnStackActive();

    void PrepareToPlay();
}
