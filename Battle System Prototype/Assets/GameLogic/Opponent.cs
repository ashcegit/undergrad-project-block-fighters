using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class Opponent:Character
{   
    public Opponent(string characterName,
                  float baseMaxHealth,
                  float baseAttack,
                  float baseDefence,
                  float baseSpeed):
                  base(characterName,
                       baseMaxHealth,
                       baseAttack,
                       baseDefence,
                       baseSpeed){
    }

    public Attack punch(Character target){
        return new Attack("punch",target,ActionType.Physical,0.8f*getAttack(),getSpeed()*1.2f);
    }
    public Attack kick(Character target){
        return new Attack("kick",target,ActionType.Physical,getAttack(),getSpeed()*0.8f);
    }
    public StatusEffect weaken(Character target){
        return new StatusEffect("weaken",target,ActionType.Magic,AttributeEnum.Attack,0.2f,0.8f,4);
    }
    public StatusEffect tough_pill(Character target){
        return new StatusEffect("tough_pill",target,ActionType.Science,AttributeEnum.Max_Health,1.4f,0.7f,5);
    }
    public Heal plaster(Character target){
        return new Heal("plaster",target,ActionType.Physical,12f);
    }
}
