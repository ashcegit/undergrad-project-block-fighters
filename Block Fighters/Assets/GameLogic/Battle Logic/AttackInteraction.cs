using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction:Interaction
{   
    private Attack attack;

    private float defenderDefence;
    private float defenderSpeed;

    private float damage;

    public AttackInteraction(Attack attack){
        this.attack=attack;
        this.defenderDefence=attack.getTarget().getDefence();
        this.defenderSpeed=attack.getTarget().getSpeed();
        damage=attack.getDamage()*(100-defenderDefence)/100;
        if(Random.Range(0.0f,attack.getSpeed())<defenderSpeed){
            result=InteractionEnum.Hit;
        }else{
            result=InteractionEnum.Miss;
        }
    }
    public float getDamage(){return damage;}
    public Attack getAttack(){return attack;}
}
