using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class LogicNotFunction : LogicFunction
{
    public override bool function(){
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        LogicFunction logicFunction0=inputFieldHandlers[0].getInputBlock().GetComponent<LogicFunction>();

        bool result = !logicFunction0.function();
        Terminal.log(TerminalLogType.Header, "Or block returns {0}", result);
        return (result);
    }
}
