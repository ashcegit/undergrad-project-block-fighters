using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoNumberFunction : InfoFunction
{
    public override float function(){
        Block block=gameObject.GetComponent<Block>();
        InputFieldHandler inputFieldHandler=block.getSections()[0].getHeader().getInputFieldHandlers()[0];
        float input=float.Parse(inputFieldHandler.getText());
        return input;
    }
}
