using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{   
    protected InteractionEnum result;

    public void setResult(InteractionEnum result){this.result=result;}
    public InteractionEnum getResult(){return result;}
}
