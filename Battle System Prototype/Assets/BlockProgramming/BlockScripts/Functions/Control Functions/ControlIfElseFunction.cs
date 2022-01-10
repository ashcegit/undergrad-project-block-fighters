using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIfElseFunction : ControlFunction
{
    private const string NAME="If Else";
    public override string getName(){return NAME;}

    private bool triggered;

    public override int function(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        pointer++;
        if(logicFunction.function()){
            triggered=true;
        }else{
            triggered=false;
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
        }
        return pointer;
    }

    public int onRepeat(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        if(triggered){
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
            triggered=false;
        }
        return ++pointer;
    }
}
