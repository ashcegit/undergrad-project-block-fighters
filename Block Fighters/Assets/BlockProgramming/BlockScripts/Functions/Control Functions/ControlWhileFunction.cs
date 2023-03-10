using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlWhileFunction : ControlFunction
{
    private const string NAME="While";

    public override string getName(){return NAME;}

    private int loggedPointer;

    public override int function(int pointer,ref List<Block> blockStack){
        loggedPointer=++pointer;
        return loggedPointer;
    }

    public override int onRepeat(int pointer,ref List<Block> blockStack){
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        if(logicFunction.function()){
            Terminal.log(TerminalLogType.Control,"While block triggered");
            return loggedPointer;
        }else{
            Terminal.log(TerminalLogType.Control, "While not block triggered");
            return pointer;
        }

    }
}
