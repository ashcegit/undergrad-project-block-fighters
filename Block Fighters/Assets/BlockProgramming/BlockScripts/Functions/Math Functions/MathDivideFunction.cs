using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class MathDivideFunction : MathFunction
{
    public override float function(){
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        MathFunction mathFunction0=inputFieldHandlers[0].getInputBlock().GetComponent<MathFunction>();
        MathFunction mathFunction1=inputFieldHandlers[1].getInputBlock().GetComponent<MathFunction>();
        
        float operand0=mathFunction0.function();
        float operand1=mathFunction1.function();

        float result = 1f*(operand0 / operand1);
        Terminal.log(TerminalLogType.Header, "Divide block returns {0}", result);
        return (result);
    }
}
