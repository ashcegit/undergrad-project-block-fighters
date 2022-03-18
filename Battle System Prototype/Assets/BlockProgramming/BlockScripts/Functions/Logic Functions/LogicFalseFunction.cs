using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class LogicFalseFunction : LogicFunction
{
    public override bool function(){
        Terminal.log(TerminalLogType.Header, "False block returns false");
        return false;
    }
}
