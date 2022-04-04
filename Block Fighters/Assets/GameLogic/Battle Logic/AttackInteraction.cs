using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction:Interaction
{   
    private Attack attack;

    private float targetDefence;
    private float targetSpeed;

    private float damage;

    public AttackInteraction(Attack attack){
        this.attack=attack;
        this.targetDefence=attack.getTarget().getDefence();
        this.targetSpeed=attack.getTarget().getSpeed();
        damage=attack.getDamage()*(200- targetDefence) /200;
        if(Random.Range(0.0f,attack.getSpeed())> targetSpeed) {
            result=InteractionEnum.Hit;
        }else{
            result=InteractionEnum.Miss;
        }
    }
    public float getDamage(){return damage;}
    public Attack getAttack(){return attack;}
}
