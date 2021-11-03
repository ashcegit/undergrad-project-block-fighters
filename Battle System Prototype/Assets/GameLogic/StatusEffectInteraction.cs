using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInteraction:Interaction
{   
    private AttributeModifier attributeModifier;
    private StatusEffect statusEffect;

    public StatusEffectInteraction(Character target,StatusEffect statusEffect):base(target){
        this.statusEffect=statusEffect;
    }

    public StatusEffect getStatusEffect(){return statusEffect;}
    public new InteractionEnum getResult(){
        if(Random.Range(0,1)<statusEffect.getChance()){return InteractionEnum.Hit;}
        else{return InteractionEnum.Miss;}
    }
    public AttributeModifier getAttributeModifier(){
        return new AttributeModifier(statusEffect.getTurns(),
                                     statusEffect.getAttribute(),
                                     statusEffect.getMultiplier());
    }
    public AttributeEnum getAttribute(){return statusEffect.getAttribute();}
    public float getMultiplier(){return statusEffect.getMultiplier();}
    public float getChance(){return statusEffect.getChance();}
    public int getTurns(){return statusEffect.getTurns();}
}
