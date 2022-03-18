using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack:GameAction
{
    float damage;
    float speed;

    public Attack(string name,Character target,ActionType actionType,float damage,float speed):base(name,target,actionType){
        this.damage=damage;
        this.speed=speed;
    }
    
    public float getDamage(){return damage;}
    public float getSpeed(){return speed;}

}
