using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_BreakLoop : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //    
    //}

    //protected override void OnStart()
    //{
    //    
    //}

    //protected override void OnUpdate()
    //{
    //
    //}

    I_BE2_Instruction _parentLoopInstruction;

    protected override void OnButtonStop()
    {
        _parentLoopInstruction = BE2_BlockUtils.GetParentInstructionOfType(this, BlockTypeEnum.loop);
    }

    public override void OnStackActive()
    {
        _parentLoopInstruction = BE2_BlockUtils.GetParentInstructionOfType(this, BlockTypeEnum.loop);
    }

    public void Function()
    {
        if (_parentLoopInstruction != null)
        {
            _parentLoopInstruction.InstructionBase.ExecuteNextInstruction();
        }
        else
        {
            ExecuteNextInstruction();
        }
    }
}
