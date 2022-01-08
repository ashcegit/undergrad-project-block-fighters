using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIfFunction : ControlFunction
{
    public override int function(Block block,int pointer,List<Block> blockStack){
        Block inputBlock=block.getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        bool input=false;
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        input=logicFunction.function();
        if(input){

        }
        return pointer;
    }
}
