using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIfFunction : ControlFunction
{
    public override int function(int pointer,List<Block> blockStack){
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        if(logicFunction.function()){

        }
        return pointer;
    }
}
