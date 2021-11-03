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
        damage=10*attack.getDamage()/defenderDefence;
    }

    public new InteractionEnum getResult(){
        if(Random.Range(0,attack.getSpeed())<defenderSpeed){
            return InteractionEnum.Hit;
        }else{
            return InteractionEnum.Miss;
        }
    }
    public float getDamage(){return damage;}
    public Attack getAttack(){return attack;}
}
