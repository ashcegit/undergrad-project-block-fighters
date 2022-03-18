using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoNumberFunction : InfoFunction
{
    public override float function(){
        Block block=gameObject.GetComponent<Block>();
        InputFieldHandler inputFieldHandler=block.getSections()[0].getHeader().getInputFieldHandlers()[0];
        float input=float.Parse(inputFieldHandler.getText());
        Terminal.log(TerminalLogType.Header, "Number block returns {0}", input);
        return input;
    }
}
