using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Op_Variable : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //
    //}
    //

    protected override void OnStart()
    {
        _variablesManager = BE2_VariablesManager.instance;
    }

    //protected override void OnUpdate()
    //{
    //    
    //}

    BE2_VariablesManager _variablesManager;
    I_BE2_BlockSectionHeaderInput _input0;

    //protected override void OnStop()
    //{
    //
    //}

    public string Operation()
    {
        _input0 = Section0Inputs[0];
        return _variablesManager.GetVariableStringValue(_input0.StringValue);
    }
}
