using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRepeatFunction : ControlFunction
{
    public const string NAME="Repeat";
    public string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        int operand=0;
        Block operandBlock=inputFieldHandlers[0].getInputBlock().GetComponent<Block>();
        if(operandBlock.getBlockType()==BlockType.Math){
            operand=(int)operandBlock.gameObject.GetComponent<MathFunction>().function();
        }else if(operandBlock.getBlockType()==BlockType.Info){
            operand=(int)operandBlock.gameObject.GetComponent<InfoFunction>().function();
        }
        pointer++;
        int endPointer=pointer;
        List<Block> subBlockStack=new List<Block>();
        while(blockStack[endPointer].getStartBlock()!=thisBlock){
            endPointer++;
        }
        subBlockStack=blockStack.GetRange(pointer,endPointer);
        for(int i=0;i<operand;i++){
            Debug.Log("Adding");
            blockStack.InsertRange(pointer,subBlockStack);
        }
        return pointer;
    }
}
