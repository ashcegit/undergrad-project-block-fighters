using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIfElseFunction : ControlFunction
{
    private const string NAME="If Else";
    public string getName(){return NAME;}
    public override int function(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        pointer++;
        if(!logicFunction.function()){
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
        }else{
            int tempPointer=pointer;
            while(blockStack[tempPointer].getStartBlock()!=thisBlock){
                tempPointer++;
            }
            while(blockStack[tempPointer].getStartBlock()!=thisBlock){
                blockStack.RemoveAt(tempPointer);
                tempPointer++;
            }
        }
        return pointer;
    }
}
