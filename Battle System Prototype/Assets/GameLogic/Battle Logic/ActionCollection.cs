using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionCollection
{
    //Collection of methods invoked by gameAction blocks

    //Attacks
    public static Attack punch(Character target,Character instigator){
        return new Attack("punch",target,ActionType.Physical,0.8f*instigator.getAttack(),1.2f*instigator.getSpeed());
    }
    public static Attack kick(Character target,Character instigator){
        return new Attack("kick",target,ActionType.Physical,1.1f*instigator.getAttack(),0.6f*instigator.getSpeed());
    }
    public static Attack fireball(Character target,Character instigator){
        return new Attack("fireball",target,ActionType.Magic,1.5f*instigator.getAttack(),0.4f*instigator.getSpeed());
    }
    public static Attack catapult(Character target,Character instigator){
        return new Attack("catapult",target,ActionType.Science,2f*instigator.getAttack(),0.2f*instigator.getSpeed());
    }
    public static Attack lightningStrike(Character target,Character instigator) {
        return new Attack("lightning strike", target, ActionType.Magic, 0.7f * instigator.getAttack(), 1.25f * instigator.getSpeed());
    }
    public static Attack laser(Character target,Character instigator) {
        return new Attack("laser", target, ActionType.Science, instigator.getAttack(), 0.9f * instigator.getSpeed());
    }

    //Heals
    public static Heal healingSpell(Character target){
        return new Heal("healing spell",target,ActionType.Magic,20f,0.5f);
    }
    public static Heal plaster(Character target){
        return new Heal("plaster",target,ActionType.Physical,15f,0.65f);
    }
    public static Heal rest(Character target) {
        return new Heal("rest", target, ActionType.Physical, 10f, 0.75f);
    }
    public static Heal medicine(Character target) {
        return new Heal("medicine", target, ActionType.Science, 30f, 0.4f);
    }
    public static Heal sandwich(Character target) {
        return new Heal("sandwich", target, ActionType.Physical, 12f, 0.7f);
    }

    //Status Effects
    public static StatusEffect guard(Character target){
        return new StatusEffect("guard",target,ActionType.Physical,AttributeEnum.Defence,2f,0.6f,1);
    }
    public static StatusEffect shield(Character target) {
        return new StatusEffect("shield", target, ActionType.Physical, AttributeEnum.Defence, 1.5f, 0.7f, 2);
    }
    public static StatusEffect wardSpell(Character target){
        return new StatusEffect("ward spell",target,ActionType.Magic,AttributeEnum.Defence,1.3f,0.5f,4);
    }
    public static StatusEffect poison(Character target){
        return new StatusEffect("poison",target,ActionType.Science,AttributeEnum.MaxHealth,0.85f,0.5f,5);
    }
    public static StatusEffect coffee(Character target) {
        return new StatusEffect("coffee", target, ActionType.Physical, AttributeEnum.Speed, 1.2f, 0.8f, 3);
    }
    public static StatusEffect freezingSpell(Character target) {
        return new StatusEffect("freezing spell", target, ActionType.Magic, AttributeEnum.Speed, 0.7f, 0.8f, 4);
    }



}