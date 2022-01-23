using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal:GameAction
{
    private float healingAmount;
    private float chance;

    public Heal(string name,Character target,ActionType actionType,float healingAmount,float chance):base(name,target,actionType){
        this.healingAmount=healingAmount;
        this.chance=chance;
    }

    public float getHealingAmount(){return healingAmount;}
    public float getChance(){return chance;}
}
