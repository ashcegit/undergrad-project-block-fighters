using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNotFunction : LogicFunction
{
    public override bool function(){
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        LogicFunction logicFunction0=inputFieldHandlers[0].getInputBlock().GetComponent<LogicFunction>();
        return(!logicFunction0.function());
    }
}
