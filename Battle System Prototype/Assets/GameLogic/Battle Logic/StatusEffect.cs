using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect:GameAction
{
    private AttributeEnum attribute;
    private float multiplier;
    private float chance;
    private int turns;

    public StatusEffect(string name,Character target,ActionType actionType,AttributeEnum attribute,float multiplier,float chance,int turns):base(name,target,actionType){
        this.attribute=attribute;
        this.multiplier=multiplier;
        this.chance=chance;
        this.turns=turns;
    }

    public AttributeEnum getAttribute(){return attribute;}
    public float getMultiplier(){return multiplier;}
    public float getChance(){return chance;}
    public int getTurns(){return turns;}
}
