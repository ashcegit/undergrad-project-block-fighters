using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathSumFunction : MathFunction
{
    public override float function(){
        List<InputFieldHandler> inputFieldHandlers=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers();
        MathFunction mathFunction0=inputFieldHandlers[0].getInputBlock().GetComponent<MathFunction>();
        MathFunction mathFunction1=inputFieldHandlers[1].getInputBlock().GetComponent<MathFunction>();
        return(mathFunction0.function()+mathFunction1.function());
    }
}
