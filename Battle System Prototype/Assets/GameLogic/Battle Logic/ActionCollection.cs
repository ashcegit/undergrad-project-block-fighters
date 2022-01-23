using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionCollection
{
    public static Attack punch(Character target,Character instigator){
        return new Attack("punch",target,ActionType.Physical,0.8f*instigator.getAttack(),1.2f*instigator.getSpeed());
    }
    public static Attack kick(Character target,Character instigator){
        return new Attack("kick",target,ActionType.Physical,1.1f*instigator.getAttack(),0.6f*instigator.getSpeed());
    }
    public static Attack fireball(Character target,Character instigator){
        return new Attack("fireball",target,ActionType.Magic,1.5f*instigator.getAttack(),0.4f*instigator.getSpeed());
    }
    public static Attack trebuchet(Character target,Character instigator){
        return new Attack("trebuchet",target,ActionType.Science,2f*instigator.getAttack(),0.2f*instigator.getSpeed());
    }

    public static Heal healingSpell(Character target){
        return new Heal("healing spell",target,ActionType.Magic,20f,0.4f);
    }
    public static Heal plaster(Character target){
        return new Heal("plaster",target,ActionType.Physical,10f,0.65f);
    }

    public static StatusEffect guard(Character target){
        return new StatusEffect("guard",target,ActionType.Physical,AttributeEnum.Defence,1.3f,0.6f,1);
    }
    public static StatusEffect wardSpell(Character target){
        return new StatusEffect("ward spell",target,ActionType.Magic,AttributeEnum.Defence,1.3f,0.5f,4);
    }
    public static StatusEffect poison(Character target){
        return new StatusEffect("poison",target,ActionType.Science,AttributeEnum.MaxHealth,1.3f,0.5f,4);
    }
}