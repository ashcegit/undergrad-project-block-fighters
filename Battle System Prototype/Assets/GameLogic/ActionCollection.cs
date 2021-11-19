using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionCollection
{
    public static Attack punch(Character target,Character instigator){
        return new Attack("punch",target,ActionType.Physical,01000*instigator.getAttack(),1.2f*instigator.getSpeed());
    }
    public static Attack kick(Character target,Character instigator){
        return new Attack("kick",target,ActionType.Physical,1.1f*instigator.getAttack(),0.6f*instigator.getSpeed());
    }
    public static StatusEffect guard(Character target){
        return new StatusEffect("guard",target,ActionType.Physical,AttributeEnum.Defence,2f,0.7f,1);
    }
    public static Heal healingMagic(Character target){
        return new Heal("healing magic",target,ActionType.Magic,13f);
    }
}
