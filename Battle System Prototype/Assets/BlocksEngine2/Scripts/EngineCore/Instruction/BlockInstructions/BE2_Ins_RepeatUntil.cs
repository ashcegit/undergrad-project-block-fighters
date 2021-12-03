using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_RepeatUntil : BE2_InstructionBase, I_BE2_Instruction
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

    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    //protected override void OnStop()
    //{
    //
    //}

    public void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        if (_value != "1" && _value != "true")
        {
            ExecuteSection(0);
        }
        else
        {
            ExecuteNextInstruction();
        }
    }
}
