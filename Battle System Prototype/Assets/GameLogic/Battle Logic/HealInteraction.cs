using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealInteraction:Interaction
{   
    Heal heal;
    float healingAmount;
    float chance;

    public HealInteraction(Character target,Heal heal):base(target){
        this.heal=heal;
        this.healingAmount=heal.getHealingAmount();
        this.chance=heal.getChance();
        if(Random.Range(0.0f,1.0f)<=heal.getChance()){result=InteractionEnum.Hit;}
        else{result=InteractionEnum.Miss;}
    }

    public Heal getHeal(){return heal;}
    public float getHealingAmount(){return healingAmount;}
    public float getChance(){return chance;}

}
