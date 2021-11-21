using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction:Interaction
{   
    private Attack attack;

    private float defenderDefence;
    private float defenderSpeed;

    private float damage;

    public AttackInteraction(Character target,Attack attack):base(target){
        this.attack=attack;
        this.defenderDefence=target.getDefence();
        this.defenderSpeed=target.getSpeed();
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
