using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal:GameAction
{
    private float healingAmount;

    public Heal(string name,Character target,ActionType actionType,float healingAmount):base(name,target,actionType){
        this.healingAmount=healingAmount;
    }

    public float getHealingAmount(){return healingAmount;}
}
