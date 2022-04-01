using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlRepeatFunction : ControlFunction
{
    public const string NAME="Repeat";
    public override string getName(){return NAME;}

    private int loggedPointer;

    private int triggerCount;

    public override int function(int pointer,ref List<Block> blockStack){
        triggerCount=0;
        loggedPointer=++pointer;
        return loggedPointer;
    }

    public override int onRepeat(int pointer,ref List<Block> blockStack){
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        int operand=0;
        Block operandBlock=inputFieldHandlers[0].getInputBlock().GetComponent<Block>();
        if(operandBlock.getBlockType()==BlockType.Math){
            operand=(int)operandBlock.gameObject.GetComponent<MathFunction>().function();
        }else if(operandBlock.getBlockType()==BlockType.Info){
            operand=(int)operandBlock.gameObject.GetComponent<InfoFunction>().function();
        }

        if(triggerCount<operand){
            Terminal.log(TerminalLogType.Control, "Repeat block triggered");
            triggerCount++;
            return loggedPointer;
        }else{
            Terminal.log(TerminalLogType.Control, "Repeat block not triggered");
            return ++pointer;
        }
    }
}
