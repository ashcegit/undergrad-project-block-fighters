using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class LogicLessThanFunction : LogicFunction
{
    public override bool function(){
        float operand0=0f;
        float operand1=0f;
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        Block block0=inputFieldHandlers[0].getInputBlock().GetComponent<Block>();
        Block block1=inputFieldHandlers[1].getInputBlock().GetComponent<Block>();
        
        if(block0.getBlockType()==BlockType.Math){
            operand0=block0.gameObject.GetComponent<MathFunction>().function();
        }else if(block0.getBlockType()==BlockType.Info){
            operand0=block0.gameObject.GetComponent<InfoFunction>().function();
        }
        
        if(block1.getBlockType()==BlockType.Math){
            operand1=block0.gameObject.GetComponent<MathFunction>().function();
        }else if(block0.getBlockType()==BlockType.Info){
            operand1=block0.gameObject.GetComponent<InfoFunction>().function();
        }
        bool result = operand0 < operand1;
        Terminal.log(TerminalLogType.Header, "Less Than block returns {0}", result);
        return (result);
    }
}
