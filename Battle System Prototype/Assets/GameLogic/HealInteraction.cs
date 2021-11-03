using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealInteraction:Interaction
{   
    Heal heal;
    float healingAmount;

    public HealInteraction(Character target,Heal heal):base(target){
        this.heal=heal;
        this.healingAmount=heal.getHealingAmount();
        result=InteractionEnum.Hit;
    }

    public Heal getHeal(){return heal;}
    public float getHealingAmount(){return healingAmount;}

}
