using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class LogicTrueFunction : LogicFunction
{
    public override bool function(){
        Terminal.log(TerminalLogType.Header, "True block returns true");
        return true;
    }
}
