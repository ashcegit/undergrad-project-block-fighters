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
        MathFunction mathFunction=inputFieldHandlers[0].getInputBlock().GetComponent<MathFunction>();
        int arg=(int)mathFunction.function();
        int endPointer=pointer;
        List<Block> subBlockStack=new List<Block>();
        while(blockStack[endPointer]!=thisBlock){
            endPointer++;
        }
        subBlockStack=blockStack.GetRange(pointer,endPointer);
        for(int i=0;i<arg;i++){
            blockStack.InsertRange(pointer,subBlockStack);
        }
        return ++pointer;
    }
}
