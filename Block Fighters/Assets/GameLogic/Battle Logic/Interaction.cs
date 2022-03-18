using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{   
    protected Character target;
    protected InteractionEnum result;

    public Interaction(Character target){
        this.target=target;
    }

    public void setResult(InteractionEnum result){this.result=result;}
    public InteractionEnum getResult(){return result;}
    public void setTarget(Character target){this.target=target;}
    public Character getTarget(){return target;}
}
